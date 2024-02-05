using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
    namespace D3DX9 {

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXMATRIX {
            [DllImport("kernel32.dll")]
            private static extern void RtlMoveMemory([In, Out] ref float pOut, [In] ref float pIn, [In] int nSize);

            public D3DXMATRIX(float[] pMat) {
                this._11 = this._12 = this._13 = this._14 = 0.0f;
                this._21 = this._22 = this._23 = this._24 = 0.0f;
                this._31 = this._32 = this._33 = this._34 = 0.0f;
                this._41 = this._42 = this._43 = this._44 = 0.0f;
                RtlMoveMemory(ref this._11, ref pMat[0], Marshal.SizeOf(typeof(D3DXMATRIX)));
            }
            public D3DXMATRIX(float[,] pMat) {
                this._11 = this._12 = this._13 = this._14 = 0.0f;
                this._21 = this._22 = this._23 = this._24 = 0.0f;
                this._31 = this._32 = this._33 = this._34 = 0.0f;
                this._41 = this._42 = this._43 = this._44 = 0.0f;
                RtlMoveMemory(ref this._11, ref pMat[0, 0], Marshal.SizeOf(typeof(D3DXMATRIX)));
            }
            public D3DXMATRIX(D3DXFLOAT16[] pf) {
                this._11 = this._12 = this._13 = this._14 = 0.0f;
                this._21 = this._22 = this._23 = this._24 = 0.0f;
                this._31 = this._32 = this._33 = this._34 = 0.0f;
                this._41 = this._42 = this._43 = this._44 = 0.0f;
                D3DX9MATH.D3DXFloat16To32Array(ref this._11, ref pf[0], 16);
            }
            public D3DXMATRIX(float _11, float _12, float _13, float _14,
               float _21, float _22, float _23, float _24,
               float _31, float _32, float _33, float _34,
               float _41, float _42, float _43, float _44) {
                this._11 = _11; this._12 = _12; this._13 = _13; this._14 = _14;
                this._21 = _21; this._22 = _22; this._23 = _23; this._24 = _24;
                this._31 = _31; this._32 = _32; this._33 = _33; this._34 = _34;
                this._41 = _41; this._42 = _42; this._43 = _43; this._44 = _44;
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            public static implicit operator float[](D3DXMATRIX v) {
                float[] f = new float[16];
                RtlMoveMemory(ref f[0], ref v._11, Marshal.SizeOf(typeof(D3DXMATRIX)));
                return f;
            }
            public static implicit operator float[,](D3DXMATRIX v) {
                float[,] f = new float[4, 4];
                RtlMoveMemory(ref f[0, 0], ref v._11, Marshal.SizeOf(typeof(D3DXMATRIX)));
                return f;
            }
            public static implicit operator D3DXMATRIX(float[,] v) {
                return new D3DXMATRIX(v);
            }
            public float this[int iRow, int iCol] {
                // get and set accessors
                get {
                    float[,] f = this;
                    return f[iRow, iCol];
                }
                set {
                    float[,] f = this;
                    f[iRow, iCol] = value;
                    this = f;
                }
            }
            public static D3DXMATRIX operator *(D3DXMATRIX mat1, D3DXMATRIX mat2) {
                D3DX9MATH.D3DXMatrixMultiply(ref mat1, ref mat1, ref mat2);
                return mat1;
            }
            public static D3DXMATRIX operator *(D3DXMATRIX mat1, float f) {
                mat1._11 *= f; mat1._12 *= f; mat1._13 *= f; mat1._14 *= f;
                mat1._21 *= f; mat1._22 *= f; mat1._23 *= f; mat1._24 *= f;
                mat1._31 *= f; mat1._32 *= f; mat1._33 *= f; mat1._34 *= f;
                mat1._41 *= f; mat1._42 *= f; mat1._43 *= f; mat1._44 *= f;
                return mat1;
            }
            public static D3DXMATRIX operator +(D3DXMATRIX mat1, D3DXMATRIX mat2) {
                mat1._11 += mat2._11; mat1._12 += mat2._12; mat1._13 += mat2._13; mat1._14 += mat2._14;
                mat1._21 += mat2._21; mat1._22 += mat2._22; mat1._23 += mat2._23; mat1._24 += mat2._24;
                mat1._31 += mat2._31; mat1._32 += mat2._32; mat1._33 += mat2._33; mat1._34 += mat2._34;
                mat1._41 += mat2._41; mat1._42 += mat2._42; mat1._43 += mat2._43; mat1._44 += mat2._44;
                return mat1;
            }
            public static D3DXMATRIX operator -(D3DXMATRIX mat1, D3DXMATRIX mat2) {
                mat1._11 -= mat2._11; mat1._12 -= mat2._12; mat1._13 -= mat2._13; mat1._14 -= mat2._14;
                mat1._21 -= mat2._21; mat1._22 -= mat2._22; mat1._23 -= mat2._23; mat1._24 -= mat2._24;
                mat1._31 -= mat2._31; mat1._32 -= mat2._32; mat1._33 -= mat2._33; mat1._34 -= mat2._34;
                mat1._41 -= mat2._41; mat1._42 -= mat2._42; mat1._43 -= mat2._43; mat1._44 -= mat2._44;
                return mat1;
            }
            public static D3DXMATRIX operator /(D3DXMATRIX mat1, float f) {
                float fInv = 1.0f / f;
                mat1._11 *= fInv; mat1._12 *= fInv; mat1._13 *= fInv; mat1._14 *= fInv;
                mat1._21 *= fInv; mat1._22 *= fInv; mat1._23 *= fInv; mat1._24 *= fInv;
                mat1._31 *= fInv; mat1._32 *= fInv; mat1._33 *= fInv; mat1._34 *= fInv;
                mat1._41 *= fInv; mat1._42 *= fInv; mat1._43 *= fInv; mat1._44 *= fInv;
                return mat1;
            }
            public static D3DXMATRIX operator +(D3DXMATRIX mat1) {
                return mat1;
            }
            public static D3DXMATRIX operator -(D3DXMATRIX mat1) {
                return new D3DXMATRIX(-mat1._11, -mat1._12, -mat1._13, -mat1._14,
                              -mat1._21, -mat1._22, -mat1._23, -mat1._24,
                              -mat1._31, -mat1._32, -mat1._33, -mat1._34,
                              -mat1._41, -mat1._42, -mat1._43, -mat1._44);
            }
            public static bool operator ==(D3DXMATRIX mat1, D3DXMATRIX mat2) {
                float[] f1 = mat1;
                float[] f2 = mat2;
                bool bTest = true;
                for (int i = 0; i < 16; i++) {
                    if (f1[i] != f2[i]) {
                        bTest = false;
                        break;
                    }
                }
                return bTest;
            }
            public static bool operator !=(D3DXMATRIX mat1, D3DXMATRIX mat2) {
                return !(mat1 == mat2);
            }

            public float _11, _12, _13, _14;
            public float _21, _22, _23, _24;
            public float _31, _32, _33, _34;
            public float _41, _42, _43, _44;
        }
    }
}