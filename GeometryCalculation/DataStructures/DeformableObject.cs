using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeometryCalculation.Simplification;
using GraphicsEngine.Geometry.CollisionCheck;
using GraphicsEngine.HalfedgeMesh;
using GraphicsEngine.HalfedgeMesh.Simplification;
using GraphicsEngine.HalfedgeMeshProcessing;
using Microsoft.SolverFoundation.Common;
using Shared;
using Shared.Geometry;
using Shared.Geometry.CollisionCheck;
using Shared.Geometry.HalfedgeMesh;

namespace GeometryCalculation.DataStructures
{
    public class DeformableObject
    {
        internal HeMesh HeMesh { get; set; }

        public enum PostprocessAlgorithm
        {
            ContourCalculator, PlanarMerge, StraightEdgeReduction
        }

        internal BoundingVolumeHierarchy Bvh;
        internal readonly List<FacePair> FacePairs = new List<FacePair>();
        private uint _bvhMaxItemCount;
        private List<IPostProcess> _postProcessSteps = new List<IPostProcess>();
        internal ContourGroupManager ContourGroupManager = new ContourGroupManager();

        public DeformableObject(uint bvhMaxtItemCount = 2)
        {
            _bvhMaxItemCount = bvhMaxtItemCount;
            HeMesh = new HeMesh();
        }

        public bool Intersect(DeformableObject other)
        {
            FacePairs.Clear();
            BvhCollisionTest(Bvh.Root, other.Bvh.Root);
            return FacePairs.Count != 0;
        }

        public void ExecutePostProcesses()
        {
            _postProcessSteps.ForEach(x => x.Execute(this));
        }

        internal void LoadMesh(Mesh mesh)
        {
            if (mesh.Vertices.Length < 3)
                throw new Exception("Mesh must have at least 3 vertices");
            foreach (var vertex in mesh.Vertices)
            {
                HeMesh.AddVertex(new HeVertex(vertex.X, vertex.Y, vertex.Z));
            }
            for (int i = 0; i < mesh.Indices.Length; i += 3)
            {
                Vector3d[] normals = null;
                HeMesh.AddFace(mesh.Indices[i], mesh.Indices[i + 1], mesh.Indices[i + 2]);
            }
        }

        private void InstallPostprocessStep(IPostProcess p)
        {
            foreach (var postProcessStep in _postProcessSteps)
            {
                if (postProcessStep.GetType() == p.GetType())
                    return;
            }
            _postProcessSteps.Add(p);
            HeMesh.AddObserver(p);
        }
        public void AddPostProcessStep(PostprocessAlgorithm algorithm)
        {
            if (algorithm == PostprocessAlgorithm.ContourCalculator)
            {
                InstallPostprocessStep(new ContourCalculator());
            }
            if (algorithm == PostprocessAlgorithm.PlanarMerge ||
                algorithm == PostprocessAlgorithm.StraightEdgeReduction)
            {
                InstallPostprocessStep(new ContourCalculator());
            }
            if (algorithm == PostprocessAlgorithm.PlanarMerge)
            {
                InstallPostprocessStep(new PlanarMerge());
            }
            if (algorithm == PostprocessAlgorithm.StraightEdgeReduction)
            {
                InstallPostprocessStep(new StraightEdgeReduction());
            }
        }

        public void CheckSanity()
        {
            if(HeMesh == null)
                throw new Exception("Mesh hasn't been initialized");
            foreach (var halfedge in HeMesh.HalfedgeList)
            {
                if (halfedge.IncidentFace == null)
                    throw new Exception("Incident face for halfedge " + halfedge + " is null");
                if(halfedge.Normal == null)
                    throw new Exception("Normal is null");
                if (halfedge.Next == null)
                    throw new Exception("Halfedge.next is null");
                if (halfedge.Prev == null)
                    throw new Exception("Halfedge.prev is null");
                if(halfedge.Twin == null)
                    throw new Exception("Halfedge.twin is null");
            }

            foreach (var face in HeMesh.FaceList)
            {
                if(face.OuterComponent == null)
                    throw new Exception("Face.Outercomponent is null");
                if(face.OuterComponent.Next == null)
                    throw new Exception("Face.Outercomponent.Next is null");
                if (face.OuterComponent.Next.Next == null)
                    throw new Exception("Face.Outercomponent.Next.Next is null");
                if(!face.IsValid)
                    throw new Exception("Illegal Face");
            }

            for(int i = 0; i < HeMesh.VertexList.Count; i++)
            {
                if(HeMesh.VertexList[i] != null && HeMesh.VertexList[i].Index != i)
                    throw new Exception("Vertex index not correct");

                if (HeMesh.VertexList[i] != null && HeMesh.VertexList[i].IncidentEdges.Count < 3)
                    throw new Exception("Vertex has too few incident edges");
            }
        }

        private void BvhCollisionTest(BoundingVolumeHierarchyNode a, BoundingVolumeHierarchyNode b)
        {
            if (!a.AABB.Overlap(b.AABB))
                return;

            if (a.IsLeaf() && b.IsLeaf())
            {
                foreach (var triangleA in a.Faces)
                    foreach (var triangleB in b.Faces)
                    {
                        // The second parameter in the list will be tested against the first parameter wether they intersect
                        FacePairs.Add(new FacePair(triangleA, triangleB));
                    }
            }
            else
            {
                if (!a.IsLeaf())
                {
                    Debug.Assert(a.Left != null && a.Right != null);
                    BvhCollisionTest(a.Left, b);
                    BvhCollisionTest(a.Right, b);
                }
                else
                {
                    Debug.Assert(b.Left != null && b.Right != null);
                    BvhCollisionTest(a, b.Left);
                    BvhCollisionTest(a, b.Right);
                }
            }
        }

        public void Initialize(Mesh mesh)
        {
            if (HeMesh.VertexList.Count != 0)
                throw new Exception("Cannot reinitialize deformable object");
            LoadMesh(mesh);
            BuildBvh();
        }

        public void BuildBvh()
        {
            Bvh = new BoundingVolumeHierarchy(HeMesh.FaceList.ToArray(), _bvhMaxItemCount);
            HeMesh.AddObserver(Bvh);
        }

        public void Translate(Vector3m amount)
        {
            HeMesh.Translate(amount);
        }

        public void TranslateAndBuildBvh(Vector3m amount)
        {
            Translate(amount);
            BuildBvh();
        }

        public void SweepVolume(DeformableObject obj, Vector3m direction)
        {
            NewMesh(obj.HeMesh, Vector3m.Zero());
            SweptVolumeCalculator.Calculate(HeMesh, direction);
            BuildBvh();
        }

        private void NewMesh(HeMesh heMesh, Vector3m vector3m)
        {
            HeMesh.ResetMesh(heMesh, vector3m);
        }
    
        public DeformableObject Clone(Vector3m translate)
        {
            if(translate == null)
                throw new ArgumentException("translate must not be null");
            DeformableObject obj = new DeformableObject(_bvhMaxItemCount);
            obj.HeMesh = new HeMesh(HeMesh, translate);
            obj.BuildBvh();
            return obj;
        }
        public Mesh ToMesh()
        {
            return new Mesh(HeMesh);
        }
    }

}
