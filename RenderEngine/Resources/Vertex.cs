namespace RenderEngine
{
    internal struct Vertex
    {
        //Positions
        internal double PosX;
        internal double PosY;
        internal double PosZ;
        //Normals
        internal double NX;
        internal double NY;
        internal double NZ;

        internal Vertex(float posX, float posY, float posZ) : this()
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }

        internal Vertex(float posX, float posY, float posZ, float nX, float nY, float nZ) : this()
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            NX = nX;
            NY = nY;
            NZ = nZ;
        }

        internal static int NormalOffset()
        {
            return sizeof(double)*3;
        }
    }
}
