using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		[StructLayout(LayoutKind.Sequential)]
		public struct D3DXATTRIBUTERANGE {
			public uint AttribId;
			public uint FaceStart;
			public uint FaceCount;
			public uint VertexStart;
			public uint VertexCount;
		}
	}
}