using System;
using System.Runtime.InteropServices;
using DirectX9;

namespace DirectX9 {

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DRESOURCESTATS {
		// Data collected since last Present()
		[MarshalAs(UnmanagedType.Bool)]
		bool bThrashing;             /* indicates if thrashing */
		uint ApproxBytesDownloaded;  /* Approximate number of bytes downloaded by resource manager */
		uint NumEvicts;              /* number of objects evicted */
		uint NumVidCreates;          /* number of objects created in video memory */
		uint LastPri;                /* priority of last object evicted */
		uint NumUsed;                /* number of objects set to the device */
		uint NumUsedInVidMem;        /* number of objects set to the device, which are already in video memory */
		// Persistent data
		uint WorkingSet;             /* number of objects in video memory */
		uint WorkingSetBytes;        /* number of bytes in video memory */
		uint TotalManaged;           /* total number of managed objects */
		uint TotalBytes;             /* total number of bytes of managed objects */
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DVERTEXELEMENT9 {
		public short Stream;     // Stream index
		public short Offset;     // Offset in the stream in bytes
		public byte Type;       // Data type
		public byte Method;     // Processing method
		public byte Usage;      // Semantics
		public byte UsageIndex; // Semantic index
		public D3DVERTEXELEMENT9(short Stream, short Offset, byte Type, byte Method, byte Usage, byte UsageIndex) {
			this.Stream = Stream;
			this.Offset = Offset;
			this.Type = Type;
			this.Method = Method;
			this.Usage = Usage;
			this.UsageIndex = UsageIndex;
		}
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DVECTOR {
		public float x;
		public float y;
		public float z;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DCOLORVALUE {
		public float r;
		public float g;
		public float b;
		public float a;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DMATRIX {
		public float _11, _12, _13, _14;
		public float _21, _22, _23, _24;
		public float _31, _32, _33, _34;
		public float _41, _42, _43, _44;

	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DVIEWPORT9 {
		public uint X;
		public uint Y;            /* Viewport Top left */
		public uint Width;
		public uint Height;       /* Viewport Dimensions */
		public float MinZ;         /* Min/max of clip Volume */
		public float MaxZ;
	} ;

	public enum D3DSHADEMODE {
		D3DSHADE_FLAT = 1,
		D3DSHADE_GOURAUD = 2,
		D3DSHADE_PHONG = 3,
		D3DSHADE_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DFILLMODE {
		D3DFILL_POINT = 1,
		D3DFILL_WIREFRAME = 2,
		D3DFILL_SOLID = 3,
		D3DFILL_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DBLEND {
		D3DBLEND_ZERO = 1,
		D3DBLEND_ONE = 2,
		D3DBLEND_SRCCOLOR = 3,
		D3DBLEND_INVSRCCOLOR = 4,
		D3DBLEND_SRCALPHA = 5,
		D3DBLEND_INVSRCALPHA = 6,
		D3DBLEND_DESTALPHA = 7,
		D3DBLEND_INVDESTALPHA = 8,
		D3DBLEND_DESTCOLOR = 9,
		D3DBLEND_INVDESTCOLOR = 10,
		D3DBLEND_SRCALPHASAT = 11,
		D3DBLEND_BOTHSRCALPHA = 12,
		D3DBLEND_BOTHINVSRCALPHA = 13,
		D3DBLEND_BLENDFACTOR = 14, /* Only supported if D3DPBLENDCAPS_BLENDFACTOR is on */
		D3DBLEND_INVBLENDFACTOR = 15, /* Only supported if D3DPBLENDCAPS_BLENDFACTOR is on */
		D3DBLEND_SRCCOLOR2 = 16,
		D3DBLEND_INVSRCCOLOR2 = 17,
		D3DBLEND_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DBLENDOP {
		D3DBLENDOP_ADD = 1,
		D3DBLENDOP_SUBTRACT = 2,
		D3DBLENDOP_REVSUBTRACT = 3,
		D3DBLENDOP_MIN = 4,
		D3DBLENDOP_MAX = 5,
		D3DBLENDOP_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DTEXTUREADDRESS {
		D3DTADDRESS_WRAP = 1,
		D3DTADDRESS_MIRROR = 2,
		D3DTADDRESS_CLAMP = 3,
		D3DTADDRESS_BORDER = 4,
		D3DTADDRESS_MIRRORONCE = 5,
		D3DTADDRESS_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DCULL {
		D3DCULL_NONE = 1,
		D3DCULL_CW = 2,
		D3DCULL_CCW = 3,
		D3DCULL_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DCMPFUNC {
		D3DCMP_NEVER = 1,
		D3DCMP_LESS = 2,
		D3DCMP_EQUAL = 3,
		D3DCMP_LESSEQUAL = 4,
		D3DCMP_GREATER = 5,
		D3DCMP_NOTEQUAL = 6,
		D3DCMP_GREATEREQUAL = 7,
		D3DCMP_ALWAYS = 8,
		D3DCMP_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DSTENCILOP {
		D3DSTENCILOP_KEEP = 1,
		D3DSTENCILOP_ZERO = 2,
		D3DSTENCILOP_REPLACE = 3,
		D3DSTENCILOP_INCRSAT = 4,
		D3DSTENCILOP_DECRSAT = 5,
		D3DSTENCILOP_INVERT = 6,
		D3DSTENCILOP_INCR = 7,
		D3DSTENCILOP_DECR = 8,
		D3DSTENCILOP_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DFOGMODE {
		D3DFOG_NONE = 0,
		D3DFOG_EXP = 1,
		D3DFOG_EXP2 = 2,
		D3DFOG_LINEAR = 3,
		D3DFOG_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DZBUFFERTYPE {
		D3DZB_FALSE = 0,
		D3DZB_TRUE = 1, // Z buffering
		D3DZB_USEW = 2, // W buffering
		D3DZB_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	// Primitives supported by draw-primitive API
	public enum D3DPRIMITIVETYPE {
		D3DPT_POINTLIST = 1,
		D3DPT_LINELIST = 2,
		D3DPT_LINESTRIP = 3,
		D3DPT_TRIANGLELIST = 4,
		D3DPT_TRIANGLESTRIP = 5,
		D3DPT_TRIANGLEFAN = 6,
		D3DPT_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	public enum D3DTRANSFORMSTATETYPE {
		D3DTS_VIEW = 2,
		D3DTS_PROJECTION = 3,
		D3DTS_TEXTURE0 = 16,
		D3DTS_TEXTURE1 = 17,
		D3DTS_TEXTURE2 = 18,
		D3DTS_TEXTURE3 = 19,
		D3DTS_TEXTURE4 = 20,
		D3DTS_TEXTURE5 = 21,
		D3DTS_TEXTURE6 = 22,
		D3DTS_TEXTURE7 = 23,
		D3DTS_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DCLIPSTATUS9 {
		uint ClipUnion;
		uint ClipIntersection;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DMATERIAL9 {
		public D3DCOLORVALUE Diffuse;        /* Diffuse color RGBA */
		public D3DCOLORVALUE Ambient;        /* Ambient color RGB */
		public D3DCOLORVALUE Specular;       /* Specular 'shininess' */
		public D3DCOLORVALUE Emissive;       /* Emissive color RGB */
		public float Power;          /* Sharpness if specular highlight */
	} ;

	public enum D3DLIGHTTYPE {
		D3DLIGHT_POINT = 1,
		D3DLIGHT_SPOT = 2,
		D3DLIGHT_DIRECTIONAL = 3,
		D3DLIGHT_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DLIGHT9 {
		public D3DLIGHTTYPE Type;            /* Type of light source */
		public D3DCOLORVALUE Diffuse;         /* Diffuse color of light */
		public D3DCOLORVALUE Specular;        /* Specular color of light */
		public D3DCOLORVALUE Ambient;         /* Ambient color of light */
		public D3DVECTOR Position;         /* Position in world space */
		public D3DVECTOR Direction;        /* Direction in world space */
		public float Range;            /* Cutoff range */
		public float Falloff;          /* Falloff */
		public float Attenuation0;     /* Constant attenuation */
		public float Attenuation1;     /* Linear attenuation */
		public float Attenuation2;     /* Quadratic attenuation */
		public float Theta;            /* Inner angle of spotlight cone */
		public float Phi;              /* Outer angle of spotlight cone */
	} ;


	/* Vertex Buffer Description */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DVERTEXBUFFER_DESC {
		public D3DFORMAT Format;
		public D3DRESOURCETYPE Type;
		public uint Usage;
		public D3DPOOL Pool;
		public uint Size;
		public uint FVF;
	} ;

	/* Index Buffer Description */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DINDEXBUFFER_DESC {
		public D3DFORMAT Format;
		public D3DRESOURCETYPE Type;
		public uint Usage;
		public D3DPOOL Pool;
		public uint Size;
	} ;

	/* Surface Description */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DSURFACE_DESC {
		public D3DFORMAT Format;
		public D3DRESOURCETYPE Type;
		public uint Usage;
		public D3DPOOL Pool;

		public D3DMULTISAMPLE_TYPE MultiSampleType;
		public uint MultiSampleQuality;
		public uint Width;
		public uint Height;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DVOLUME_DESC {
		public D3DFORMAT Format;
		public D3DRESOURCETYPE Type;
		public uint Usage;
		public D3DPOOL Pool;

		public uint Width;
		public uint Height;
		public uint Depth;
	} ;

	/* Structure for LockRect */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DLOCKED_RECT {
		public int Pitch;
		public IntPtr pBits;
	} ;

	/* Structures for LockBox */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DBOX {
		public uint Left;
		public uint Top;
		public uint Right;
		public uint Bottom;
		public uint Front;
		public uint Back;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DLOCKED_BOX {
		public int RowPitch;
		public int SlicePitch;
		public IntPtr pBits;
	} ;

	/* Structures for LockRange */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DRANGE {
		public uint Offset;
		public uint Size;
	} ;

	/* Structures for high order primitives */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DRECTPATCH_INFO {
		public uint StartVertexOffsetWidth;
		public uint StartVertexOffsetHeight;
		public uint Width;
		public uint Height;
		public uint Stride;
		public D3DBASISTYPE Basis;
		public D3DDEGREETYPE Degree;
	} ;

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DTRIPATCH_INFO {
		public uint StartVertexOffset;
		public uint NumVertices;
		public D3DBASISTYPE Basis;
		public D3DDEGREETYPE Degree;
	} ;


	#region Adapter Identifier
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
	public struct D3DADAPTER_IDENTIFIER9 {
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = (int)D3D9TYPES.MAX_DEVICE_IDENTIFIER_STRING)]
		public string Driver;
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = (int)D3D9TYPES.MAX_DEVICE_IDENTIFIER_STRING)]
		public string Description;
		/* Device name for GDI (ex. \\.\DISPLAY1) */
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
		/* Defined for 16 bit driver components */
		public uint DriverVersionLowPart;
		public uint DriverVersionHighPart;
		public uint VendorId;
		public uint DeviceId;
		public uint SubSysId;
		public uint Revision;
		public Guid DeviceIdentifier;
		public uint WHQLLevel;
	}
	#endregion

	#region Display Modes
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DDISPLAYMODE {
		public uint Width;
		public uint Height;
		public uint RefreshRate;
		public D3DFORMAT Format;
	} ;
	#endregion

	#region Formats
	public enum D3DFORMAT {
		D3DFMT_UNKNOWN = 0,
		D3DFMT_R8G8B8 = 20,
		D3DFMT_A8R8G8B8 = 21,
		D3DFMT_X8R8G8B8 = 22,
		D3DFMT_R5G6B5 = 23,
		D3DFMT_X1R5G5B5 = 24,
		D3DFMT_A1R5G5B5 = 25,
		D3DFMT_A4R4G4B4 = 26,
		D3DFMT_R3G3B2 = 27,
		D3DFMT_A8 = 28,
		D3DFMT_A8R3G3B2 = 29,
		D3DFMT_X4R4G4B4 = 30,
		D3DFMT_A2B10G10R10 = 31,
		D3DFMT_A8B8G8R8 = 32,
		D3DFMT_X8B8G8R8 = 33,
		D3DFMT_G16R16 = 34,
		D3DFMT_A2R10G10B10 = 35,
		D3DFMT_A16B16G16R16 = 36,
		D3DFMT_A8P8 = 40,
		D3DFMT_P8 = 41,
		D3DFMT_L8 = 50,
		D3DFMT_A8L8 = 51,
		D3DFMT_A4L4 = 52,
		D3DFMT_V8U8 = 60,
		D3DFMT_L6V5U5 = 61,
		D3DFMT_X8L8V8U8 = 62,
		D3DFMT_Q8W8V8U8 = 63,
		D3DFMT_V16U16 = 64,
		D3DFMT_A2W10V10U10 = 67,
		D3DFMT_UYVY = 0x59565955,
		D3DFMT_R8G8_B8G8 = 0x47424752,
		D3DFMT_YUY2 = 0x32595559,
		D3DFMT_G8R8_G8B8 = 0x42475247,
		D3DFMT_DXT1 = 0x31545844,
		D3DFMT_DXT2 = 0x32545844,
		D3DFMT_DXT3 = 0x33545844,
		D3DFMT_DXT4 = 0x34545844,
		D3DFMT_DXT5 = 0x35545844,
		//D3DFMT_UYVY                 = MAKEFOURCC('U', 'Y', 'V', 'Y'),
		//D3DFMT_R8G8_B8G8            = MAKEFOURCC('R', 'G', 'B', 'G'),
		//D3DFMT_YUY2                 = MAKEFOURCC('Y', 'U', 'Y', '2'),
		//D3DFMT_G8R8_G8B8            = MAKEFOURCC('G', 'R', 'G', 'B'),
		//D3DFMT_DXT1                 = MAKEFOURCC('D', 'X', 'T', '1'),
		//D3DFMT_DXT2                 = MAKEFOURCC('D', 'X', 'T', '2'),
		//D3DFMT_DXT3                 = MAKEFOURCC('D', 'X', 'T', '3'),
		//D3DFMT_DXT4                 = MAKEFOURCC('D', 'X', 'T', '4'),
		//D3DFMT_DXT5                 = MAKEFOURCC('D', 'X', 'T', '5'),
		D3DFMT_D16_LOCKABLE = 70,
		D3DFMT_D32 = 71,
		D3DFMT_D15S1 = 73,
		D3DFMT_D24S8 = 75,
		D3DFMT_D24X8 = 77,
		D3DFMT_D24X4S4 = 79,
		D3DFMT_D16 = 80,
		D3DFMT_D32F_LOCKABLE = 82,
		D3DFMT_D24FS8 = 83,
		/* Z-Stencil formats valid for CPU access */
		D3DFMT_D32_LOCKABLE = 84,
		D3DFMT_S8_LOCKABLE = 85,
		D3DFMT_L16 = 81,
		D3DFMT_VERTEXDATA = 100,
		D3DFMT_INDEX16 = 101,
		D3DFMT_INDEX32 = 102,
		D3DFMT_Q16W16V16U16 = 110,
		D3DFMT_MULTI2_ARGB8 = 0x3154454D,
		//D3DFMT_MULTI2_ARGB8 = MAKEFOURCC('M', 'E', 'T', '1'),
		// Floating point surface formats
		// s10e5 formats (16-bits per channel)
		D3DFMT_R16F = 111,
		D3DFMT_G16R16F = 112,
		D3DFMT_A16B16G16R16F = 113,
		// IEEE s23e8 formats (32-bits per channel)
		D3DFMT_R32F = 114,
		D3DFMT_G32R32F = 115,
		D3DFMT_A32B32G32R32F = 116,
		D3DFMT_CxV8U8 = 117,
		// Monochrome 1 bit per pixel format
		D3DFMT_A1 = 118,
		// Binary format indicating that the data has no inherent type
		D3DFMT_BINARYBUFFER = 199,
		D3DFMT_FORCE_DWORD = 0x7fffffff
	} ;
	#endregion

	#region Direct3D9 Device types
	public enum D3DDEVTYPE {
		D3DDEVTYPE_HAL = 1,
		D3DDEVTYPE_REF = 2,
		D3DDEVTYPE_SW = 3,
		D3DDEVTYPE_NULLREF = 4,
		D3DDEVTYPE_FORCE_DWORD = 0x7fffffff
	} ;
	#endregion

	#region Types
	public enum D3DRESOURCETYPE {
		D3DRTYPE_SURFACE = 1,
		D3DRTYPE_VOLUME = 2,
		D3DRTYPE_TEXTURE = 3,
		D3DRTYPE_VOLUMETEXTURE = 4,
		D3DRTYPE_CUBETEXTURE = 5,
		D3DRTYPE_VERTEXBUFFER = 6,
		D3DRTYPE_INDEXBUFFER = 7,           //if this changes, change _D3DDEVINFO_RESOURCEMANAGER definition
		D3DRTYPE_FORCE_DWORD = 0x7fffffff
	} ;
	#endregion

	#region Multi-Sample buffer types
	public enum D3DMULTISAMPLE_TYPE {
		D3DMULTISAMPLE_NONE = 0,
		D3DMULTISAMPLE_NONMASKABLE = 1,
		D3DMULTISAMPLE_2_SAMPLES = 2,
		D3DMULTISAMPLE_3_SAMPLES = 3,
		D3DMULTISAMPLE_4_SAMPLES = 4,
		D3DMULTISAMPLE_5_SAMPLES = 5,
		D3DMULTISAMPLE_6_SAMPLES = 6,
		D3DMULTISAMPLE_7_SAMPLES = 7,
		D3DMULTISAMPLE_8_SAMPLES = 8,
		D3DMULTISAMPLE_9_SAMPLES = 9,
		D3DMULTISAMPLE_10_SAMPLES = 10,
		D3DMULTISAMPLE_11_SAMPLES = 11,
		D3DMULTISAMPLE_12_SAMPLES = 12,
		D3DMULTISAMPLE_13_SAMPLES = 13,
		D3DMULTISAMPLE_14_SAMPLES = 14,
		D3DMULTISAMPLE_15_SAMPLES = 15,
		D3DMULTISAMPLE_16_SAMPLES = 16,
		D3DMULTISAMPLE_FORCE_DWORD = 0x7fffffff
	} ;
	#endregion

	/* Creation Parameters */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DDEVICE_CREATION_PARAMETERS {
		public uint AdapterOrdinal;
		public D3DDEVTYPE DeviceType;
		public IntPtr hFocusWindow;
		public uint BehaviorFlags;
	} ;


	/* SwapEffects */
	public enum D3DSWAPEFFECT {
		D3DSWAPEFFECT_DISCARD = 1,
		D3DSWAPEFFECT_FLIP = 2,
		D3DSWAPEFFECT_COPY = 3,
		D3DSWAPEFFECT_FORCE_DWORD = 0x7fffffff
	} ;

	/* Pool types */
	public enum D3DPOOL {
		D3DPOOL_DEFAULT = 0,
		D3DPOOL_MANAGED = 1,
		D3DPOOL_SYSTEMMEM = 2,
		D3DPOOL_SCRATCH = 3,
		D3DPOOL_FORCE_DWORD = 0x7fffffff
	} ;

	/* Resize Optional Parameters */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DPRESENT_PARAMETERS {
		public int BackBufferWidth;
        public int BackBufferHeight;
		public D3DFORMAT BackBufferFormat;
		public uint BackBufferCount;
		public D3DMULTISAMPLE_TYPE MultiSampleType;
		public uint MultiSampleQuality;
		public D3DSWAPEFFECT SwapEffect;
		public IntPtr hDeviceWindow;
		[MarshalAs(UnmanagedType.Bool)]
		public bool Windowed;
		[MarshalAs(UnmanagedType.Bool)]
		public bool EnableAutoDepthStencil;
		public D3DFORMAT AutoDepthStencilFormat;
		public uint Flags;
		/* FullScreen_RefreshRateInHz must be zero for Windowed mode */
		public uint FullScreen_RefreshRateInHz;
		public uint PresentationInterval;
	} ;
	/* Raster Status structure returned by GetRasterStatus */
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DRASTER_STATUS {
		[MarshalAs(UnmanagedType.Bool)]
		public bool InVBlank;
		public uint ScanLine;
	} ;
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DGAMMARAMP {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] red;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] green;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] blue;
	} ;
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct D3DRECT {
		public int x1;
		public int y1;
		public int x2;
		public int y2;
	} ;
	public enum D3DTEXTUREFILTERTYPE {
		D3DTEXF_NONE = 0,    // filtering disabled (valid for mip filter only)
		D3DTEXF_POINT = 1,    // nearest
		D3DTEXF_LINEAR = 2,    // linear interpolation
		D3DTEXF_ANISOTROPIC = 3,    // anisotropic
		D3DTEXF_PYRAMIDALQUAD = 6,    // 4-sample tent
		D3DTEXF_GAUSSIANQUAD = 7,    // 4-sample gaussian
		D3DTEXF_CONVOLUTIONMONO = 8,    // Convolution filter for monochrome textures
		D3DTEXF_FORCE_DWORD = 0x7fffffff,   // force 32-bit size enum
	} ;
	public enum D3DSTATEBLOCKTYPE {
		D3DSBT_ALL = 1, // capture all state
		D3DSBT_PIXELSTATE = 2, // capture pixel state
		D3DSBT_VERTEXSTATE = 3, // capture vertex state
		D3DSBT_FORCE_DWORD = 0x7fffffff,
	} ;
	public enum D3DRENDERSTATETYPE {
		D3DRS_ZENABLE = 7,    /* D3DZBUFFERTYPE (or TRUE/FALSE for legacy) */
		D3DRS_FILLMODE = 8,    /* D3DFILLMODE */
		D3DRS_SHADEMODE = 9,    /* D3DSHADEMODE */
		D3DRS_ZWRITEENABLE = 14,   /* TRUE to enable z writes */
		D3DRS_ALPHATESTENABLE = 15,   /* TRUE to enable alpha tests */
		D3DRS_LASTPIXEL = 16,   /* TRUE for last-pixel on lines */
		D3DRS_SRCBLEND = 19,   /* D3DBLEND */
		D3DRS_DESTBLEND = 20,   /* D3DBLEND */
		D3DRS_CULLMODE = 22,   /* D3DCULL */
		D3DRS_ZFUNC = 23,   /* D3DCMPFUNC */
		D3DRS_ALPHAREF = 24,   /* D3DFIXED */
		D3DRS_ALPHAFUNC = 25,   /* D3DCMPFUNC */
		D3DRS_DITHERENABLE = 26,   /* TRUE to enable dithering */
		D3DRS_ALPHABLENDENABLE = 27,   /* TRUE to enable alpha blending */
		D3DRS_FOGENABLE = 28,   /* TRUE to enable fog blending */
		D3DRS_SPECULARENABLE = 29,   /* TRUE to enable specular */
		D3DRS_FOGCOLOR = 34,   /* D3DCOLOR */
		D3DRS_FOGTABLEMODE = 35,   /* D3DFOGMODE */
		D3DRS_FOGSTART = 36,   /* Fog start (for both vertex and pixel fog) */
		D3DRS_FOGEND = 37,   /* Fog end      */
		D3DRS_FOGDENSITY = 38,   /* Fog density  */
		D3DRS_RANGEFOGENABLE = 48,   /* Enables range-based fog */
		D3DRS_STENCILENABLE = 52,   /* BOOL enable/disable stenciling */
		D3DRS_STENCILFAIL = 53,   /* D3DSTENCILOP to do if stencil test fails */
		D3DRS_STENCILZFAIL = 54,   /* D3DSTENCILOP to do if stencil test passes and Z test fails */
		D3DRS_STENCILPASS = 55,   /* D3DSTENCILOP to do if both stencil and Z tests pass */
		D3DRS_STENCILFUNC = 56,   /* D3DCMPFUNC fn.  Stencil Test passes if ((ref & mask) stencilfn (stencil & mask)) is true */
		D3DRS_STENCILREF = 57,   /* Reference value used in stencil test */
		D3DRS_STENCILMASK = 58,   /* Mask value used in stencil test */
		D3DRS_STENCILWRITEMASK = 59,   /* Write mask applied to values written to stencil buffer */
		D3DRS_TEXTUREFACTOR = 60,   /* D3DCOLOR used for multi-texture blend */
		D3DRS_WRAP0 = 128,  /* wrap for 1st texture coord. set */
		D3DRS_WRAP1 = 129,  /* wrap for 2nd texture coord. set */
		D3DRS_WRAP2 = 130,  /* wrap for 3rd texture coord. set */
		D3DRS_WRAP3 = 131,  /* wrap for 4th texture coord. set */
		D3DRS_WRAP4 = 132,  /* wrap for 5th texture coord. set */
		D3DRS_WRAP5 = 133,  /* wrap for 6th texture coord. set */
		D3DRS_WRAP6 = 134,  /* wrap for 7th texture coord. set */
		D3DRS_WRAP7 = 135,  /* wrap for 8th texture coord. set */
		D3DRS_CLIPPING = 136,
		D3DRS_LIGHTING = 137,
		D3DRS_AMBIENT = 139,
		D3DRS_FOGVERTEXMODE = 140,
		D3DRS_COLORVERTEX = 141,
		D3DRS_LOCALVIEWER = 142,
		D3DRS_NORMALIZENORMALS = 143,
		D3DRS_DIFFUSEMATERIALSOURCE = 145,
		D3DRS_SPECULARMATERIALSOURCE = 146,
		D3DRS_AMBIENTMATERIALSOURCE = 147,
		D3DRS_EMISSIVEMATERIALSOURCE = 148,
		D3DRS_VERTEXBLEND = 151,
		D3DRS_CLIPPLANEENABLE = 152,
		D3DRS_POINTSIZE = 154,   /* float point size */
		D3DRS_POINTSIZE_MIN = 155,   /* float point size min threshold */
		D3DRS_POINTSPRITEENABLE = 156,   /* BOOL point texture coord control */
		D3DRS_POINTSCALEENABLE = 157,   /* BOOL point size scale enable */
		D3DRS_POINTSCALE_A = 158,   /* float point attenuation A value */
		D3DRS_POINTSCALE_B = 159,   /* float point attenuation B value */
		D3DRS_POINTSCALE_C = 160,   /* float point attenuation C value */
		D3DRS_MULTISAMPLEANTIALIAS = 161,  // BOOL - set to do FSAA with multisample buffer
		D3DRS_MULTISAMPLEMASK = 162,  // uint  - per-sample enable/disable
		D3DRS_PATCHEDGESTYLE = 163,  // Sets whether patch edges will use float style tessellation
		D3DRS_DEBUGMONITORTOKEN = 165,  // DEBUG ONLY - token to debug monitor
		D3DRS_POINTSIZE_MAX = 166,   /* float point size max threshold */
		D3DRS_INDEXEDVERTEXBLENDENABLE = 167,
		D3DRS_COLORWRITEENABLE = 168,  // per-channel write enable
		D3DRS_TWEENFACTOR = 170,   // float tween factor
		D3DRS_BLENDOP = 171,   // D3DBLENDOP setting
		D3DRS_POSITIONDEGREE = 172,   // NPatch position interpolation degree. D3DDEGREE_LINEAR or D3DDEGREE_CUBIC (default)
		D3DRS_NORMALDEGREE = 173,   // NPatch normal interpolation degree. D3DDEGREE_LINEAR (default) or D3DDEGREE_QUADRATIC
		D3DRS_SCISSORTESTENABLE = 174,
		D3DRS_SLOPESCALEDEPTHBIAS = 175,
		D3DRS_ANTIALIASEDLINEENABLE = 176,
		D3DRS_MINTESSELLATIONLEVEL = 178,
		D3DRS_MAXTESSELLATIONLEVEL = 179,
		D3DRS_ADAPTIVETESS_X = 180,
		D3DRS_ADAPTIVETESS_Y = 181,
		D3DRS_ADAPTIVETESS_Z = 182,
		D3DRS_ADAPTIVETESS_W = 183,
		D3DRS_ENABLEADAPTIVETESSELLATION = 184,
		D3DRS_TWOSIDEDSTENCILMODE = 185,   /* BOOL enable/disable 2 sided stenciling */
		D3DRS_CCW_STENCILFAIL = 186,   /* D3DSTENCILOP to do if ccw stencil test fails */
		D3DRS_CCW_STENCILZFAIL = 187,   /* D3DSTENCILOP to do if ccw stencil test passes and Z test fails */
		D3DRS_CCW_STENCILPASS = 188,   /* D3DSTENCILOP to do if both ccw stencil and Z tests pass */
		D3DRS_CCW_STENCILFUNC = 189,   /* D3DCMPFUNC fn.  ccw Stencil Test passes if ((ref & mask) stencilfn (stencil & mask)) is true */
		D3DRS_COLORWRITEENABLE1 = 190,   /* Additional ColorWriteEnables for the devices that support D3DPMISCCAPS_INDEPENDENTWRITEMASKS */
		D3DRS_COLORWRITEENABLE2 = 191,   /* Additional ColorWriteEnables for the devices that support D3DPMISCCAPS_INDEPENDENTWRITEMASKS */
		D3DRS_COLORWRITEENABLE3 = 192,   /* Additional ColorWriteEnables for the devices that support D3DPMISCCAPS_INDEPENDENTWRITEMASKS */
		D3DRS_BLENDFACTOR = 193,   /* D3DCOLOR used for a constant blend factor during alpha blending for devices that support D3DPBLENDCAPS_BLENDFACTOR */
		D3DRS_SRGBWRITEENABLE = 194,   /* Enable rendertarget writes to be DE-linearized to SRGB (for formats that expose D3DUSAGE_QUERY_SRGBWRITE) */
		D3DRS_DEPTHBIAS = 195,
		D3DRS_WRAP8 = 198,   /* Additional wrap states for vs_3_0+ attributes with D3DDECLUSAGE_TEXCOORD */
		D3DRS_WRAP9 = 199,
		D3DRS_WRAP10 = 200,
		D3DRS_WRAP11 = 201,
		D3DRS_WRAP12 = 202,
		D3DRS_WRAP13 = 203,
		D3DRS_WRAP14 = 204,
		D3DRS_WRAP15 = 205,
		D3DRS_SEPARATEALPHABLENDENABLE = 206,  /* TRUE to enable a separate blending function for the alpha channel */
		D3DRS_SRCBLENDALPHA = 207,  /* SRC blend factor for the alpha channel when D3DRS_SEPARATEDESTALPHAENABLE is TRUE */
		D3DRS_DESTBLENDALPHA = 208,  /* DST blend factor for the alpha channel when D3DRS_SEPARATEDESTALPHAENABLE is TRUE */
		D3DRS_BLENDOPALPHA = 209,  /* Blending operation for the alpha channel when D3DRS_SEPARATEDESTALPHAENABLE is TRUE */


		D3DRS_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	/* Back buffer types */
	public enum D3DBACKBUFFER_TYPE {
		D3DBACKBUFFER_TYPE_MONO = 0,
		D3DBACKBUFFER_TYPE_LEFT = 1,
		D3DBACKBUFFER_TYPE_RIGHT = 2,
		D3DBACKBUFFER_TYPE_FORCE_DWORD = 0x7fffffff
	} ;
	// High order surfaces
	//
	public enum D3DBASISTYPE {
		D3DBASIS_BEZIER = 0,
		D3DBASIS_BSPLINE = 1,
		D3DBASIS_CATMULL_ROM = 2, /* In D3D8 this used to be D3DBASIS_INTERPOLATE */
		D3DBASIS_FORCE_DWORD = 0x7fffffff,
	} ;

	public enum D3DDEGREETYPE {
		D3DDEGREE_LINEAR = 1,
		D3DDEGREE_QUADRATIC = 2,
		D3DDEGREE_CUBIC = 3,
		D3DDEGREE_QUINTIC = 5,
		D3DDEGREE_FORCE_DWORD = 0x7fffffff,
	} ;

	public enum D3DPATCHEDGESTYLE {
		D3DPATCHEDGE_DISCRETE = 0,
		D3DPATCHEDGE_CONTINUOUS = 1,
		D3DPATCHEDGE_FORCE_DWORD = 0x7fffffff,
	} ;

	// The D3DVERTEXBLENDFLAGS type is used with D3DRS_VERTEXBLEND state.
	//
	public enum D3DVERTEXBLENDFLAGS {
		D3DVBF_DISABLE = 0,     // Disable vertex blending
		D3DVBF_1WEIGHTS = 1,     // 2 matrix blending
		D3DVBF_2WEIGHTS = 2,     // 3 matrix blending
		D3DVBF_3WEIGHTS = 3,     // 4 matrix blending
		D3DVBF_TWEENING = 255,   // blending using D3DRS_TWEENFACTOR
		D3DVBF_0WEIGHTS = 256,   // one matrix is used with weight 1.0
		D3DVBF_FORCE_DWORD = 0x7fffffff, // force 32-bit size enum
	} ;

	public enum D3DTEXTURETRANSFORMFLAGS {
		D3DTTFF_DISABLE = 0,    // texture coordinates are passed directly
		D3DTTFF_COUNT1 = 1,    // rasterizer should expect 1-D texture coords
		D3DTTFF_COUNT2 = 2,    // rasterizer should expect 2-D texture coords
		D3DTTFF_COUNT3 = 3,    // rasterizer should expect 3-D texture coords
		D3DTTFF_COUNT4 = 4,    // rasterizer should expect 4-D texture coords
		D3DTTFF_PROJECTED = 256,  // texcoords to be divided by COUNTth element
		D3DTTFF_FORCE_DWORD = 0x7fffffff,
	} ;

	/* CubeMap Face identifiers */
	public enum D3DCUBEMAP_FACES {
		D3DCUBEMAP_FACE_POSITIVE_X = 0,
		D3DCUBEMAP_FACE_NEGATIVE_X = 1,
		D3DCUBEMAP_FACE_POSITIVE_Y = 2,
		D3DCUBEMAP_FACE_NEGATIVE_Y = 3,
		D3DCUBEMAP_FACE_POSITIVE_Z = 4,
		D3DCUBEMAP_FACE_NEGATIVE_Z = 5,
		D3DCUBEMAP_FACE_FORCE_DWORD = 0x7fffffff
	} ;
	/*
 * State enumerants for per-stage processing of fixed function pixel processing
 * Two of these affect fixed function vertex processing as well: TEXTURETRANSFORMFLAGS and TEXCOORDINDEX.
 */
	public enum D3DTEXTURESTAGESTATETYPE {
		D3DTSS_COLOROP = 1, /* D3DTEXTUREOP - per-stage blending controls for color channels */
		D3DTSS_COLORARG1 = 2, /* D3DTA_* (texture arg) */
		D3DTSS_COLORARG2 = 3, /* D3DTA_* (texture arg) */
		D3DTSS_ALPHAOP = 4, /* D3DTEXTUREOP - per-stage blending controls for alpha channel */
		D3DTSS_ALPHAARG1 = 5, /* D3DTA_* (texture arg) */
		D3DTSS_ALPHAARG2 = 6, /* D3DTA_* (texture arg) */
		D3DTSS_BUMPENVMAT00 = 7, /* float (bump mapping matrix) */
		D3DTSS_BUMPENVMAT01 = 8, /* float (bump mapping matrix) */
		D3DTSS_BUMPENVMAT10 = 9, /* float (bump mapping matrix) */
		D3DTSS_BUMPENVMAT11 = 10, /* float (bump mapping matrix) */
		D3DTSS_TEXCOORDINDEX = 11, /* identifies which set of texture coordinates index this texture */
		D3DTSS_BUMPENVLSCALE = 22, /* float scale for bump map luminance */
		D3DTSS_BUMPENVLOFFSET = 23, /* float offset for bump map luminance */
		D3DTSS_TEXTURETRANSFORMFLAGS = 24, /* D3DTEXTURETRANSFORMFLAGS controls texture transform */
		D3DTSS_COLORARG0 = 26, /* D3DTA_* third arg for triadic ops */
		D3DTSS_ALPHAARG0 = 27, /* D3DTA_* third arg for triadic ops */
		D3DTSS_RESULTARG = 28, /* D3DTA_* arg for result (CURRENT or TEMP) */
		D3DTSS_CONSTANT = 32, /* Per-stage constant D3DTA_CONSTANT */
		D3DTSS_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	// Values for material source
	public enum D3DMATERIALCOLORSOURCE {
		D3DMCS_MATERIAL = 0,            // Color from material is used
		D3DMCS_COLOR1 = 1,            // Diffuse vertex color is used
		D3DMCS_COLOR2 = 2,            // Specular vertex color is used
		D3DMCS_FORCE_DWORD = 0x7fffffff,   // force 32-bit size enum
	} ;

	/*
 * State enumerants for per-sampler texture processing.
 */
	public enum D3DSAMPLERSTATETYPE {
		D3DSAMP_ADDRESSU = 1,  /* D3DTEXTUREADDRESS for U coordinate */
		D3DSAMP_ADDRESSV = 2,  /* D3DTEXTUREADDRESS for V coordinate */
		D3DSAMP_ADDRESSW = 3,  /* D3DTEXTUREADDRESS for W coordinate */
		D3DSAMP_BORDERCOLOR = 4,  /* D3DCOLOR */
		D3DSAMP_MAGFILTER = 5,  /* D3DTEXTUREFILTER filter to use for magnification */
		D3DSAMP_MINFILTER = 6,  /* D3DTEXTUREFILTER filter to use for minification */
		D3DSAMP_MIPFILTER = 7,  /* D3DTEXTUREFILTER filter to use between mipmaps during minification */
		D3DSAMP_MIPMAPLODBIAS = 8,  /* float Mipmap LOD bias */
		D3DSAMP_MAXMIPLEVEL = 9,  /* DWORD 0..(n-1) LOD index of largest map to use (0 == largest) */
		D3DSAMP_MAXANISOTROPY = 10, /* DWORD maximum anisotropy */
		D3DSAMP_SRGBTEXTURE = 11, /* Default = 0 (which means Gamma 1.0,
                                   no correction required.) else correct for
                                   Gamma = 2.2 */
		D3DSAMP_ELEMENTINDEX = 12, /* When multi-element texture is assigned to sampler, this
                                    indicates which element index to use.  Default = 0.  */
		D3DSAMP_DMAPOFFSET = 13, /* Offset in vertices in the pre-sampled displacement map.
                                    Only valid for D3DDMAPSAMPLER sampler  */
		D3DSAMP_FORCE_DWORD = 0x7fffffff, /* force 32-bit size enum */
	} ;

	// Vertex Shaders
	//

	// Vertex shader declaration

	// Vertex element semantics
	//
	public enum D3DDECLUSAGE {
		D3DDECLUSAGE_POSITION = 0,
		D3DDECLUSAGE_BLENDWEIGHT,   // 1
		D3DDECLUSAGE_BLENDINDICES,  // 2
		D3DDECLUSAGE_NORMAL,        // 3
		D3DDECLUSAGE_PSIZE,         // 4
		D3DDECLUSAGE_TEXCOORD,      // 5
		D3DDECLUSAGE_TANGENT,       // 6
		D3DDECLUSAGE_BINORMAL,      // 7
		D3DDECLUSAGE_TESSFACTOR,    // 8
		D3DDECLUSAGE_POSITIONT,     // 9
		D3DDECLUSAGE_COLOR,         // 10
		D3DDECLUSAGE_FOG,           // 11
		D3DDECLUSAGE_DEPTH,         // 12
		D3DDECLUSAGE_SAMPLE,        // 13
	} ;

	public enum D3DDECLMETHOD {
		D3DDECLMETHOD_DEFAULT = 0,
		D3DDECLMETHOD_PARTIALU,
		D3DDECLMETHOD_PARTIALV,
		D3DDECLMETHOD_CROSSUV,    // Normal
		D3DDECLMETHOD_UV,
		D3DDECLMETHOD_LOOKUP,               // Lookup a displacement map
		D3DDECLMETHOD_LOOKUPPRESAMPLED,     // Lookup a pre-sampled displacement map
	} ;

	// Declarations for _Type fields
	//
	public enum D3DDECLTYPE {
		D3DDECLTYPE_FLOAT1 = 0,  // 1D float expanded to (value, 0., 0., 1.)
		D3DDECLTYPE_FLOAT2 = 1,  // 2D float expanded to (value, value, 0., 1.)
		D3DDECLTYPE_FLOAT3 = 2,  // 3D float expanded to (value, value, value, 1.)
		D3DDECLTYPE_FLOAT4 = 3,  // 4D float
		D3DDECLTYPE_D3DCOLOR = 4,  // 4D packed unsigned bytes mapped to 0. to 1. range
		// Input is in D3DCOLOR format (ARGB) expanded to (R, G, B, A)
		D3DDECLTYPE_UBYTE4 = 5,  // 4D unsigned byte
		D3DDECLTYPE_SHORT2 = 6,  // 2D signed short expanded to (value, value, 0., 1.)
		D3DDECLTYPE_SHORT4 = 7,  // 4D signed short

		// The following types are valid only with vertex shaders >= 2.0


		D3DDECLTYPE_UBYTE4N = 8,  // Each of 4 bytes is normalized by dividing to 255.0
		D3DDECLTYPE_SHORT2N = 9,  // 2D signed short normalized (v[0]/32767.0,v[1]/32767.0,0,1)
		D3DDECLTYPE_SHORT4N = 10,  // 4D signed short normalized (v[0]/32767.0,v[1]/32767.0,v[2]/32767.0,v[3]/32767.0)
		D3DDECLTYPE_USHORT2N = 11,  // 2D unsigned short normalized (v[0]/65535.0,v[1]/65535.0,0,1)
		D3DDECLTYPE_USHORT4N = 12,  // 4D unsigned short normalized (v[0]/65535.0,v[1]/65535.0,v[2]/65535.0,v[3]/65535.0)
		D3DDECLTYPE_UDEC3 = 13,  // 3D unsigned 10 10 10 format expanded to (value, value, value, 1)
		D3DDECLTYPE_DEC3N = 14,  // 3D signed 10 10 10 format normalized and expanded to (v[0]/511.0, v[1]/511.0, v[2]/511.0, 1)
		D3DDECLTYPE_FLOAT16_2 = 15,  // Two 16-bit floating point values, expanded to (value, value, 0, 1)
		D3DDECLTYPE_FLOAT16_4 = 16,  // Four 16-bit floating point values
		D3DDECLTYPE_UNUSED = 17,  // When the type field in a decl is unused.
	} ;

	public enum D3DSHADER_INSTRUCTION_OPCODE_TYPE {
		D3DSIO_NOP = 0,
		D3DSIO_MOV,
		D3DSIO_ADD,
		D3DSIO_SUB,
		D3DSIO_MAD,
		D3DSIO_MUL,
		D3DSIO_RCP,
		D3DSIO_RSQ,
		D3DSIO_DP3,
		D3DSIO_DP4,
		D3DSIO_MIN,
		D3DSIO_MAX,
		D3DSIO_SLT,
		D3DSIO_SGE,
		D3DSIO_EXP,
		D3DSIO_LOG,
		D3DSIO_LIT,
		D3DSIO_DST,
		D3DSIO_LRP,
		D3DSIO_FRC,
		D3DSIO_M4x4,
		D3DSIO_M4x3,
		D3DSIO_M3x4,
		D3DSIO_M3x3,
		D3DSIO_M3x2,
		D3DSIO_CALL,
		D3DSIO_CALLNZ,
		D3DSIO_LOOP,
		D3DSIO_RET,
		D3DSIO_ENDLOOP,
		D3DSIO_LABEL,
		D3DSIO_DCL,
		D3DSIO_POW,
		D3DSIO_CRS,
		D3DSIO_SGN,
		D3DSIO_ABS,
		D3DSIO_NRM,
		D3DSIO_SINCOS,
		D3DSIO_REP,
		D3DSIO_ENDREP,
		D3DSIO_IF,
		D3DSIO_IFC,
		D3DSIO_ELSE,
		D3DSIO_ENDIF,
		D3DSIO_BREAK,
		D3DSIO_BREAKC,
		D3DSIO_MOVA,
		D3DSIO_DEFB,
		D3DSIO_DEFI,

		D3DSIO_TEXCOORD = 64,
		D3DSIO_TEXKILL,
		D3DSIO_TEX,
		D3DSIO_TEXBEM,
		D3DSIO_TEXBEML,
		D3DSIO_TEXREG2AR,
		D3DSIO_TEXREG2GB,
		D3DSIO_TEXM3x2PAD,
		D3DSIO_TEXM3x2TEX,
		D3DSIO_TEXM3x3PAD,
		D3DSIO_TEXM3x3TEX,
		D3DSIO_RESERVED0,
		D3DSIO_TEXM3x3SPEC,
		D3DSIO_TEXM3x3VSPEC,
		D3DSIO_EXPP,
		D3DSIO_LOGP,
		D3DSIO_CND,
		D3DSIO_DEF,
		D3DSIO_TEXREG2RGB,
		D3DSIO_TEXDP3TEX,
		D3DSIO_TEXM3x2DEPTH,
		D3DSIO_TEXDP3,
		D3DSIO_TEXM3x3,
		D3DSIO_TEXDEPTH,
		D3DSIO_CMP,
		D3DSIO_BEM,
		D3DSIO_DP2ADD,
		D3DSIO_DSX,
		D3DSIO_DSY,
		D3DSIO_TEXLDD,
		D3DSIO_SETP,
		D3DSIO_TEXLDL,
		D3DSIO_BREAKP,

		D3DSIO_PHASE = 0xFFFD,
		D3DSIO_COMMENT = 0xFFFE,
		D3DSIO_END = 0xFFFF,

		D3DSIO_FORCE_DWORD = 0x7fffffff,   // force 32-bit size enum
	} ;

	// Comparison for dynamic conditional instruction opcodes (i.e. if, breakc)
	public enum D3DSHADER_COMPARISON {
		// < = >
		D3DSPC_RESERVED0 = 0, // 0 0 0
		D3DSPC_GT = 1, // 0 0 1
		D3DSPC_EQ = 2, // 0 1 0
		D3DSPC_GE = 3, // 0 1 1
		D3DSPC_LT = 4, // 1 0 0
		D3DSPC_NE = 5, // 1 0 1
		D3DSPC_LE = 6, // 1 1 0
		D3DSPC_RESERVED1 = 7  // 1 1 1
	} ;

	//TODO: FIX THIS
	/*public enum D3DSAMPLER_TEXTURE_TYPE {
		D3DSTT_UNKNOWN = 0 << D3DSP_TEXTURETYPE_SHIFT, // uninitialized value
		D3DSTT_2D = 2 << D3DSP_TEXTURETYPE_SHIFT, // dcl_2d s# (for declaring a 2-D texture)
		D3DSTT_CUBE = 3 << D3DSP_TEXTURETYPE_SHIFT, // dcl_cube s# (for declaring a cube texture)
		D3DSTT_VOLUME = 4 << D3DSP_TEXTURETYPE_SHIFT, // dcl_volume s# (for declaring a volume texture)
		D3DSTT_FORCE_DWORD = 0x7fffffff,      // force 32-bit size enum
	} ;*/

	public enum D3DSHADER_PARAM_REGISTER_TYPE {
		D3DSPR_TEMP = 0, // Temporary Register File
		D3DSPR_INPUT = 1, // Input Register File
		D3DSPR_CONST = 2, // Constant Register File
		D3DSPR_ADDR = 3, // Address Register (VS)
		D3DSPR_TEXTURE = 3, // Texture Register File (PS)
		D3DSPR_RASTOUT = 4, // Rasterizer Register File
		D3DSPR_ATTROUT = 5, // Attribute Output Register File
		D3DSPR_TEXCRDOUT = 6, // Texture Coordinate Output Register File
		D3DSPR_OUTPUT = 6, // Output register file for VS3.0+
		D3DSPR_CONSTINT = 7, // Constant Integer Vector Register File
		D3DSPR_COLOROUT = 8, // Color Output Register File
		D3DSPR_DEPTHOUT = 9, // Depth Output Register File
		D3DSPR_SAMPLER = 10, // Sampler State Register File
		D3DSPR_CONST2 = 11, // Constant Register File  2048 - 4095
		D3DSPR_CONST3 = 12, // Constant Register File  4096 - 6143
		D3DSPR_CONST4 = 13, // Constant Register File  6144 - 8191
		D3DSPR_CONSTBOOL = 14, // Constant Boolean register file
		D3DSPR_LOOP = 15, // Loop counter register file
		D3DSPR_TEMPFLOAT16 = 16, // 16-bit float temp register file
		D3DSPR_MISCTYPE = 17, // Miscellaneous (single) registers.
		D3DSPR_LABEL = 18, // Label
		D3DSPR_PREDICATE = 19, // Predicate register
		D3DSPR_FORCE_DWORD = 0x7fffffff,         // force 32-bit size enum
	} ;

	// The miscellaneous register file (D3DSPR_MISCTYPES)
	// contains register types for which there is only ever one
	// register (i.e. the register # is not needed).
	// Rather than use up additional register types for such
	// registers, they are defined
	// as particular offsets into the misc. register file:
	public enum D3DSHADER_MISCTYPE_OFFSETS {
		D3DSMO_POSITION = 0, // Input position x,y,z,rhw (PS)
		D3DSMO_FACE = 1, // Floating point primitive area (PS)
	} ;

	// Register offsets in the Rasterizer Register File
	//
	public enum D3DVS_RASTOUT_OFFSETS {
		D3DSRO_POSITION = 0,
		D3DSRO_FOG,
		D3DSRO_POINT_SIZE,
		D3DSRO_FORCE_DWORD = 0x7fffffff,         // force 32-bit size enum
	} ;

	//TODO: FIX THIS
	/*
	public enum D3DVS_ADDRESSMODE_TYPE {
		D3DVS_ADDRMODE_ABSOLUTE = (0 << D3DVS_ADDRESSMODE_SHIFT),
		D3DVS_ADDRMODE_RELATIVE = (1 << D3DVS_ADDRESSMODE_SHIFT),
		D3DVS_ADDRMODE_FORCE_DWORD = 0x7fffffff, // force 32-bit size enum
	} ;

	public enum D3DSHADER_ADDRESSMODE_TYPE {
		D3DSHADER_ADDRMODE_ABSOLUTE = (0 << D3DSHADER_ADDRESSMODE_SHIFT),
		D3DSHADER_ADDRMODE_RELATIVE = (1 << D3DSHADER_ADDRESSMODE_SHIFT),
		D3DSHADER_ADDRMODE_FORCE_DWORD = 0x7fffffff, // force 32-bit size enum
	} ;

	public enum D3DSHADER_PARAM_SRCMOD_TYPE {
		D3DSPSM_NONE = 0 << D3DSP_SRCMOD_SHIFT, // nop
		D3DSPSM_NEG = 1 << D3DSP_SRCMOD_SHIFT, // negate
		D3DSPSM_BIAS = 2 << D3DSP_SRCMOD_SHIFT, // bias
		D3DSPSM_BIASNEG = 3 << D3DSP_SRCMOD_SHIFT, // bias and negate
		D3DSPSM_SIGN = 4 << D3DSP_SRCMOD_SHIFT, // sign
		D3DSPSM_SIGNNEG = 5 << D3DSP_SRCMOD_SHIFT, // sign and negate
		D3DSPSM_COMP = 6 << D3DSP_SRCMOD_SHIFT, // complement
		D3DSPSM_X2 = 7 << D3DSP_SRCMOD_SHIFT, // *2
		D3DSPSM_X2NEG = 8 << D3DSP_SRCMOD_SHIFT, // *2 and negate
		D3DSPSM_DZ = 9 << D3DSP_SRCMOD_SHIFT, // divide through by z component
		D3DSPSM_DW = 10 << D3DSP_SRCMOD_SHIFT, // divide through by w component
		D3DSPSM_ABS = 11 << D3DSP_SRCMOD_SHIFT, // abs()
		D3DSPSM_ABSNEG = 12 << D3DSP_SRCMOD_SHIFT, // -abs()
		D3DSPSM_NOT = 13 << D3DSP_SRCMOD_SHIFT, // for predicate register: "!p0"
		D3DSPSM_FORCE_DWORD = 0x7fffffff,        // force 32-bit size enum
	} ;*/

	/* Debug monitor tokens (DEBUG only)

	   Note that if D3DRS_DEBUGMONITORTOKEN is set, the call is treated as
	   passing a token to the debug monitor.  For example, if, after passing
	   D3DDMT_ENABLE/DISABLE to D3DRS_DEBUGMONITORTOKEN other token values
	   are passed in, the enabled/disabled state of the debug
	   monitor will still persist.

	   The debug monitor defaults to enabled.

	   Calling GetRenderState on D3DRS_DEBUGMONITORTOKEN is not of any use.
	*/
	public enum D3DDEBUGMONITORTOKENS {
		D3DDMT_ENABLE = 0,    // enable debug monitor
		D3DDMT_DISABLE = 1,    // disable debug monitor
		D3DDMT_FORCE_DWORD = 0x7fffffff,
	} ;

	// Async feedback
	public enum D3DQUERYTYPE {
		D3DQUERYTYPE_VCACHE = 4, /* D3DISSUE_END */
		D3DQUERYTYPE_RESOURCEMANAGER = 5, /* D3DISSUE_END */
		D3DQUERYTYPE_VERTEXSTATS = 6, /* D3DISSUE_END */
		D3DQUERYTYPE_EVENT = 8, /* D3DISSUE_END */
		D3DQUERYTYPE_OCCLUSION = 9, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_TIMESTAMP = 10, /* D3DISSUE_END */
		D3DQUERYTYPE_TIMESTAMPDISJOINT = 11, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_TIMESTAMPFREQ = 12, /* D3DISSUE_END */
		D3DQUERYTYPE_PIPELINETIMINGS = 13, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_INTERFACETIMINGS = 14, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_VERTEXTIMINGS = 15, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_PIXELTIMINGS = 16, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_BANDWIDTHTIMINGS = 17, /* D3DISSUE_BEGIN, D3DISSUE_END */
		D3DQUERYTYPE_CACHEUTILIZATION = 18, /* D3DISSUE_BEGIN, D3DISSUE_END */
	} ;

	public class D3D9TYPES {

		// Flags field for Issue
		public const uint D3DISSUE_END = (1 << 0); // Tells the runtime to issue the end of a query, changing it's state to "non-signaled".
		public const uint D3DISSUE_BEGIN = (1 << 1); // Tells the runtime to issue the beginng of a query.

		// Flags field for GetData
		public const uint D3DGETDATA_FLUSH = (1 << 0); // Tells the runtime to flush if the query is outstanding.

		// Source operand swizzle definitions
		//
		public const int D3DVS_SWIZZLE_SHIFT = 16;
		public const int D3DVS_SWIZZLE_MASK = 0x00FF0000;

		// The following bits define where to take component X from:

		public const uint D3DVS_X_X = (0 << D3DVS_SWIZZLE_SHIFT);
		public const uint D3DVS_X_Y = (1 << D3DVS_SWIZZLE_SHIFT);
		public const uint D3DVS_X_Z = (2 << D3DVS_SWIZZLE_SHIFT);
		public const uint D3DVS_X_W = (3 << D3DVS_SWIZZLE_SHIFT);

		// The following bits define where to take component Y from:

		public const uint D3DVS_Y_X = (0 << (D3DVS_SWIZZLE_SHIFT + 2));
		public const uint D3DVS_Y_Y = (1 << (D3DVS_SWIZZLE_SHIFT + 2));
		public const uint D3DVS_Y_Z = (2 << (D3DVS_SWIZZLE_SHIFT + 2));
		public const uint D3DVS_Y_W = (3 << (D3DVS_SWIZZLE_SHIFT + 2));

		// The following bits define where to take component Z from:

		public const uint D3DVS_Z_X = (0 << (D3DVS_SWIZZLE_SHIFT + 4));
		public const uint D3DVS_Z_Y = (1 << (D3DVS_SWIZZLE_SHIFT + 4));
		public const uint D3DVS_Z_Z = (2 << (D3DVS_SWIZZLE_SHIFT + 4));
		public const uint D3DVS_Z_W = (3 << (D3DVS_SWIZZLE_SHIFT + 4));

		// The following bits define where to take component W from:

		public const uint D3DVS_W_X = (0 << (D3DVS_SWIZZLE_SHIFT + 6));
		public const uint D3DVS_W_Y = (1 << (D3DVS_SWIZZLE_SHIFT + 6));
		public const uint D3DVS_W_Z = (2 << (D3DVS_SWIZZLE_SHIFT + 6));
		public const uint D3DVS_W_W = (3 << (D3DVS_SWIZZLE_SHIFT + 6));

		// Value when there is no swizzle (X is taken from X, Y is taken from Y,
		// Z is taken from Z, W is taken from W
		//
		public const uint D3DVS_NOSWIZZLE = (D3DVS_X_X | D3DVS_Y_Y | D3DVS_Z_Z | D3DVS_W_W);

		// source parameter swizzle
		public const int D3DSP_SWIZZLE_SHIFT = 16;
		public const int D3DSP_SWIZZLE_MASK = 0x00FF0000;

		public const uint D3DSP_NOSWIZZLE = ((0 << (D3DSP_SWIZZLE_SHIFT + 0)) | (1 << (D3DSP_SWIZZLE_SHIFT + 2)) | (2 << (D3DSP_SWIZZLE_SHIFT + 4)) | (3 << (D3DSP_SWIZZLE_SHIFT + 6)));

		// pixel-shader swizzle ops
		public const uint D3DSP_REPLICATERED = ((0 << (D3DSP_SWIZZLE_SHIFT + 0)) | (0 << (D3DSP_SWIZZLE_SHIFT + 2)) | (0 << (D3DSP_SWIZZLE_SHIFT + 4)) | (0 << (D3DSP_SWIZZLE_SHIFT + 6)));
		public const uint D3DSP_REPLICATEGREEN = ((1 << (D3DSP_SWIZZLE_SHIFT + 0)) | (1 << (D3DSP_SWIZZLE_SHIFT + 2)) | (1 << (D3DSP_SWIZZLE_SHIFT + 4)) | (1 << (D3DSP_SWIZZLE_SHIFT + 6)));
		public const uint D3DSP_REPLICATEBLUE = ((2 << (D3DSP_SWIZZLE_SHIFT + 0)) | (2 << (D3DSP_SWIZZLE_SHIFT + 2)) | (2 << (D3DSP_SWIZZLE_SHIFT + 4)) | (2 << (D3DSP_SWIZZLE_SHIFT + 6)));
		public const uint D3DSP_REPLICATEALPHA = ((3 << (D3DSP_SWIZZLE_SHIFT + 0)) | (3 << (D3DSP_SWIZZLE_SHIFT + 2)) | (3 << (D3DSP_SWIZZLE_SHIFT + 4)) | (3 << (D3DSP_SWIZZLE_SHIFT + 6)));

		// source parameter modifiers
		public const int D3DSP_SRCMOD_SHIFT = 24;
		public const int D3DSP_SRCMOD_MASK = 0x0F000000;

		public const int D3DSHADER_ADDRESSMODE_SHIFT = 13;
		public const uint D3DSHADER_ADDRESSMODE_MASK = (1 << D3DSHADER_ADDRESSMODE_SHIFT);

		// Source operand addressing modes
		public const int D3DVS_ADDRESSMODE_SHIFT = 13;
		public const uint D3DVS_ADDRESSMODE_MASK = (1 << D3DVS_ADDRESSMODE_SHIFT);

		//---------------------------------------------------------------------
		// Parameter Token Bit Definitions
		//
		public const uint D3DSP_REGNUM_MASK = 0x000007FF;

		// destination parameter write mask
		public const uint D3DSP_WRITEMASK_0 = 0x00010000;  // Component 0 (X;Red)
		public const uint D3DSP_WRITEMASK_1 = 0x00020000;  // Component 1 (Y;Green)
		public const uint D3DSP_WRITEMASK_2 = 0x00040000;  // Component 2 (Z;Blue)
		public const uint D3DSP_WRITEMASK_3 = 0x00080000;  // Component 3 (W;Alpha)
		public const uint D3DSP_WRITEMASK_ALL = 0x000F0000;  // All Components

		// destination parameter modifiers
		public const int D3DSP_DSTMOD_SHIFT = 20;
		public const int D3DSP_DSTMOD_MASK = 0x00F00000;

		// Bit masks for destination parameter modifiers
		public const uint D3DSPDM_NONE = (0 << D3DSP_DSTMOD_SHIFT); // nop
		public const uint D3DSPDM_SATURATE = (1 << D3DSP_DSTMOD_SHIFT); // clamp to 0. to 1. range
		public const uint D3DSPDM_PARTIALPRECISION = (2 << D3DSP_DSTMOD_SHIFT); // Partial precision hint
		public const uint D3DSPDM_MSAMPCENTROID = (4 << D3DSP_DSTMOD_SHIFT); // Relevant to multisampling only:
		//      When the pixel center is not covered, sample
		//      attribute or compute gradients/LOD
		//      using multisample "centroid" location.
		//      "Centroid" is some location within the covered
		//      region of the pixel.

		// destination parameter 
		public const int D3DSP_DSTSHIFT_SHIFT = 24;
		public const int D3DSP_DSTSHIFT_MASK = 0x0F000000;

		// destination/source parameter register type
		public const uint D3DSP_REGTYPE_SHIFT = 28;
		public const uint D3DSP_REGTYPE_SHIFT2 = 8;
		public const uint D3DSP_REGTYPE_MASK = 0x70000000;
		public const uint D3DSP_REGTYPE_MASK2 = 0x00001800;

		// Comparison is part of instruction opcode token:
		public const int D3DSHADER_COMPARISON_SHIFT = D3DSP_OPCODESPECIFICCONTROL_SHIFT;
		public const uint D3DSHADER_COMPARISON_MASK = (0x7 << D3DSHADER_COMPARISON_SHIFT);

		//---------------------------------------------------------------------
		// Predication flags on instruction token
		public const uint D3DSHADER_INSTRUCTION_PREDICATED = (0x1 << 28);

		//---------------------------------------------------------------------
		// DCL Info Token Controls

		// For dcl info tokens requiring a semantic (usage + index)
		public const uint D3DSP_DCL_USAGE_SHIFT = 0;
		public const uint D3DSP_DCL_USAGE_MASK = 0x0000000f;

		public const uint D3DSP_DCL_USAGEINDEX_SHIFT = 16;
		public const uint D3DSP_DCL_USAGEINDEX_MASK = 0x000f0000;

		// DCL pixel shader sampler info token.
		public const uint D3DSP_TEXTURETYPE_SHIFT = 27;
		public const uint D3DSP_TEXTURETYPE_MASK = 0x78000000;

		//---------------------------------------------------------------------
		// Use these constants with D3DSIO_SINCOS macro as SRC2, SRC3
		//
		// TODO: FIX THIS
		//public const uint D3DSINCOSCONST1 -1.5500992e-006f, -2.1701389e-005f,  0.0026041667f, 0.00026041668f
		//public const uint D3DSINCOSCONST2 -0.020833334f, -0.12500000f, 1.0f, 0.50000000f

		//---------------------------------------------------------------------
		// Co-Issue Instruction Modifier - if set then this instruction is to be
		// issued in parallel with the previous instruction(s) for which this bit
		// is not set.
		//
		public const uint D3DSI_COISSUE = 0x40000000;

		//---------------------------------------------------------------------
		// Opcode specific controls

		public const int D3DSP_OPCODESPECIFICCONTROL_MASK = 0x00ff0000;
		public const int D3DSP_OPCODESPECIFICCONTROL_SHIFT = 16;

		// ps_2_0 texld controls
		public const uint D3DSI_TEXLD_PROJECT = (0x01 << D3DSP_OPCODESPECIFICCONTROL_SHIFT);
		public const uint D3DSI_TEXLD_BIAS = (0x02 << D3DSP_OPCODESPECIFICCONTROL_SHIFT);


		// This is used to initialize the last vertex element in a vertex declaration
		// array
		//
		//TODO: FIX THIS
		public static D3DVERTEXELEMENT9 D3DDECL_END = new D3DVERTEXELEMENT9(0xFF, 0, (byte)D3DDECLTYPE.D3DDECLTYPE_UNUSED, 0, 0, 0);

		// Maximum supported number of texture coordinate sets
		public const uint D3DDP_MAXTEXCOORD = 8;

		//---------------------------------------------------------------------
		// Values for IDirect3DDevice9::SetStreamSourceFreq's Setting parameter
		//---------------------------------------------------------------------
		public const int D3DSTREAMSOURCE_INDEXEDDATA = (1 << 30);
		public const int D3DSTREAMSOURCE_INSTANCEDATA = (2 << 30);

		//
		// Instruction Token Bit Definitions
		//
		public const uint D3DSI_OPCODE_MASK = 0x0000FFFF;

		public const uint D3DSI_INSTLENGTH_MASK = 0x0F000000;
		public const uint D3DSI_INSTLENGTH_SHIFT = 24;


		public const uint MAXD3DDECLTYPE = (uint)D3DDECLTYPE.D3DDECLTYPE_UNUSED;

		public const uint MAXD3DDECLMETHOD = (uint)D3DDECLMETHOD.D3DDECLMETHOD_LOOKUPPRESAMPLED;

		public const uint MAXD3DDECLUSAGE = (uint)D3DDECLUSAGE.D3DDECLUSAGE_SAMPLE;
		public const uint MAXD3DDECLUSAGEINDEX = 15;
		public const uint MAXD3DDECLLENGTH = 64; // does not include "end" marker vertex element

		/* Special sampler which is used in the tesselator */
		public const uint D3DDMAPSAMPLER = 256;

		// Samplers used in vertex shaders
		public const uint D3DVERTEXTEXTURESAMPLER0 = (D3DDMAPSAMPLER + 1);
		public const uint D3DVERTEXTEXTURESAMPLER1 = (D3DDMAPSAMPLER + 2);
		public const uint D3DVERTEXTEXTURESAMPLER2 = (D3DDMAPSAMPLER + 3);
		public const uint D3DVERTEXTEXTURESAMPLER3 = (D3DDMAPSAMPLER + 4);

		// Values, used with D3DTSS_TEXCOORDINDEX, to specify that the vertex data(position
		// and normal in the camera space) should be taken as texture coordinates
		// Low 16 bits are used to specify texture coordinate index, to take the WRAP mode from
		//
		public const uint D3DTSS_TCI_PASSTHRU = 0x00000000;
		public const uint D3DTSS_TCI_CAMERASPACENORMAL = 0x00010000;
		public const uint D3DTSS_TCI_CAMERASPACEPOSITION = 0x00020000;
		public const uint D3DTSS_TCI_CAMERASPACEREFLECTIONVECTOR = 0x00030000;
		public const uint D3DTSS_TCI_SPHEREMAP = 0x00040000;

		// Maximum number of simultaneous render targets D3D supports
		public const uint D3D_MAX_SIMULTANEOUS_RENDERTARGETS = 4;


		// Bias to apply to the texture coordinate set to apply a wrap to.
		public const uint D3DRENDERSTATE_WRAPBIAS = 128;

		/* Flags to construct the WRAP render states */
		public const uint D3DWRAP_U = 0x00000001;
		public const uint D3DWRAP_V = 0x00000002;
		public const uint D3DWRAP_W = 0x00000004;

		/* Flags to construct the WRAP render states for 1D thru 4D texture coordinates */
		public const uint D3DWRAPCOORD_0 = 0x00000001;    // same as D3DWRAP_U
		public const uint D3DWRAPCOORD_1 = 0x00000002;    // same as D3DWRAP_V
		public const uint D3DWRAPCOORD_2 = 0x00000004;    // same as D3DWRAP_W
		public const uint D3DWRAPCOORD_3 = 0x00000008;

		/* Flags to construct D3DRS_COLORWRITEENABLE */
		public const uint D3DCOLORWRITEENABLE_RED = (1 << 0);
		public const uint D3DCOLORWRITEENABLE_GREEN = (1 << 1);
		public const uint D3DCOLORWRITEENABLE_BLUE = (1 << 2);
		public const uint D3DCOLORWRITEENABLE_ALPHA = (1 << 3);

		/* Adapter Identifier */
		public const uint MAX_DEVICE_IDENTIFIER_STRING = 512;

		/* RefreshRate pre-defines */
		public const uint D3DPRESENT_RATE_DEFAULT = 0x00000000;

		// Values for D3DPRESENT_PARAMETERS.Flags
		public const uint D3DPRESENTFLAG_LOCKABLE_BACKBUFFER = 0x00000001;
		public const uint D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL = 0x00000002;
		public const uint D3DPRESENTFLAG_DEVICECLIP = 0x00000004;
		public const uint D3DPRESENTFLAG_VIDEO = 0x00000010;
		public const uint D3DPRESENTFLAG_NOAUTOROTATE = 0x00000020;
		public const uint D3DPRESENTFLAG_UNPRUNEDMODE = 0x00000040;

		/* Lock flags */
		public const uint D3DLOCK_READONLY = 0x00000010;
		public const uint D3DLOCK_DISCARD = 0x00002000;
		public const uint D3DLOCK_NOOVERWRITE = 0x00001000;
		public const uint D3DLOCK_NOSYSLOCK = 0x00000800;
		public const uint D3DLOCK_DONOTWAIT = 0x00004000;

		public const uint D3DLOCK_NO_DIRTY_UPDATE = 0x00008000;

		/*
 * Values for clip fields.
 */

		// Max number of user clipping planes, supported in D3D.
		public const uint D3DMAXUSERCLIPPLANES = 32;

		// These bits could be ORed together to use with D3DRS_CLIPPLANEENABLE
		//
		public const uint D3DCLIPPLANE0 = (1 << 0);
		public const uint D3DCLIPPLANE1 = (1 << 1);
		public const uint D3DCLIPPLANE2 = (1 << 2);
		public const uint D3DCLIPPLANE3 = (1 << 3);
		public const uint D3DCLIPPLANE4 = (1 << 4);
		public const uint D3DCLIPPLANE5 = (1 << 5);

		// The following bits are used in the ClipUnion and ClipIntersection
		// members of the D3DCLIPSTATUS9
		//

		public const uint D3DCS_LEFT = 0x00000001;
		public const uint D3DCS_RIGHT = 0x00000002;
		public const uint D3DCS_TOP = 0x00000004;
		public const uint D3DCS_BOTTOM = 0x00000008;
		public const uint D3DCS_FRONT = 0x00000010;
		public const uint D3DCS_BACK = 0x00000020;
		public const uint D3DCS_PLANE0 = 0x00000040;
		public const uint D3DCS_PLANE1 = 0x00000080;
		public const uint D3DCS_PLANE2 = 0x00000100;
		public const uint D3DCS_PLANE3 = 0x00000200;
		public const uint D3DCS_PLANE4 = 0x00000400;
		public const uint D3DCS_PLANE5 = 0x00000800;

		public const uint D3DCS_ALL = (D3DCS_LEFT | D3DCS_RIGHT | D3DCS_TOP | D3DCS_BOTTOM | D3DCS_FRONT | D3DCS_BACK | D3DCS_PLANE0 | D3DCS_PLANE1 | D3DCS_PLANE2 | D3DCS_PLANE3 | D3DCS_PLANE4 | D3DCS_PLANE5);

		/*
	 * Options for clearing
	 */
		public const uint D3DCLEAR_TARGET = 0x00000001;  /* Clear target surface */
		public const uint D3DCLEAR_ZBUFFER = 0x00000002;  /* Clear target z buffer */
		public const uint D3DCLEAR_STENCIL = 0x00000004; /* Clear stencil planes */

		public static uint D3DTS_WORLDMATRIX(int iIndex) {
			return (uint)(iIndex + 256);
		}
		public const uint D3DTS_WORLD = 256;
		public const uint D3DTS_WORLD1 = 257;
		public const uint D3DTS_WORLD2 = 258;
		public const uint D3DTS_WORLD3 = 259;

		// maps unsigned 8 bits/channel to D3DCOLOR
		public static uint D3DCOLOR_ARGB(uint a, uint r, uint g, uint b) {
			return ((uint)((((a) & 0xff) << 24) | (((r) & 0xff) << 16) | (((g) & 0xff) << 8) | ((b) & 0xff)));
		}
		public static uint D3DCOLOR_RGBA(uint r, uint g, uint b, uint a) {
			return D3DCOLOR_ARGB(a, r, g, b);
		}

		public static uint D3DCOLOR_XRGB(uint r, uint g, uint b) {
			return D3DCOLOR_ARGB(0xff, r, g, b);
		}

		public static uint D3DCOLOR_XYUV(uint y, uint u, uint v) {
			return D3DCOLOR_ARGB(0xff, y, u, v);
		}
		public static uint D3DCOLOR_AYUV(uint a, uint y, uint u, uint v) {
			return D3DCOLOR_ARGB(a, y, u, v);
		}

		// maps floating point channels (0.f to 1.f range) to D3DCOLOR
		public static uint D3DCOLOR_COLORVALUE(float r, float g, float b, float a) {
			return D3DCOLOR_RGBA((uint)((r) * 255.0), (uint)((g) * 255.0), (uint)((b) * 255.0), (uint)((a) * 255.0));
		}
		/* Bits for Flags in ProcessVertices call */

		public const uint D3DPV_DONOTCOPYDATA = (1 << 0);

		//-------------------------------------------------------------------

		// Flexible vertex format bits
		//
		public const uint D3DFVF_RESERVED0 = 0x001;
		public const uint D3DFVF_POSITION_MASK = 0x400E;
		public const uint D3DFVF_XYZ = 0x002;
		public const uint D3DFVF_XYZRHW = 0x004;
		public const uint D3DFVF_XYZB1 = 0x006;
		public const uint D3DFVF_XYZB2 = 0x008;
		public const uint D3DFVF_XYZB3 = 0x00a;
		public const uint D3DFVF_XYZB4 = 0x00c;
		public const uint D3DFVF_XYZB5 = 0x00e;
		public const uint D3DFVF_XYZW = 0x4002;

		public const uint D3DFVF_NORMAL = 0x010;
		public const uint D3DFVF_PSIZE = 0x020;
		public const uint D3DFVF_DIFFUSE = 0x040;
		public const uint D3DFVF_SPECULAR = 0x080;

		public const uint D3DFVF_TEXCOUNT_MASK = 0xf00;
		public const uint D3DFVF_TEXCOUNT_SHIFT = 8;
		public const uint D3DFVF_TEX0 = 0x000;
		public const uint D3DFVF_TEX1 = 0x100;
		public const uint D3DFVF_TEX2 = 0x200;
		public const uint D3DFVF_TEX3 = 0x300;
		public const uint D3DFVF_TEX4 = 0x400;
		public const uint D3DFVF_TEX5 = 0x500;
		public const uint D3DFVF_TEX6 = 0x600;
		public const uint D3DFVF_TEX7 = 0x700;
		public const uint D3DFVF_TEX8 = 0x800;

		public const uint D3DFVF_LASTBETA_UBYTE4 = 0x1000;
		public const uint D3DFVF_LASTBETA_D3DCOLOR = 0x8000;

		public const uint D3DFVF_RESERVED2 = 0x6000; // 2 reserved bits


	}
}