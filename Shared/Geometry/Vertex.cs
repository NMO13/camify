using System;
using System.Runtime.InteropServices;

namespace RenderEngine
{
    public struct Vertex
    {
        //Positions
        public double X;
        public double Y;
        public double Z;
        //ContourEdge
        public float IsContourEdge;
        //Normals
        public double NX;
        public double NY;
        public double NZ;
        public Vertex(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
            IsContourEdge = 0f;
        }
        public Vertex(float x, float y, float z, float nX, float nY, float nZ) : this()
        {
            X = x;
            Y = y;
            Z = z;
            NX = nX;
            NY = nY;
            NZ = nZ;
            IsContourEdge = 0f;
        }
        public static int IsContourEdgeOffset()
        {
            return sizeof (double) * 3;
        }
        public static int NormalOffset()
        {
            return (int)Marshal.OffsetOf(typeof (Vertex), "NX");
        }
    }
}
