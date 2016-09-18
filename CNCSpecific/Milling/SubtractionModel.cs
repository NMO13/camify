using System;
using System.Collections.Generic;
using GeometryCalculation.BooleanOperations;
using GeometryCalculation.DataStructures;
using GraphicsEngine.HalfedgeMesh;
using MessageHandling;
using MessageHandling.Messages;
using MessageHandling.SnapshotFormat;
using Model;
using Shared.Geometry;
using Shared.Geometry.HalfedgeMesh;

namespace CNCSpecific.Milling
{
    public class SubtractionModel : AbstractModel
    {
        public static SubtractionModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SubtractionModel();
                }
                return _instance;
            }
        }

        private event ModelHandler<AbstractModel> Changed;
        private List<DeformableObject> _tools = new List<DeformableObject>();
        private List<DeformableObject> _roughParts = new List<DeformableObject>();
        private static SubtractionModel _instance;
        public NCProgram NCProgram { get; set; }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            Changed += abstractModel.ModelNotified;
        }

        public override void AttachObserver(IObserver observer)
        {
            Changed += observer.Notified;
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            if (message.MessageType == MessageType.NewRoughParts)
            {
                var meshMessage = message as HeMeshMessage;
                if (meshMessage == null)
                    return;
                AddRoughParts(meshMessage.GetMeshes);
            }
            else if (message.MessageType == MessageType.NewTools)
            {
                var meshMessage = message as HeMeshMessage;
                if (meshMessage == null)
                    return;
                AddTools(meshMessage.GetMeshes);
            }
            else if (message.MessageType == MessageType.ClearMeshes)
            {
                var meshMessage = message as HeMeshMessage;
                if (meshMessage == null)
                    return;
                _tools.Clear();
                _roughParts.Clear();
            }
            else if (message.MessageType == MessageType.MoveObject)
            {
                var transformationMessage = message as TransformationMessage;
                if (transformationMessage == null)
                    return;
                _tools[transformationMessage.ToolId].Translate(transformationMessage.Transformation);
            }
        }

        private void AddTools(List<HeMesh> getMeshes)
        {
            foreach (var mesh in getMeshes)
            {
                var tool = CreateDeformableObject(mesh);
                _tools.Add(tool);
            }
        }

        private void AddRoughParts(List<HeMesh> getMeshes)
        {
            foreach (var mesh in getMeshes)
            {
                var roughPart = CreateDeformableObject(mesh);
                roughPart.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
                roughPart.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
                _roughParts.Add(roughPart);
            }
        }

        public void SingleSubtractionStep(bool reversed = false)
        {
            if (_tools.Count < 1 || _roughParts.Count < 1)
                throw new Exception("Not enough tools and/or roughparts specified.");
            if(reversed)
                BooleanModeller.SubtractSweptVolume(_tools[0], _roughParts[0]);
            else
                BooleanModeller.SubtractSweptVolume(_roughParts[0], _tools[0]);
            //Changed(this, new MeshMessage(MessageType.ReplaceParts, new List<Mesh>() {_roughParts[0].ToMesh()}));
        }

        public void BuildSnapshotList(bool collectTsv)
        {
            DeformableObject tsv = new DeformableObject();
            SnapshotCollector collector = new SnapshotCollector(collectTsv);
            foreach (var path in NCProgram.PathList)
            {
                //TODO var tool = _tools.Find(x => x.Id == path.ActiveTool);
                tsv.SweepVolume(_tools[0], path.RelativePosition);
                BooleanModeller.SubtractSweptVolume(_roughParts[0], tsv, false);

                if(collectTsv)
                    collector.AddNextSnapshot(_roughParts[0].ToMesh(), tsv.ToMesh(), path.RelativePosition.Vector3d, path.ActiveTool);
                else
                {
                    collector.AddNextSnapshot(_roughParts[0].ToMesh(), null, path.RelativePosition.Vector3d, path.ActiveTool);
                }
                _tools[0].Translate(path.RelativePosition);
            }
            if (Changed != null)
                Changed(this, new SnapshotMessage(MessageType.NewSnapshotList, collector));
        }

        private DeformableObject CreateDeformableObject(HeMesh mesh)
        {
            return DeformableObjectFactory.Create(mesh);
        }

        public List<DeformableObject> RoughParts { get { return _roughParts;} }

        public List<DeformableObject> Tools { get { return _tools; } }

        public bool IsValidForBuilding
        {
            get { return NCProgram != null && NCProgram.PathList.Count > 0 && _tools.Count > 0 && _roughParts.Count > 0; }
        }
    }
}
