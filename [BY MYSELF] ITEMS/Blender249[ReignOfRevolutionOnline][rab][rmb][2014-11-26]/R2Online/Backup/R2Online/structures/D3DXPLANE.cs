using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		[StructLayout(LayoutKind.Sequential)]
		public struct D3DXPLANE {
			public D3DXPLANE(float[] pf) {
				this.a = pf[0];
				this.b = pf[1];
				this.c = pf[2];
				this.d = pf[3];
			}
			public D3DXPLANE(D3DXFLOAT16[] pf) {
				this.a = 0;
				this.b = 0;
				this.c = 0;
				this.d = 0;
				D3DX9MATH.D3DXFloat16To32Array(ref this.a, ref pf[0], 4);
			}
			public D3DXPLANE(float fa, float fb, float fc, float fd) {
				this.a = fa;
				this.b = fb;
				this.c = fc;
				this.d = fd;
			}
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            // casting
			public static explicit operator float[](D3DXPLANE v) {
				float[] f = new float[4];
				f[0] = v.a;
				f[1] = v.b;
				f[2] = v.c;
				f[3] = v.d;
				return f;
			}
			// binary operators
			public static D3DXPLANE operator *(D3DXPLANE p, float f) {
				p.a *= f;
				p.b *= f;
				p.c *= f;
				p.d *= f;
				return p;
			}
			public static D3DXPLANE operator *(float f, D3DXPLANE p) {
				p.a *= f;
				p.b *= f;
				p.c *= f;
				p.d *= f;
				return p;
			}
			public static D3DXPLANE operator /(D3DXPLANE p, float f) {
				float fInv = 1.0f / f;
				p.a *= fInv;
				p.b *= fInv;
				p.c *= fInv;
				p.d *= fInv;
				return p;
			}
			// unary operators
			public static D3DXPLANE operator +(D3DXPLANE p) {
				return p;
			}
			public static D3DXPLANE operator -(D3DXPLANE p) {
				return new D3DXPLANE(-p.a, -p.b, -p.c, -p.d);
			}
			//friend D3DXPLANE operator * ( FLOAT, CONST D3DXPLANE& );
			public static bool operator ==(D3DXPLANE v1, D3DXPLANE v2) {
				return v1.a == v2.a && v1.b == v2.b && v1.c == v2.c && v1.d == v2.d;
			}
			public static bool operator !=(D3DXPLANE v1, D3DXPLANE v2) {
				return v1.a != v2.a || v1.b != v2.b || v1.c != v2.c || v1.d != v2.d;
			}
			public float a, b, c, d;
		}
	}
}