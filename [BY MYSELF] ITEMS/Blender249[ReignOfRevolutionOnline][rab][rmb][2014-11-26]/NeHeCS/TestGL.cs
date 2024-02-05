using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OpenGL;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Text;

namespace MyFormProject {
    /// <summary>
    /// Example implementation of the BaseGLControl
    /// </summary>
    public class TestGL :BaseGLControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private const int MAX_FVF_DECL_SIZE = 65;

        [StructLayout(LayoutKind.Sequential)]
        private struct D3DVERTEXELEMENT9 {
            public short Stream;
            public short Offset;
            public byte Type;
            public byte Method;
            public byte Usage;
            public byte UsageIndex;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct D3DXMATRIX {
            public float _11, _12, _13, _14;
            public float _21, _22, _23, _24;
            public float _31, _32, _33, _34;
            public float _41, _42, _43, _44;
        }

        bool direction = false;
        public TestGL(bool direction) {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            readRMB();
            this.direction = direction;
            this.KeyPress += new KeyPressEventHandler(TestGL_KeyPress);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
        }
        #endregion

        long lastMs = 0;
        float angle = 0;
        /// <summary>
        /// Override OnPaint to draw our gl scene
        /// </summary>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            if (DC == 0 || RC == 0)
                return;

            if (lastMs == 0)
                lastMs = DateTime.Now.Ticks;
            long currentMs = DateTime.Now.Ticks;
            long milliseconds = (currentMs - lastMs) / 10000;
            lastMs = currentMs;										//Calculate elapsed timer

            WGL.wglMakeCurrent(DC, RC);

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            if (texture != null) {
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);					// defines the texture
            }
            GL.glLoadIdentity();

            //float fPos = 600.0f;
            GLU.gluLookAt(vEye.x, vEye.y, vEye.z, vLookAt.x, vLookAt.y, vLookAt.z, 0, 1, 0);
            //GLU.gluLookAt(fPos, fPos, fPos, 0, 0, 0, 0, 1, 0);

            GL.glRotatef(angle, 0.0f, 1.0f, 0.0f);
            for (int nMesh = 0; nMesh < r2Mesh.meshCount; nMesh++) {
                mesh = r2Mesh.meshes[nMesh];

                GL.glBegin(GL.GL_TRIANGLES);
                int nFaces = mesh.nIndexCount / 3;
                for (int n = 0; n < nFaces; n++) {
                    GL.glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
                    int a = mesh.indices[3 * n + 0];
                    int b = mesh.indices[3 * n + 1];
                    int c = mesh.indices[3 * n + 2];

                    R2Vertex v1, v2, v3;
                    v1 = mesh.vertices[a];
                    v2 = mesh.vertices[b];
                    v3 = mesh.vertices[c];

                    float[] pModel = new float[16];
                    GL.glGetFloatv(GL.GL_MODELVIEW_MATRIX, ref pModel[0]);

                    GL.glTexCoord2f(v1.uv.x, v1.uv.y);
                    GL.glNormal3f(v1.normal.x, v1.normal.y, v1.normal.z);
                    GL.glVertex3f(v1.pos.x, v1.pos.y, v1.pos.z);

                    GL.glTexCoord2f(v2.uv.x, v2.uv.y);
                    GL.glNormal3f(v2.normal.x, v2.normal.y, v2.normal.z);
                    GL.glVertex3f(v2.pos.x, v2.pos.y, v2.pos.z);

                    GL.glTexCoord2f(v3.uv.x, v3.uv.y);
                    GL.glNormal3f(v3.normal.x, v3.normal.y, v3.normal.z);
                    GL.glVertex3f(v3.pos.x, v3.pos.y, v3.pos.z);
                }
                GL.glEnd();
                GL.glFlush();													// Flush The GL Rendering Pipeline
            }
            WGL.wglSwapBuffers(DC);
            angle += (float)(milliseconds) / 10.0f;
        }

        /// <summary>
        /// Handle keys, specifically escape
        /// </summary>
        private void TestGL_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Escape) {
                MainForm.App.Close();
            } else if (e.KeyChar == 'w') {
                if ((vEye.z - 5) < 0) {
                    vEye.z = 0;
                } else {
                    vEye.z -= 5;
                }
            } else if (e.KeyChar == 's') {
                vEye.z += 5;
            } else if (e.KeyChar == 'q') {
                if ((vEye.z - 5) < 0) {
                    vEye.y = 0;
                } else {
                    vEye.y -= 5;
                }
            } else if (e.KeyChar == 'a') {
                vEye.y += 5;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR4 {
            public float x, y, z, w;
            public D3DXVECTOR4(float x, float y, float z, float w) {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
            public D3DXVECTOR4(float v) {
                this.x = v;
                this.y = v;
                this.z = v;
                this.w = v;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR3 {
            public float x, y, z;
            public D3DXVECTOR3(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public D3DXVECTOR3(float v) {
                this.x = v;
                this.y = v;
                this.z = v;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR2 {
            public float x, y;
            public D3DXVECTOR2(float x, float y) {
                this.x = x;
                this.y = y;
            }
            public D3DXVECTOR2(float v) {
                this.x = v;
                this.y = v;
            }
        }
        private struct R2RMBFile {
            public int textureCount, meshCount, boneCount;
            public int dataOffset;
            public string[] textureFiles;
            public R2Mesh[] meshes;
            public D3DXMATRIX[] bones;
        }
        private struct R2Mesh {
            public int index, reserved;
            public int res1, res2;
            public string name, parent;
            public int nVertexCount, nIndexCount;
            /*public D3DXVECTOR3[] pos;
            public D3DXVECTOR3[] normal;
            public D3DXVECTOR2[] uv;
            public D3DXVECTOR3[] unk1;
            public D3DXVECTOR3[] unk2;
            public D3DXVECTOR4[] boneWeights;
            public short[,] matrixBoneIndicies;*/
            public R2Vertex[] vertices;
            public short[] indices;
        }
        private struct R2Vertex {
            public D3DXVECTOR3 pos;
            public D3DXVECTOR3 normal;
            public D3DXVECTOR2 uv;
            public D3DXVECTOR4 weight;
            public short[] boneIndices;
            public D3DXVECTOR3 unk1;
            public D3DXVECTOR3 unk2;
        }
        private R2Mesh mesh;
        private R2RMBFile r2Mesh;
        private enum VertexType :int {
            pos = 1,
            normal = 2,
            uv = 3,
            unk1 = 4,
            unk2 = 5
        }

        protected override void loaded() {
            GL.glGenTextures(r2Mesh.textureCount, texture);

            for (int n = 0; n < r2Mesh.textureCount; n++) {
                loadImage(r2Mesh.textureFiles[n], n);
            }
        }

        private string formatString(int iLen, char[] chars) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iLen; i++) {
                if (chars[i] != '\0') {
                    sb.Append(chars[i]);
                }
            }

            return sb.ToString();
        }


        private D3DXVECTOR3 min, max, vEye, vLookAt;
        const string path = "C:\\models\\";
        private void readRMB() {
            string meshFile = path + "p070000.rmb";

            Console.Out.WriteLine("meshFile = " + meshFile);
            Clipboard.SetText(meshFile);

            int[] order = { 1, 2, 3, 4, 5 };
            /*
            pos = 1,
            normal = 2,
            uv = 3,
            unk1 = 4,
            unk2 = 5
            */
            // correct:
            // pos = 1st
            // normals = 5th or 2nd?
            // uv = 3rd
            order = new int[] { 1, 4, 3, 5, 2 }; // 1, 4, 3, 5, 2 seems to work
            Console.Out.WriteLine("Order is: {0}, {1}, {2}, {3}, {4}", order[0], order[1], order[2], order[3], order[4]);

            if (!File.Exists(meshFile)) {
                return;
            }
            FileStream fs = new FileStream(meshFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(fs, System.Text.Encoding.ASCII);

            br.BaseStream.Seek(0x14, SeekOrigin.Begin);
            r2Mesh = new R2RMBFile();

            int nOffset = 0;
            long nInfoStart = 0;
            if (fs.Name.EndsWith(".rmb")) {
                r2Mesh.textureCount = br.ReadInt32();
                r2Mesh.meshCount = br.ReadInt32();
                r2Mesh.boneCount = br.ReadInt32();
                r2Mesh.dataOffset = br.ReadInt32();

                texture = new uint[r2Mesh.textureCount];

                r2Mesh.meshes = new R2Mesh[r2Mesh.meshCount];
                r2Mesh.textureFiles = new string[r2Mesh.textureCount];
                for (int n = 0; n < r2Mesh.textureCount; n++) {
                    string temp = "";
                    char[] cData = br.ReadChars(260);
                    for (int j = 0; j < 260; j++) {
                        if (cData[j] == 0) {
                            break;
                        }
                        temp += cData[j];
                    }
                    r2Mesh.textureFiles[n] = path + "texture\\" + temp;
                }

                nOffset = r2Mesh.dataOffset;
                nInfoStart = br.BaseStream.Position + 136;
            } else {
                br.BaseStream.Seek(0x0C, SeekOrigin.Begin);
                max = new D3DXVECTOR3();
                max.x = br.ReadSingle();
                max.y = br.ReadSingle();
                max.z = br.ReadSingle();

                min = new D3DXVECTOR3();
                min.x = br.ReadSingle();
                min.y = br.ReadSingle();
                min.z = br.ReadSingle();

                nOffset = 0x520;
                nInfoStart = 0x510;
            }

            vEye = new D3DXVECTOR3(80.0f, 90.0f, 80.0f);
            vLookAt = new D3DXVECTOR3(0.0f, 80.0f, 0.0f);

            LightPosition[0] = vEye.x;
            LightPosition[1] = vEye.y;
            LightPosition[2] = vEye.z;

            for (int nMesh = 0; nMesh < r2Mesh.meshCount; nMesh++) {
                mesh = new R2Mesh();
                mesh.index = br.ReadInt32();
                mesh.reserved = br.ReadInt32();

                char[] cData = br.ReadChars(64);
                for (int j = 0; j < 260; j++) {
                    if (cData[j] == 0) {
                        break;
                    }
                    mesh.name += cData[j];
                }
                cData = br.ReadChars(64);
                for (int j = 0; j < 260; j++) {
                    if (cData[j] == 0) {
                        break;
                    }
                    mesh.parent += cData[j];
                }
                mesh.res1 = br.ReadInt32();
                mesh.res2 = br.ReadInt32();
                if (mesh.res2 != 1) {
                    Console.Out.WriteLine("!!!");
                }
                mesh.nVertexCount = br.ReadInt32();
                mesh.nIndexCount = br.ReadInt32();

                r2Mesh.meshes[nMesh] = mesh;

                br.BaseStream.Seek(0x7D0, SeekOrigin.Current);
            }
            r2Mesh.bones = new D3DXMATRIX[r2Mesh.boneCount];
            for (int nBone = 0; nBone < r2Mesh.boneCount; nBone++) {
                br.ReadBytes(92);
                char[] name = br.ReadChars(64);
                char[] parent = br.ReadChars(64);

                r2Mesh.bones[nBone]._11 = br.ReadSingle();
                r2Mesh.bones[nBone]._12 = br.ReadSingle();
                r2Mesh.bones[nBone]._13 = br.ReadSingle();
                r2Mesh.bones[nBone]._14 = br.ReadSingle();
                r2Mesh.bones[nBone]._21 = br.ReadSingle();
                r2Mesh.bones[nBone]._22 = br.ReadSingle();
                r2Mesh.bones[nBone]._23 = br.ReadSingle();
                r2Mesh.bones[nBone]._24 = br.ReadSingle();
                r2Mesh.bones[nBone]._31 = br.ReadSingle();
                r2Mesh.bones[nBone]._32 = br.ReadSingle();
                r2Mesh.bones[nBone]._33 = br.ReadSingle();
                r2Mesh.bones[nBone]._34 = br.ReadSingle();
                r2Mesh.bones[nBone]._41 = br.ReadSingle();
                r2Mesh.bones[nBone]._42 = br.ReadSingle();
                r2Mesh.bones[nBone]._43 = br.ReadSingle();
                r2Mesh.bones[nBone]._44 = br.ReadSingle();

                br.ReadBytes(64);
                br.ReadBytes(64);
            }

            br.BaseStream.Seek(nOffset, SeekOrigin.Begin);
            for (int nMesh = 0; nMesh < r2Mesh.meshCount; nMesh++) {
                mesh = r2Mesh.meshes[nMesh];
                mesh.vertices = new R2Vertex[mesh.nVertexCount];

                for (int n = 0; n < order.Length; n++) {
                    switch ((VertexType)order[n]) {
                        case VertexType.pos:
                            for (int i = 0; i < mesh.nVertexCount; i++) {
                                D3DXVECTOR3 pos = new D3DXVECTOR3();
                                pos.x = br.ReadSingle();
                                pos.y = br.ReadSingle();
                                pos.z = br.ReadSingle();

                                mesh.vertices[i].pos = pos;
                            }
                            break;
                        case VertexType.normal:
                            for (int i = 0; i < mesh.nVertexCount; i++) {
                                D3DXVECTOR3 normal = new D3DXVECTOR3();
                                normal.x = br.ReadSingle();
                                normal.y = br.ReadSingle();
                                normal.z = br.ReadSingle();

                                mesh.vertices[i].normal = normal;
                            }
                            break;
                        case VertexType.uv:
                            for (int i = 0; i < mesh.nVertexCount; i++) {
                                D3DXVECTOR2 uv = new D3DXVECTOR2();
                                uv.x = br.ReadSingle();
                                uv.y = br.ReadSingle();

                                mesh.vertices[i].uv = uv;
                            }
                            break;
                        case VertexType.unk1:
                            for (int i = 0; i < mesh.nVertexCount; i++) {
                                D3DXVECTOR3 unk1 = new D3DXVECTOR3();
                                unk1.x = br.ReadSingle();
                                unk1.y = br.ReadSingle();
                                unk1.z = br.ReadSingle();

                                mesh.vertices[i].unk1 = unk1;
                            }
                            break;
                        case VertexType.unk2:
                            for (int i = 0; i < mesh.nVertexCount; i++) {
                                D3DXVECTOR3 unk2 = new D3DXVECTOR3();
                                unk2.x = br.ReadSingle();
                                unk2.y = br.ReadSingle();
                                unk2.z = br.ReadSingle();

                                mesh.vertices[i].unk2 = unk2;
                            }
                            break;
                    }
                }
                if (mesh.res1 == 1) {
                    if (fs.Name.EndsWith(".rmb")) {
                        Console.Out.WriteLine("pos = " + br.BaseStream.Position);
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            D3DXVECTOR4 boneWeight = new D3DXVECTOR4();
                            boneWeight.x = br.ReadSingle();
                            boneWeight.y = br.ReadSingle();
                            boneWeight.z = br.ReadSingle();
                            boneWeight.w = br.ReadSingle();

                            mesh.vertices[i].weight = boneWeight;
                        }

                        Console.Out.WriteLine("pos = " + br.BaseStream.Position);
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            mesh.vertices[i].boneIndices = new short[4];

                            mesh.vertices[i].boneIndices[0] = br.ReadInt16();
                            mesh.vertices[i].boneIndices[1] = br.ReadInt16();
                            mesh.vertices[i].boneIndices[2] = br.ReadInt16();
                            mesh.vertices[i].boneIndices[3] = br.ReadInt16();
                        }
                    }
                }
                mesh.indices = new short[mesh.nIndexCount];
                for (int i = 0; i < mesh.nIndexCount; i++) {
                    mesh.indices[i] = br.ReadInt16();
                }
                r2Mesh.meshes[nMesh] = mesh;
            }
            br.Close();
            fs.Close();
        }

        #region DDS loading methods
        public struct DDS_Header {
            public int dwMagic;
            public int dwSize;
            public DDSD dwFlags;
            public int dwHeight;
            public int dwWidth;
            public int dwPitchOrLinearSize;
            public int dwDepth;
            public int dwMipMapCount;
            public int[] dwReserved1;
            public DDPIXELFORMAT sPixelFormat;
            public DDSCAPS sCaps;
            public int dwReserved2;
        }
        public struct DDSURFACEDESC2 {
            public int dwSize;
            public DDSD dwHeaderFlags;
            public int dwHeight;
            public int dwWidth;
            public int dwPitchOrLinearSize;
            public int dwDepth; // only if DDS_HEADER_FLAGS_VOLUME is set in dwHeaderFlags
            public int dwMipMapCount;
            public int[] dwReserved1;//[11];
            public DDPIXELFORMAT ddpfPixelFormat;
            public int dwPixelFormat;
            public int dwSurfaceFlags;
            public int dwCubemapFlags;
            public int[] dwReserved2;//[3]
            public DDSCAPS ddsCaps2;
        };
        public struct DDPIXELFORMAT {
            public int dwSize;
            public DDPF dwFlags;
            public DDSFOURCC dwFourCC;
            public int dwRGBBitCount;
            public int dwRBitMask;
            public int dwGBitMask;
            public int dwBBitMask;
            public int dwABitMask;
        };
        [Flags]
        public enum DDPF {
            DDPF_ALPHAPIXELS = 0x00000001,
            DDPF_FOURCC = 0x00000004,
            DDPF_RGB = 0x00000040,

        }
        public enum D3DXIMAGE_FILEFORMAT {
            D3DXIFF_BMP = 0,
            D3DXIFF_JPG = 1,
            D3DXIFF_TGA = 2,
            D3DXIFF_PNG = 3,
            D3DXIFF_DDS = 4,
            D3DXIFF_PPM = 5,
            D3DXIFF_DIB = 6,
            D3DXIFF_HDR = 7,
            D3DXIFF_PFM = 8,
            D3DXIFF_FORCE_DWORD = 0x7fffffff,
        }
        public enum D3DRESOURCETYPE {
            D3DRTYPE_SURFACE = 1,
            D3DRTYPE_VOLUME = 2,
            D3DRTYPE_TEXTURE = 3,
            D3DRTYPE_VOLUMETEXTURE = 4,
            D3DRTYPE_CUBETEXTURE = 5,
            D3DRTYPE_VERTEXBUFFER = 6,
            D3DRTYPE_INDEXBUFFER = 7,
            D3DRTYPE_FORCE_DWORD = 0x7fffffff,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXIMAGE_INFO {
            public int Width;
            public int Height;
            public int Depth;
            public int MipLevels;
            public int Format;
            D3DRESOURCETYPE ResourceType;
            D3DXIMAGE_FILEFORMAT ImageFileFormat;
        }
        [Flags]
        public enum DDSD {
            DDSD_CAPS = 0x00000001,
            DDSD_HEIGHT = 0x00000002,
            DDSD_WIDTH = 0x00000004,
            DDSD_PITCH = 0x00000008,
            DDSD_PIXELFORMAT = 0x00001000,
            DDSD_MIPMAPCOUNT = 0x00020000,
            DDSD_LINEARSIZE = 0x00080000,
            DDSD_DEPTH = 0x00800000,
        }
        public struct DDSCAPS {
            public DDSCAPS1 dwCaps;
            public DDSCAPS2 dwCaps2;
            public int dwCaps3;
            public int dwCaps4;
        }
        public enum DDSCAPS1 {
            DDSCAPS_COMPLEX = 0x00000008,
            DDSCAPS_TEXTURE = 0x00001000,
            DDSCAPS_MIPMAP = 0x00400000,
        }
        public enum DDSFOURCC {
            DXT1 = 0x31545844,
            DXT2 = 0x32545844,
            DXT3 = 0x33545844
        }
        public enum DDSCAPS2 {
            DDSCAPS2_CUBEMAP = 0x00000200,
            DDSCAPS2_CUBEMAP_POSITIVEX = 0x00000400,
            DDSCAPS2_CUBEMAP_NEGATIVEX = 0x00000800,
            DDSCAPS2_CUBEMAP_POSITIVEY = 0x00001000,
            DDSCAPS2_CUBEMAP_NEGATIVEY = 0x00002000,
            DDSCAPS2_CUBEMAP_POSITIVEZ = 0x00004000,
            DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x00008000,
            DDSCAPS2_VOLUME = 0x00200000,

        }
        public const int DDS_FOURCC = 0x00000004; // DDPF_FOURCC
        public const int DDS_RGB = 0x00000040; // DDPF_RGB
        public const int DDS_RGBA = 0x00000041; // DDPF_RGB | DDPF_ALPHAPIXELS

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct D3DXCOLOR {
            [FieldOffset(0)]
            public byte r;
            [FieldOffset(1)]
            public byte g;
            [FieldOffset(2)]
            public byte b;
            [FieldOffset(3)]
            public byte a;
            [FieldOffset(0)]
            public uint col;
        }
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct D3DXCOLOR32 {
            [FieldOffset(0)]
            public byte b;
            [FieldOffset(1)]
            public byte g;
            [FieldOffset(2)]
            public byte r;
            [FieldOffset(3)]
            public byte a;
            [FieldOffset(0)]
            public uint col;
            [FieldOffset(0)]
            public int color;
        }
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct D3DXCOLOR24 {
            [FieldOffset(0)]
            public byte b;
            [FieldOffset(1)]
            public byte g;
            [FieldOffset(2)]
            public byte r;
        }
        static D3DXCOLOR Col565To32(ushort Col) {
            D3DXCOLOR c = new D3DXCOLOR();
            c.a = 0xff;
            c.r = (byte)((long)(Col >> 11) * 255 / 31);
            c.g = (byte)((long)((Col >> 5) & 0x3F) * 255 / 63);
            c.b = (byte)((long)(Col & 0x1F) * 255 / 31);

            return c;
        }

        unsafe void PlotDXT1(ushort* pSrc, D3DXCOLOR* pDest, long Pitch, bool DXT1) {
            D3DXCOLOR[] Col = new D3DXCOLOR[4];
            long r, g, b, xx, yy;

            if ((pSrc[0] > pSrc[1]) || !DXT1) {
                Col[0] = Col565To32(pSrc[0]);
                Col[1] = Col565To32(pSrc[1]);
                pSrc += 2;

                r = (Col[0].r * 2 + Col[1].r) / 3;
                g = (Col[0].g * 2 + Col[1].g) / 3;
                b = (Col[0].b * 2 + Col[1].b) / 3;
                Col[2].a = 0xff;
                Col[2].r = (byte)r;
                Col[2].g = (byte)g;
                Col[2].b = (byte)b;

                r = (Col[1].r * 2 + Col[0].r) / 3;
                g = (Col[1].g * 2 + Col[0].g) / 3;
                b = (Col[1].b * 2 + Col[0].b) / 3;
                Col[3].a = 0xff;
                Col[3].r = (byte)r;
                Col[3].g = (byte)g;
                Col[3].b = (byte)b;

                int Shift = 0;
                for (yy = 0; yy < 4; yy++) {
                    for (xx = 0; xx < 4; xx++, Shift += 2)
                        pDest[xx] = Col[(pSrc[0] >> Shift) & 3];

                    pSrc += (yy & 1);
                    Shift &= 0x0f;
                    pDest += Pitch;
                }
            } else {
                Col[0] = Col565To32(pSrc[0]);
                Col[1] = Col565To32(pSrc[1]);
                pSrc += 2;

                r = (Col[0].r + Col[1].r) / 2;
                g = (Col[0].g + Col[1].g) / 2;
                b = (Col[0].b + Col[1].b) / 2;
                Col[2].a = 0xff;
                Col[2].r = (byte)r;
                Col[2].g = (byte)g;
                Col[2].b = (byte)b;
                Col[3].col = 0;

                int Shift = 0;
                for (yy = 0; yy < 4; yy++) {
                    for (xx = 0; xx < 4; xx++, Shift += 2)
                        pDest[xx] = Col[(pSrc[0] >> Shift) & 3];

                    pSrc += (yy & 1);
                    Shift &= 0x0f;
                    pDest += Pitch;
                }
            }
        }

        unsafe void PlotDXT3Alpha(ushort* pSrc, D3DXCOLOR* pDest, long Pitch) {
            ushort xx, yy;

            for (yy = 0; yy < 4; yy++) {
                for (xx = 0; xx < 4; xx++) {
                    int iAlpha = (((pSrc[yy] >> (xx * 4)) & 0x0F) << 4);
                    pDest[xx].a = (byte)(iAlpha);
                }
                pDest += Pitch;
            }
        }
        #endregion

        private void loadImage(string filename, int nIndex) {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader r = new BinaryReader(fs);

            int iDataSize = (int)r.BaseStream.Length - 3;

            char[] geo = r.ReadChars(3);
            byte[] filedata = r.ReadBytes(iDataSize);
            r.BaseStream.Seek(3, SeekOrigin.Begin);

            byte[] data = null;

            DDS_Header header;
            //header.dwMagic = r.ReadInt32();
            r.ReadByte();
            header.dwSize = r.ReadInt32();
            header.dwFlags = (DDSD)r.ReadInt32();
            header.dwHeight = r.ReadInt32();
            header.dwWidth = r.ReadInt32();
            header.dwPitchOrLinearSize = r.ReadInt32();
            header.dwDepth = r.ReadInt32();
            header.dwMipMapCount = r.ReadInt32();
            header.dwReserved1 = new int[11];
            for (int i = 0; i < 11; i++) {
                header.dwReserved1[i] = r.ReadInt32();
            }
            DDSFOURCC dwFourCC = DDSFOURCC.DXT3;
            if ((header.dwFlags & DDSD.DDSD_PIXELFORMAT) == DDSD.DDSD_PIXELFORMAT) {
                header.sPixelFormat = new DDPIXELFORMAT();
                header.sPixelFormat.dwSize = r.ReadInt32();
                header.sPixelFormat.dwFlags = (DDPF)r.ReadInt32();
                header.sPixelFormat.dwFourCC = (DDSFOURCC)r.ReadInt32();
                dwFourCC = header.sPixelFormat.dwFourCC;
            }
            if ((header.dwFlags & DDSD.DDSD_CAPS) == DDSD.DDSD_CAPS) {
                header.sCaps.dwCaps = (DDSCAPS1)r.ReadInt32();
                header.sCaps.dwCaps2 = (DDSCAPS2)r.ReadInt32();
                header.sCaps.dwCaps3 = r.ReadInt32();
                header.sCaps.dwCaps4 = r.ReadInt32();
            }
            header.dwReserved2 = r.ReadInt32();
            r.ReadBytes(20);

            int xSize = header.dwWidth;
            int ySize = header.dwHeight;
            int x = xSize;
            int y = ySize;
            int iDivSize = 4;
            int iBlockBytes = 8;

            int iMipMapCount = (((header.dwFlags & DDSD.DDSD_MIPMAPCOUNT) == DDSD.DDSD_MIPMAPCOUNT) ? header.dwMipMapCount : header.dwMipMapCount);
            int iSize = 0;

            int[] iMipMapSize = new int[iMipMapCount];
            int iTotalSize = iDataSize - header.dwSize - 4;

            for (int i = 0; i < iMipMapCount; i++) {
                iSize = Math.Max(iDivSize, x) / iDivSize * Math.Max(iDivSize, y) / iDivSize * iBlockBytes;
                iMipMapSize[i] = iSize;
                x = (x + 1) >> 1;
                y = (y + 1) >> 1;
            }
            int iOffset = 0;
            for (int i = iMipMapCount; i > 1; i--) {
                iOffset += iMipMapSize[i - 1];
            }
            data = new byte[iTotalSize];
            data = r.ReadBytes(iTotalSize);

            Bitmap bm = new Bitmap(xSize, ySize, PixelFormat.Format32bppArgb);
            BitmapData bmData = bm.LockBits(new Rectangle(0, 0, xSize, ySize),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe {
                fixed (void* pData = data) {
                    if (dwFourCC == DDSFOURCC.DXT1) {
                        ushort* pSrc = (ushort*)pData;
                        D3DXCOLOR* pPix = (D3DXCOLOR*)bmData.Scan0;
                        for (y = 0; y < ySize; y += 4) {
                            for (x = 0; x < xSize; x += 4) {
                                PlotDXT1(pSrc, pPix + x, xSize, true);
                                pSrc += 4;
                            }

                            pPix += xSize * 4;
                        }
                    } else if (dwFourCC == DDSFOURCC.DXT3) {
                        D3DXCOLOR* pPix = (D3DXCOLOR*)bmData.Scan0;
                        ushort* pSrc = (ushort*)pData;
                        for (y = 0; y < ySize; y += 4) {
                            for (x = 0; x < xSize; x += 4) {
                                PlotDXT1(pSrc + 4, pPix + x, xSize, false);
                                PlotDXT3Alpha(pSrc, pPix + x, xSize);
                                pSrc += 8;
                            }

                            pPix += xSize * 4;
                        }
                    }

                }
            }

            bm.UnlockBits(bmData);
            MemoryStream ms = new MemoryStream();

            bm.Save(ms, ImageFormat.Png);
            bm.Dispose();

            Bitmap image = new Bitmap(ms);
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[nIndex]);
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGBA, xSize, ySize,
                0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, bitmapdata.Scan0);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);		// Linear Filtering
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();

            ms.Dispose();

        }
    }
}
