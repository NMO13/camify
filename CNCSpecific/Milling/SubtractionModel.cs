using System;
using System.Collections.Generic;
using GeometryCalculation.BooleanOperations;
using GeometryCalculation.DataStructures;
using GraphicsEngine.HalfedgeMesh;
using MessageHandling;
using MessageHandling.Messages;
using Model;
using Shared.Geometry;

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
        public List<Mesh> SnapshotList = new List<Mesh>();
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
                AddDeformableObjectsToList(meshMessage.GetMeshes, _roughParts);
            }
            else if (message.MessageType == MessageType.NewTools)
            {
                var meshMessage = message as HeMeshMessage;
                if (meshMessage == null)
                    return;
                AddDeformableObjectsToList(meshMessage.GetMeshes, _tools);
            }
            else if (message.MessageType == MessageType.ClearMeshes)
            {
                var meshMessage = message as HeMeshMessage;
                if (meshMessage == null)
                    return;
                _tools.Clear();
                _roughParts.Clear();
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

        public void BuildSnapshotList()
        { 
            //DeformableObject tsv = new DeformableObject();
            //SnapshotCollector collector = new SnapshotCollector();
            //foreach (var path in NCProgram.PathList)
            //{
            //    //TODO var tool = _tools.Find(x => x.Id == path.ActiveTool);
            //    tsv.SweepVolume(_tools[0], path.RelativePosition);
            //    BooleanModeller.SubtractSweptVolume(_roughParts[0], tsv, false);
            //    collector.AddNextMesh(_roughParts[0].GetMesh());
            //    _tools[0].Translate(path.RelativePosition);
            //}
            //if(Changed != null)
            //    Changed(this, new MeshMessage(MessageType.SnapshotList, collector.Meshes));
            //SnapshotList = collector.Meshes;
        }

        private void AddDeformableObjectsToList(List<HeMesh> getMeshes, List<DeformableObject> deformableObjects)
        {
            foreach (var mesh in getMeshes)
            {
                deformableObjects.Add(CreateDeformableObject(mesh));
            }
        }

        private DeformableObject CreateDeformableObject(HeMesh mesh)
        {
            return DeformableObjectFactory.Create(mesh);
        }

        public List<DeformableObject> RoughParts { get { return _roughParts;} }

        public List<DeformableObject> Tools { get { return _tools; } }
    }
}
