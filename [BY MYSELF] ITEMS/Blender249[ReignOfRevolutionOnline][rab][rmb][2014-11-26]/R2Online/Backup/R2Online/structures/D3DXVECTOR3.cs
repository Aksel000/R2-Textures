using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace DirectX9 {
    namespace D3DX9 {

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR3 {
            public D3DXVECTOR3(float[] pf) {
                this.x = pf[0];
                this.y = pf[1];
                this.z = pf[2];
            }
            public D3DXVECTOR3(D3DXFLOAT16[] pf) {
                this.x = 0;
                this.y = 0;
                this.z = 0;
                D3DX9MATH.D3DXFloat16To32Array(ref this.x, ref pf[0], 3);
            }
            public D3DXVECTOR3(float fx, float fy, float fz) {
                this.x = fx;
                this.y = fy;
                this.z = fz;
            }
            public D3DXVECTOR3(BinaryReader br) {
                try {
                    this.x = br.ReadSingle();
                    this.y = br.ReadSingle();
                    this.z = br.ReadSingle();
                } catch (Exception e) {
                    MessageBox.Show(e.ToString());
                    this.x = this.y = this.z = 0.0f;
                }
            }
            public override string ToString() {
                return String.Format("{0}, {1}, {2}", this.x, this.y, this.z);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            // casting
            public static explicit operator float[](D3DXVECTOR3 v) {
                float[] f = new float[3];
                f[0] = v.x;
                f[1] = v.y;
                f[2] = v.z;
                return f;
            }
            // binary operators
            public static D3DXVECTOR3 operator +(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                v1.x += v2.x;
                v1.y += v2.y;
                v1.z += v2.z;
                return v1;
            }
            public static D3DXVECTOR3 operator -(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                v1.x -= v2.x;
                v1.y -= v2.y;
                v1.z -= v2.z;
                return v1;
            }
            public static D3DXVECTOR3 operator *(D3DXVECTOR3 v, float f) {
                v.x *= f;
                v.y *= f;
                v.z *= f;
                return v;
            }
            public static D3DXVECTOR3 operator /(D3DXVECTOR3 v, float f) {
                float fInv = 1.0f / f;
                v.x *= fInv;
                v.y *= fInv;
                v.z *= fInv;
                return v;
            }
            // unary operators
            public static D3DXVECTOR3 operator +(D3DXVECTOR3 v) {
                return v;
            }
            public static D3DXVECTOR3 operator -(D3DXVECTOR3 v) {
                return new D3DXVECTOR3(-v.x, -v.y, -v.z);
            }
            //friend D3DXVECTOR3 operator * ( FLOAT, CONST D3DXVECTOR3& );
            public static bool operator ==(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
            }
            public static bool operator !=(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
            }
            public static bool operator >(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                return v1.x > v2.x && v1.y > v2.y && v1.z > v2.z;
            }
            public static bool operator <(D3DXVECTOR3 v1, D3DXVECTOR3 v2) {
                return v1.x < v2.x || v1.y < v2.y || v1.z < v2.z;
            }
            public float x, y, z;
        }
    }
}