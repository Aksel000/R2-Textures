using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		[StructLayout(LayoutKind.Sequential)]
		public struct D3DXVECTOR4 {
			public D3DXVECTOR4(float[] pf) {
				this.x = pf[0];
				this.y = pf[1];
				this.z = pf[2];
				this.w = pf[3];
			}
			public D3DXVECTOR4(D3DXFLOAT16[] pf) {
				this.x = 0;
				this.y = 0;
				this.z = 0;
				this.w = 0;
				D3DX9MATH.D3DXFloat16To32Array(ref this.x, ref pf[0], 4);
			}
			public D3DXVECTOR4(D3DXVECTOR3 v, float f) {
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
				this.w = f;
			}
			public D3DXVECTOR4(float fx, float fy, float fz, float fw) {
				this.x = fx;
				this.y = fy;
				this.z = fz;
				this.w = fw;
			}
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            // casting
			public static explicit operator float[](D3DXVECTOR4 v) {
				float[] f = new float[4];
				f[0] = v.x;
				f[1] = v.y;
				f[2] = v.z;
				f[3] = v.w;
				return f;
			}
			// binary operators
			public static D3DXVECTOR4 operator +(D3DXVECTOR4 v1, D3DXVECTOR4 v2) {
				v1.x += v2.x;
				v1.y += v2.y;
				v1.z += v2.z;
				v1.w += v2.w;
				return v1;
			}
			public static D3DXVECTOR4 operator -(D3DXVECTOR4 v1, D3DXVECTOR4 v2) {
				v1.x -= v2.x;
				v1.y -= v2.y;
				v1.z -= v2.z;
				v1.w -= v2.w;
				return v1;
			}
			public static D3DXVECTOR4 operator *(D3DXVECTOR4 v, float f) {
				v.x *= f;
				v.y *= f;
				v.z *= f;
				v.w *= f;
				return v;
			}
			public static D3DXVECTOR4 operator /(D3DXVECTOR4 v, float f) {
				float fInv = 1.0f / f;
				v.x *= fInv;
				v.y *= fInv;
				v.z *= fInv;
				v.w *= fInv;
				return v;
			}
			// unary operators
			public static D3DXVECTOR4 operator +(D3DXVECTOR4 v) {
				return v;
			}
			public static D3DXVECTOR4 operator -(D3DXVECTOR4 v) {
				return new D3DXVECTOR4(-v.x, -v.y, -v.z, -v.w);
			}
			//friend D3DXVECTOR4 operator * ( FLOAT, CONST D3DXVECTOR4& );
			public static bool operator ==(D3DXVECTOR4 v1, D3DXVECTOR4 v2) {
				return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;
			}
			public static bool operator !=(D3DXVECTOR4 v1, D3DXVECTOR4 v2) {
				return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w;
			}
			public float x, y, z, w;
		}
	}
}