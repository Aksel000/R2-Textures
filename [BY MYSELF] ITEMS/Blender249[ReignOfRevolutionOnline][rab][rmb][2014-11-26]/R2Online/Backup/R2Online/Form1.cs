using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DirectX9.D3DX9;
using CsGL.OpenGL;
using OpenGL;
using System.Runtime.InteropServices;

namespace R2Online {
    public partial class Form1 :Form {
        public Form1() {
            InitializeComponent();

            this.view = new OurView();			// view
            this.view.Parent = this;
            this.view.Dock = DockStyle.Fill; // Will fill whole form
        }

        private OurView view;

        public class OurView :OpenGLControl {

            private float[] LightAmbient = { 1.0f, 1.0f, 1.0f, 1.0f }; 				// Ambient Light Values ( NEW )
            private float[] LightDiffuse = { 1.0f, 1.0f, 1.0f, 1.0f };				 // Diffuse Light Values ( NEW )
            protected float[] LightPosition = { 10000.0f, 10000.0f, 10000.0f, 1.0f };				 // Light Position ( NEW )
            public D3DXVECTOR3 vEye, vLookAt, vUp;
            private int drawMode = 0;
            const float STATIC_POS = 800.0f;

            public OurView()
                : base() {
                this.KeyPress += new KeyPressEventHandler(OurView_KeyPress);
                vEye = new D3DXVECTOR3(STATIC_POS, 0.0f, STATIC_POS);
                vLookAt = new D3DXVECTOR3(0.0f, 0.0f, 0.0f);
                vUp = new D3DXVECTOR3(0.0f, 1.0f, 0.0f);
            }
            const float MOVE = 50.0f;

            protected void OurView_KeyPress(object Sender, KeyPressEventArgs e) {
                //if escape was pressed exit the application
                if (e.KeyChar == (char)Keys.Escape) {
                    Application.Exit();
                } 
                if (e.KeyChar == 's') {
                    vEye.z -= MOVE;
                    vLookAt.z -= MOVE;
                } else if (e.KeyChar == 'w') {
                    vEye.z += MOVE;
                    vLookAt.z += MOVE;
                } 
                if (e.KeyChar == 'a') {
                    vEye.y -= MOVE;
                    vLookAt.y -= MOVE;
                } else if (e.KeyChar == 'q') {
                    vEye.y += MOVE;
                    vLookAt.y += MOVE;
                } 
                if (e.KeyChar == 'e') {
                    angle -= 5.0f;
                } else if (e.KeyChar == 'd') {
                    angle += 5.0f;
                } 
                if (e.KeyChar == 'f') {
                    vEye.x -= MOVE;
                    vLookAt.x -= MOVE;
                } else if (e.KeyChar == 'r') {
                    vEye.x += MOVE;
                    vLookAt.x += MOVE;
                }

                if (e.KeyChar == '0') {
                    drawMode = 0;
                } else if (e.KeyChar == '1') {
                    drawMode = 1;
                } else if (e.KeyChar == '2') {
                    drawMode = 2;
                } else if (e.KeyChar == '3') {
                    drawMode = 3;
                } else if (e.KeyChar == '4') {
                    drawMode = 4;
                } else if (e.KeyChar == '5') {
                    drawMode = 5;
                } else if (e.KeyChar == '6') {
                    drawMode = 6;
                } else if (e.KeyChar == '7') {
                    drawMode = 7;
                } else if (e.KeyChar == '8') {
                    drawMode = 8;
                }
            }

            long lastMs = 0;
            float angle = 0;

            public override void glDraw() {
                if (lastMs == 0)
                    lastMs = DateTime.Now.Ticks;
                long currentMs = DateTime.Now.Ticks;
                long milliseconds = (currentMs - lastMs) / 10000;
                lastMs = currentMs;										//Calculate elapsed timer

                GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
                GL.glLoadIdentity();

                //float fPos = 6000.0f;
                GLU.gluLookAt(vEye.x, vEye.y, vEye.z, vLookAt.x, vLookAt.y, vLookAt.z, vUp.x, vUp.y, vUp.z);
                //GLU.gluLookAt(fPos, fPos, fPos, 0, 0, 0, 0, 1, 0);
                GL.glRotatef(angle, 0.0f, 1.0f, 0.0f);

                float[] no_mat = { 0.0f, 0.0f, 0.0f, 1.0f };
                float[] mat_ambient = { 0.7f, 0.7f, 0.7f, 1.0f };
                float[] mat_ambient_color = { 0.2f, 0.2f, 0.2f, 1.0f };
                float[] mat_diffuse = { 0.8f, 0.8f, 0.8f, 1.0f };
                float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
                float no_shininess = 0.0f;
                float low_shininess = 5.0f;
                float high_shininess = 100.0f;
                float[] mat_emission = { 0.2f, 0.2f, 0.2f, 0.0f };

                for (int nMesh = 0; nMesh < model.nMeshCount; nMesh++) {
                    R3MMesh mesh = model.meshes[nMesh];

                    for (int n = 0; n < mesh.nObjCount; n++) {
                        MeshIndex info = mesh.info[n];

                        int nFaces = info.nIndexCount / 3;

                        if (mesh.textures != null) {
                            // defines the texture
                            if (mesh.textures[n] != 0) {
                                GL.glBindTexture(GL.GL_TEXTURE_2D, mesh.textures[n]);
                            }
                        }
                        GL.glBegin(GL.GL_TRIANGLES);
                        //GL.glBegin(GL.GL_LINES);
                        switch (drawMode) {
                            case 0:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, mat_ambient_color);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, mat_specular);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, high_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 1:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, mat_ambient_color);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, no_mat);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, no_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, mat_emission);
                                break;
                            case 2:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, no_mat);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, no_mat);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, no_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 3:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, no_mat);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, mat_specular);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, low_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 4:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, no_mat);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, mat_specular);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, high_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 5:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, no_mat);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, no_mat);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, no_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, mat_emission);
                                break;
                            case 6:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, mat_ambient);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, no_mat);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, no_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 7:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, mat_ambient);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, mat_specular);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, high_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, no_mat);
                                break;
                            case 8:
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, mat_ambient);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, mat_diffuse);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, no_mat);
                                GL.glMaterialf(GL.GL_FRONT, GL.GL_SHININESS, no_shininess);
                                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, mat_emission);
                                break;
                        }
                        for (int nFace = 0; nFace < nFaces; nFace++) {
                            int a = mesh.indices[info.nIndexStart + (3 * nFace + 0)];
                            int b = mesh.indices[info.nIndexStart + (3 * nFace + 1)];
                            int c = mesh.indices[info.nIndexStart + (3 * nFace + 2)];

                            R3MVertex v1, v2, v3;
                            v1 = mesh.vertices[info.nVertexStart + a];
                            v2 = mesh.vertices[info.nVertexStart + b];
                            v3 = mesh.vertices[info.nVertexStart + c];

                            GL.glColor4f(0.5f, 0.0f, 0.0f, 1.0f);

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
                    }
                }
                // Flush The GL Rendering Pipeline
                GL.glFlush();

                //uint hDC = WGL.GetDC(this.Handle);
                //WGL.wglSwapBuffers(hDC);
                //WGL.ReleaseDC(this.Handle, hDC);

                //angle += (float)(milliseconds) / 10.0f;
                this.Invalidate();
            }

            protected override void InitGLContext() {
                GL.glClearColor(0.1f, 0.1f, 0.1f, 0.0f);
                GL.glEnable(GL.GL_DEPTH_TEST);
                GL.glDepthFunc(GL.GL_LEQUAL);
                GL.glEnable(GL.GL_CULL_FACE);
                GL.glCullFace(GL.GL_BACK);
                GL.glShadeModel(GL.GL_SMOOTH);
                GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);

                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glEnable(GL.GL_BLEND);
                GL.glAlphaFunc(GL.GL_GREATER, 0.1f);
                GL.glEnable(GL.GL_ALPHA_TEST);
                GL.glEnable(GL.GL_TEXTURE_2D);

                //light properties
                float[] ambient = { 0.0f, 0.0f, 0.0f, 1.0f };
                float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
                float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f };
                float[] position = { 10000.0f, 10000.0f, 10000.0f, 1.0f };
                //float[] position = { 0.0f, 10.0f, 0.0f, 1.0f };

                GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, ambient);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, diffuse);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, position);

                //light model properties
                float[] model_ambient = { 0.8f, 0.8f, 0.8f, 1.0f };
                int model_two_side = 1;                                //0=2sided, 1=1sided
                int viewpoint = 0;                                     //0=infiniteViewpoint, 1=localViewpoint

                GL.glLightModeli(GL.GL_LIGHT_MODEL_LOCAL_VIEWER, viewpoint);

                //Only outside face because we don't see the inside of the spheres
                GL.glLightModeli(GL.GL_LIGHT_MODEL_TWO_SIDE, model_two_side);

                GL.glEnable(GL.GL_LIGHT0);
                GL.glEnable(GL.GL_LIGHTING);
                /*
                GL.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT, LightAmbient);				// Setup The Ambient Light
                GL.glLightfv(GL.GL_LIGHT1, GL.GL_DIFFUSE, LightDiffuse);				// Setup The Diffuse Light
                GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, LightPosition);			// Position The Light
                GL.glEnable(GL.GL_LIGHT1);
                */

                GL.glEnable(GL.GL_TEXTURE_2D);
                Console.Out.WriteLine("OpenGL initialized");
            }

            protected override void OnSizeChanged(EventArgs e) {
                base.OnSizeChanged(e);
                Size s = Size;

                GL.glMatrixMode(GL.GL_PROJECTION);
                GL.glLoadIdentity();
                //GL.gluPerspective(45.0f, (double)s.Width / (double)s.Height, 0.1f, 100.0f);
                GLU.gluPerspective(90.0, ((double)(s.Width) / (double)(s.Height)), 0.1f, 1000000.0f);
                GL.glMatrixMode(GL.GL_MODELVIEW);
                GL.glLoadIdentity();
                Console.Out.WriteLine("OpenGL resized");
            }
        }

        private string path = "C:\\models\\test\\";
        private string tpath = "C:\\models\\objects\\texture\\object\\";
        private string key_code = "4a3408a275b0343719ae2ab7250a8cab0c03b2178a58f2de";

        private struct MeshIndex {
            public int nVertexCount;
            public int nIndexCount;
            public int nVertexStart;
            public int nIndexStart;
        }
        private struct R3MVertex {
            public D3DXVECTOR3 pos;
            public D3DXVECTOR3 normal;
            public D3DXVECTOR2 uv;
            public byte[] diffuse;
            public D3DXVECTOR3 unk1;
            public D3DXVECTOR3 unk2;
            public D3DXVECTOR2 unk3;
        }

        private struct R3MMesh {
            public string fileName;
            public int nUnkCount1;
            public int nTextureCount;
            public int nUnkCount2;
            public D3DXVECTOR3 min;
            public D3DXVECTOR3 max;
            public string[] textureNames;
            public uint[] textures;
            public int nFlag;
            public int nObjCount;
            public int nVertexCount;
            public int nIndexCount;
            public MeshIndex[] info;
            public R3MVertex[] vertices;
            public short[] indices;
        }
        private struct R2Model {
            public int nMeshCount;
            public R3MMesh[] meshes;
        }
        private enum VertexType :int {
            pos = 1,
            normal = 2,
            uv = 3,
            unk1 = 4,
            unk2 = 5,
            unk3 = 6,
            diffuse = 7,
        }

        // static R3MMesh mesh = new R3MMesh();
        static R2Model model = new R2Model();

        private void loadModel() {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.r3m");

            model.nMeshCount = files.Length;
            model.meshes = new R3MMesh[model.nMeshCount];

            for (int n = 0; n < model.nMeshCount; n++) {
                FileInfo file = files[n];
                Console.Out.WriteLine("Loading file...{0}", file.Name);
                model.meshes[n] = loadMesh(file.OpenRead());
                model.meshes[n].fileName = file.Name;
            }
        }

        private R3MMesh loadMesh(FileStream fs) {
            BinaryReader br = new BinaryReader(fs, Encoding.ASCII);
            R3MMesh mesh = new R3MMesh();

            mesh.nUnkCount1 = br.ReadInt32();
            mesh.nTextureCount = br.ReadInt32();
            mesh.nUnkCount2 = br.ReadInt32();
            //Console.Out.WriteLine("Unknown 1: {0}", mesh.nUnkCount1);
            //Console.Out.WriteLine("Texture Count: {0}", mesh.nTextureCount);
            //Console.Out.WriteLine("Unknown 2: {0}", mesh.nUnkCount2);

            mesh.textureNames = new string[mesh.nTextureCount];
            mesh.textures = new uint[mesh.nTextureCount];

            mesh.max = new D3DXVECTOR3(br);
            mesh.min = new D3DXVECTOR3(br);
            if (mesh.max > view.vEye) {
                view.vEye = mesh.max;
                //view.vEye.y /= 2.0f;
            }
            //Console.Out.WriteLine("\tMin: {0}", mesh.min);
            //Console.Out.WriteLine("\tMax: {0}", mesh.max);

            GL.glGenTextures(mesh.nTextureCount, mesh.textures);

            br.BaseStream.Seek(0x400, SeekOrigin.Begin);
            for (int n = 0; n < mesh.nTextureCount; n++) {
                char[] ctexture = br.ReadChars(256);
                mesh.textureNames[n] = CropNull(new String(ctexture));
                loadImage(tpath + mesh.textureNames[n], n, ref mesh.textures);
                //Console.Out.WriteLine("Texture {0}: {1}", n + 1, mesh.textureNames[n]);
            }
            //Console.Out.WriteLine("Current Position: 0x" + br.BaseStream.Position.ToString("X"));

            mesh.nFlag = br.ReadInt32();
            mesh.nObjCount = br.ReadInt32();
            Console.Out.WriteLine("\tMesh Flag: {0}", mesh.nFlag);
            //Console.Out.WriteLine("Object Count: {0}", mesh.nObjCount);

            mesh.nVertexCount = br.ReadInt32();
            mesh.nIndexCount = br.ReadInt32();
            //Console.Out.WriteLine("Vertex Count: {0}", mesh.nVertexCount);
            //Console.Out.WriteLine("Index Count: {0}", mesh.nIndexCount);

            mesh.info = new MeshIndex[mesh.nObjCount];
            for (int n = 0; n < mesh.nObjCount; n++) {
                mesh.info[n].nVertexCount = br.ReadInt32();
                mesh.info[n].nIndexCount = br.ReadInt32();
                mesh.info[n].nVertexStart = br.ReadInt32();
                mesh.info[n].nIndexStart = br.ReadInt32();
                //Console.Out.WriteLine("\tMesh Index {0}: [Vertex Count: {1}, Index Count: {2}, Vertex Start: {3}, Index Start: {4}]", n + 1, info[n].nVertexCount, info[n].nIndexCount, info[n].nVertexStart, info[n].nIndexStart);
                //Console.Out.WriteLine("\tMesh Index {0}: [{1}, {2}, {3}, {4}]", n + 1, mesh.info[n].nVertexCount, mesh.info[n].nIndexCount, mesh.info[n].nVertexStart, mesh.info[n].nIndexStart);
            }
            //Console.Out.WriteLine("Current Position: 0x" + br.BaseStream.Position.ToString("X"));

            int[] order = { 1, 2, 3, 4, 5, 6 };
            /*
            pos = 1,
            normal = 2,
            uv = 3,
            unk1 = 4,
            unk2 = 5,
            unk3 = 6,
            diffuse = 7,
            */
            // correct:
            // pos = 1st
            // normals = 5th or 2nd?
            // uv = 3rd
            // 1, 4, 3, 5, 2 seems to work
            if (mesh.nFlag == 1295) {
                order = new int[] { 1, 2, 3, 6, 4, 5 };
                //Console.Out.WriteLine("Order is: {0}, {1}, {2}, {3}, {4}, {5}", order[0], order[1], order[2], order[3], order[4], order[5]);
            } else if (mesh.nFlag == 1311) {
                order = new int[] { 1, 2, 7, 3, 6, 4, 5 };
            } else {
                order = new int[] { 1, 2, 3, 4, 5 };
                //Console.Out.WriteLine("Order is: {0}, {1}, {2}, {3}, {4}", order[0], order[1], order[2], order[3], order[4]);
            }

            mesh.vertices = new R3MVertex[mesh.nVertexCount];
            for (int n = 0; n < order.Length; n++) {
                switch ((VertexType)order[n]) {
                    case VertexType.pos:
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            D3DXVECTOR3 pos = new D3DXVECTOR3(br);

                            mesh.vertices[i].pos = pos;
                        }
                        break;
                    case VertexType.normal:
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            D3DXVECTOR3 normal = new D3DXVECTOR3(br);

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
                            D3DXVECTOR3 unk1 = new D3DXVECTOR3(br);

                            mesh.vertices[i].unk1 = unk1;
                        }
                        break;
                    case VertexType.unk2:
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            D3DXVECTOR3 unk2 = new D3DXVECTOR3(br);

                            mesh.vertices[i].unk2 = unk2;
                        }
                        break;
                    case VertexType.unk3:
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            D3DXVECTOR2 unk3 = new D3DXVECTOR2();
                            unk3.x = br.ReadSingle();
                            unk3.y = br.ReadSingle();

                            mesh.vertices[i].unk3 = unk3;
                        }
                        break;
                    case VertexType.diffuse:
                        for (int i = 0; i < mesh.nVertexCount; i++) {
                            mesh.vertices[i].diffuse = br.ReadBytes(4);
                        }
                        break;
                }
            }

            mesh.indices = new short[mesh.nIndexCount];
            for (int n = 0; n < mesh.nIndexCount; n++) {
                mesh.indices[n] = br.ReadInt16();
            }

            //Console.Out.WriteLine("Current Position: 0x" + br.BaseStream.Position.ToString("X"));
            //Console.Out.WriteLine("File Length: 0x" + br.BaseStream.Length.ToString("X"));
            br.Close();
            fs.Close();

            return mesh;
        }
        private void Form1_Load(object sender, EventArgs e) {
            Console.Out.WriteLine("Key Code: " + key_code);
            loadModel();
        }

        private string CropNull(string input) {
            input = input.Trim();

            if (input.EndsWith(((char)13).ToString()) && input.IndexOf('\0') == -1) {
                return input.Substring(0, input.Length - 1);
            }

            if (input.IndexOf('\0') == -1)
                return input;

            return input.Substring(0, input.IndexOf('\0'));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            Console.Out.WriteLine("Closing form...deleting textures");
            for (int n = 0; n < model.nMeshCount; n++) {
                R3MMesh mesh = model.meshes[n];
                GL.glDeleteTextures(mesh.nTextureCount, mesh.textures);
            }
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

        private void loadImage(string filename, int nIndex, ref uint[] textures) {
            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists) {
                Console.Out.WriteLine("Texture: {0} does not exist!", filename);
                textures[nIndex] = 0;
                return;
            }
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

            //bm.Save("C:\\temp" + nIndex + ".png", ImageFormat.Png);
            bm.Save(ms, ImageFormat.Png);
            bm.Dispose();

            Bitmap image = new Bitmap(ms);
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.glBindTexture(GL.GL_TEXTURE_2D, textures[nIndex]);
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
