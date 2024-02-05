using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		[StructLayout(LayoutKind.Sequential)]
		public struct D3DXFLOAT16 {
			public D3DXFLOAT16(float f) {
				this.value = 0;
				D3DX9MATH.D3DXFloat32To16Array(ref this, ref f, 1);
			}
			public D3DXFLOAT16(D3DXFLOAT16 f16) {
				this.value = f16.value;
			}
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            //// casting
			public static explicit operator float(D3DXFLOAT16 f16) {
				float f = 0.0f;
				D3DX9MATH.D3DXFloat16To32Array(ref f, ref f16, 1);
				return f;
			}
			//public static implicit operator float(D3DXFLOAT16 f16) {
			//    float f = 0.0f;
			//    D3DX9MATH.D3DXFloat16To32Array(ref f, ref f16, 1);
			//    return f;
			//}
			//// binary operators
			public static bool operator ==(D3DXFLOAT16 a, D3DXFLOAT16 b) {
				return (a.value == b.value);
			}
			public static bool operator !=(D3DXFLOAT16 a, D3DXFLOAT16 b) {
				return (a.value != b.value);
			}
			public ushort value;
		}
	}
}