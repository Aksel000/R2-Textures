using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		[StructLayout(LayoutKind.Sequential)]
		public struct D3DXVECTOR2 {
			public D3DXVECTOR2(float[] pf) {
				this.x = pf[0];
				this.y = pf[1];
			}
			public D3DXVECTOR2(D3DXFLOAT16[] pf) {
				this.x = 0;
				this.y = 0;
				D3DX9MATH.D3DXFloat16To32Array(ref this.x, ref pf[0], 2);
			}
			public D3DXVECTOR2(float fx, float fy) {
				this.x = fx;
				this.y = fy;
			}
            public override string ToString() {
                return String.Format("{0}, {1}", this.x, this.y);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            // casting
			public static explicit operator float[](D3DXVECTOR2 v) {
				float[] f = new float[2];
				f[0] = v.x;
				f[1] = v.y;
				return f;
			}
			// binary operators
			public static D3DXVECTOR2 operator +(D3DXVECTOR2 v1, D3DXVECTOR2 v2) {
				v1.x += v2.x;
				v1.y += v2.y;
				return v1;
			}
			public static D3DXVECTOR2 operator -(D3DXVECTOR2 v1, D3DXVECTOR2 v2) {
				v1.x -= v2.x;
				v1.y -= v2.y;
				return v1;
			}
			public static D3DXVECTOR2 operator *(D3DXVECTOR2 v, float f) {
				v.x *= f;
				v.y *= f;
				return v;
			}
			public static D3DXVECTOR2 operator /(D3DXVECTOR2 v, float f) {
				float fInv = 1.0f / f;
				v.x *= fInv;
				v.y *= fInv;
				return v;
			}
			// unary operators
			public static D3DXVECTOR2 operator +(D3DXVECTOR2 v) {
				return v;
			}
			public static D3DXVECTOR2 operator -(D3DXVECTOR2 v) {
				return new D3DXVECTOR2(-v.x, -v.y);
			}
			public static bool operator ==(D3DXVECTOR2 v1, D3DXVECTOR2 v2) {
				return v1.x == v2.x && v1.y == v2.y;
			}
			public static bool operator !=(D3DXVECTOR2 v1, D3DXVECTOR2 v2) {
				return v1.x != v2.x || v1.y != v2.y;
			}
			public float x, y;
		}
	}
}