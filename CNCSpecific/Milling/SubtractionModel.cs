using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCSpecific.Milling;
using GeometryCalculation.DataStructures;
using GraphicsEngine.Geometry.Boolean_Ops;
using Shared;
using MessageHandling;
using Shared.Geometry;

namespace Model
{
    public class SubtractionModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        private List<NCPath> _pathList = new List<NCPath>();
        private List<DeformableObject> _tools = new List<DeformableObject>();
        private List<DeformableObject> _roughParts = new List<DeformableObject>();
        
        public void AddPath(Vector3m path, int toolNumber)
        {
            NCPath ncPath = new NCPath();
            ncPath.ActiveTool = toolNumber;
            ncPath.RelativePosition = path;
            _pathList.Add(ncPath);
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            Changed += abstractModel.ModelNotified;
        }

        public override void AttachObserver(IObserver observer)
        {
            Changed += observer.Notify;
        }

        public void CalcSubtractions()
        {
            
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            if (message.MessageType == MessageType.NewRoughParts)
            {
                var meshMessage = message as MeshMessage;
                AddDeformableObjectsToList(meshMessage.GetMeshes, _roughParts);
            }
            else if (message.MessageType == MessageType.NewTools)
            {
                var meshMessage = message as MeshMessage;
                AddDeformableObjectsToList(meshMessage.GetMeshes, _tools);
            }
        }

        public void BuildSnapshotList()
        {
            DeformableObject tsv = new DeformableObject();
            SnapshotCollector collector = new SnapshotCollector();
            foreach (var path in _pathList)
            {
                //TODO var tool = _tools.Find(x => x.Id == path.ActiveTool);
                tsv.SweepVolume(_tools[0], path.RelativePosition);
                BooleanModeller.SubtractSweptVolume(_roughParts[0], tsv, false);
                collector.AddNextMesh(_roughParts[0].GetMesh());
                _tools[0].Translate(path.RelativePosition);
            }
        }

        private void AddDeformableObjectsToList(List<Mesh> getMeshes, List<DeformableObject> deformableObjects)
        {
            foreach (var mesh in getMeshes)
            {
                deformableObjects.Add(CreateDeformableObject(mesh));
            }
        }

        private DeformableObject CreateDeformableObject(Mesh mesh)
        {
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            return obj;
        }
    }
}
