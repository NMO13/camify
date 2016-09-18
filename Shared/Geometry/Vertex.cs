using System.Runtime.InteropServices;

namespace Shared.Geometry
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
        public Vertex(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
            IsContourEdge = 1f;
        }
        public Vertex(double x, double y, double z, double nX, double nY, double nZ) : this()
        {
            X = x;
            Y = y;
            Z = z;
            NX = nX;
            NY = nY;
            NZ = nZ;
            IsContourEdge = 1f;
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
