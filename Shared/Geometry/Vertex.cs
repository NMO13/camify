namespace RenderEngine
{
    public struct Vertex
    {
        //Positions
        public double X;
        public double Y;
        public double Z;
        //Normals
        public double NX;
        public double NY;
        public double NZ;

        public Vertex(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vertex(float x, float y, float z, float nX, float nY, float nZ) : this()
        {
            X = x;
            Y = y;
            Z = z;
            NX = nX;
            NY = nY;
            NZ = nZ;
        }

        public static int NormalOffset()
        {
            return sizeof(double)*3;
        }
    }
}
