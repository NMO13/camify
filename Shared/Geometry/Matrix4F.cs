namespace Shared.Geometry
{
    public class Matrix4F
    {
        private float[,] _m;
        private float _scl = 1.0f;
        private Vector3F _pan = new Vector3F();

        public Matrix4F()
        {
            SetIdentity();
        }

        public void get_Renamed(float[] dest)
        {
            int k = 0;
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    dest[k] = _m[j, i];
                    k++;
                }
        }

        public void SetIdentity()
        {
            _m = new float[4, 4]; // set to zero
            for (int i = 0; i <= 3; i++) _m[i, i] = 1.0f;
        }

        public void SetRenamed(Matrix4F m1)
        {
            _m = m1._m;
        }

        public static void MatrixMultiply(Matrix4F m1, Matrix4F m2)
        {
            float[] mulMat = new float[16];
            float elMat = 0.0f;
            int k = 0;

            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    for (int l = 0; l <= 3; l++) elMat += m1._m[i, l] * m2._m[l, j];
                    mulMat[k] = elMat;
                    elMat = 0.0f;
                    k++;
                }

            k = 0;
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    m1._m[i, j] = mulMat[k];
                    k++;
                }
        }

        public Quat4F Rotation
        {
            set
            {
                float n, s;
                float xs, ys, zs;
                float wx, wy, wz;
                float xx, xy, xz;
                float yy, yz, zz;

                _m = new float[4, 4];

                n = (value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z) + (value.W * value.W);
                s = (n > 0.0f) ? 2.0f / n : 0.0f;

                xs = value.X * s;
                ys = value.Y * s;
                zs = value.Z * s;
                wx = value.W * xs;
                wy = value.W * ys;
                wz = value.W * zs;
                xx = value.X * xs;
                xy = value.X * ys;
                xz = value.X * zs;
                yy = value.Y * ys;
                yz = value.Y * zs;
                zz = value.Z * zs;

                // rotation
                _m[0, 0] = 1.0f - (yy + zz);
                _m[0, 1] = xy - wz;
                _m[0, 2] = xz + wy;

                _m[1, 0] = xy + wz;
                _m[1, 1] = 1.0f - (xx + zz);
                _m[1, 2] = yz - wx;

                _m[2, 0] = xz - wy;
                _m[2, 1] = yz + wx;
                _m[2, 2] = 1.0f - (xx + yy);

                _m[3, 3] = 1.0f;

                // translation (pan)
                _m[0, 3] = _pan.X;
                _m[1, 3] = _pan.Y;

                // scale (zoom)
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        _m[i, j] *= _scl;


            }
        }

        public float Scale
        {
            set { _scl = value; }

        }

        public Vector3F Pan
        {
            set { _pan = value; }

        }

    }
}
