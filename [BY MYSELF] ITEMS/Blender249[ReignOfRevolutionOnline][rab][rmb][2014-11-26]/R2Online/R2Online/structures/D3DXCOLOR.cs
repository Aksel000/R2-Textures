using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXCOLOR {
            public D3DXCOLOR(uint dw) {
                float f = 1.0f / 255.0f;
                this.r = f * (float)(byte)(dw >> 16);
                this.g = f * (float)(byte)(dw >> 8);
                this.b = f * (float)(byte)(dw >> 0);
                this.a = f * (float)(byte)(dw >> 24);
            }
            public D3DXCOLOR(float[] pf) {
                this.r = pf[0];
                this.g = pf[1];
                this.b = pf[2];
                this.a = pf[3];
            }
            public D3DXCOLOR(D3DXFLOAT16[] pf) {
                this.r = this.g = this.b = this.a = 0;
                D3DX9MATH.D3DXFloat16To32Array(ref this.r, ref pf[0], 4);
            }
            public D3DXCOLOR(float fa, float fb, float fc, float fd) {
                this.r = fa;
                this.g = fb;
                this.b = fc;
                this.a = fd;
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            // casting
            public static explicit operator float[](D3DXCOLOR v) {
                float[] f = new float[4];
                f[0] = v.r;
                f[1] = v.g;
                f[2] = v.b;
                f[3] = v.a;
                return f;
            }
            public static explicit operator uint(D3DXCOLOR c) {
                uint dwR = c.r >= 1.0f ? 0xff : c.r <= 0.0f ? 0x00 : (uint)(c.r * 255.0f + 0.5f);
                uint dwG = c.g >= 1.0f ? 0xff : c.g <= 0.0f ? 0x00 : (uint)(c.g * 255.0f + 0.5f);
                uint dwB = c.b >= 1.0f ? 0xff : c.b <= 0.0f ? 0x00 : (uint)(c.b * 255.0f + 0.5f);
                uint dwA = c.a >= 1.0f ? 0xff : c.a <= 0.0f ? 0x00 : (uint)(c.a * 255.0f + 0.5f);

                return (dwA << 24) | (dwR << 16) | (dwG << 8) | dwB;
            }
            // binary operators
            public static D3DXCOLOR operator *(D3DXCOLOR p, float f) {
                p.r *= f;
                p.g *= f;
                p.b *= f;
                p.a *= f;
                return p;
            }
            public static D3DXCOLOR operator *(float f, D3DXCOLOR p) {
                p.r *= f;
                p.g *= f;
                p.b *= f;
                p.a *= f;
                return p;
            }
            public static D3DXCOLOR operator /(D3DXCOLOR p, float f) {
                float fInv = 1.0f / f;
                p.r *= fInv;
                p.g *= fInv;
                p.b *= fInv;
                p.a *= fInv;
                return p;
            }
            // unary operators
            public static D3DXCOLOR operator +(D3DXCOLOR p) {
                return p;
            }
            public static D3DXCOLOR operator -(D3DXCOLOR p) {
                return new D3DXCOLOR(-p.r, -p.g, -p.b, -p.a);
            }
            public static bool operator ==(D3DXCOLOR v1, D3DXCOLOR v2) {
                return v1.r == v2.r && v1.g == v2.g && v1.b == v2.b && v1.a == v2.a;
            }
            public static bool operator !=(D3DXCOLOR v1, D3DXCOLOR v2) {
                return v1.r != v2.r || v1.g != v2.g || v1.b != v2.b || v1.a != v2.a;
            }
            public float r, g, b, a;
        }
    }
}