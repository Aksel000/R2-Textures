//************************************************************************
//
// GLU bridge file.
// Implements GLU constants
// Maps GLU functions to their respective entry points in the glu32 dll
//
// Original work by Colin Fahey, http://www.colinfahey.com/opengl/csharp.htm
// Adapted by Chris Hegarty, avariant@hotmail.com
//   Cleaned import functions so that they compile under safe (pointer free) code.
//   Not all functions have been tested, so feel free to report problems or bugs to
//   me at avariant@hotmail.com
//**************************************************************************
using System;
using System.Runtime.InteropServices; 

namespace OpenGL
{
	/// <summary>
	/// Bridge class to the glu32 dll.  Provides constants and function definitions.
	/// </summary>
	public class GLU
	{
		public const string GLU_DLL ="glu32";

		/**** Generic constants****/

		/* Version */
		public const uint GLU_VERSION_1_1 = 1;
		public const uint GLU_VERSION_1_2 = 1;

		/* Errors: (return value 0 = no error) */
		public const uint GLU_INVALID_ENUM = 100900;
		public const uint GLU_INVALID_VALUE= 100901;
		public const uint GLU_OUT_OF_MEMORY= 100902;
		public const uint GLU_INCOMPATIBLE_GL_VERSION= 100903;

		/* StringName */
		public const uint GLU_VERSION = 100800;
		public const uint GLU_EXTENSIONS  = 100801;

		/* Boolean */
		public const uint GLU_TRUE  = 1; // GL_TRUE;
		public const uint GLU_FALSE  = 0; // GL_FALSE;

		/**** Quadric constants****/

		/* QuadricNormal */
		public const uint GLU_SMOOTH = 100000;
		public const uint GLU_FLAT  = 100001;
		public const uint GLU_NONE  = 100002;

		/* QuadricDrawStyle */
		public const uint GLU_POINT  = 100010;
		public const uint GLU_LINE  = 100011;
		public const uint GLU_FILL  = 100012;
		public const uint GLU_SILHOUETTE  = 100013;

		/* QuadricOrientation */
		public const uint GLU_OUTSIDE = 100020;
		public const uint GLU_INSIDE = 100021;

		/* Callback types: */
		/* GLU_ERROR100103 */

		/**** Tesselation constants ****/

		public const double GLU_TESS_MAX_COORD  = 1.0e150;

		/* TessProperty */
		public const uint GLU_TESS_WINDING_RULE = 100140;
		public const uint GLU_TESS_BOUNDARY_ONLY= 100141;
		public const uint GLU_TESS_TOLERANCE  = 100142;

		/* TessWinding */
		public const uint GLU_TESS_WINDING_ODD = 100130;
		public const uint GLU_TESS_WINDING_NONZERO  = 100131;
		public const uint GLU_TESS_WINDING_POSITIVE = 100132;
		public const uint GLU_TESS_WINDING_NEGATIVE = 100133;
		public const uint GLU_TESS_WINDING_ABS_GEQ_TWO  = 100134;

		/* TessCallback */
		public const uint GLU_TESS_BEGIN  = 100100;/* void (CALLBACK*)(GLenum  type) */
		public const uint GLU_TESS_VERTEX = 100101;/* void (CALLBACK*)(void *data) */
		public const uint GLU_TESS_END= 100102;/* void (CALLBACK*)(void) */
		public const uint GLU_TESS_ERROR  = 100103;/* void (CALLBACK*)(GLenum  errno) */
		public const uint GLU_TESS_EDGE_FLAG  = 100104;/* void (CALLBACK*)(GLboolean boundaryEdge) */
		public const uint GLU_TESS_COMBINE = 100105;/* void (CALLBACK*)(GLdouble coords[3],
														void *data[4],
														GLfloat  weight[4],
														void **dataOut)*/
		public const uint GLU_TESS_BEGIN_DATA  = 100106;/* void (CALLBACK*)(GLenum  type, 
														void *polygon_data) */
		public const uint GLU_TESS_VERTEX_DATA = 100107;/* void (CALLBACK*)(void *data, 
														void *polygon_data) */
		public const uint GLU_TESS_END_DATA= 100108;/* void (CALLBACK*)(void *polygon_data) */
		public const uint GLU_TESS_ERROR_DATA  = 100109;/* void (CALLBACK*)(GLenum  errno, 
														void *polygon_data) */
		public const uint GLU_TESS_EDGE_FLAG_DATA  = 100110;/* void (CALLBACK*)(GLboolean boundaryEdge,
														void *polygon_data) */
		public const uint GLU_TESS_COMBINE_DATA = 100111;/* void (CALLBACK*)(GLdouble coords[3],
														void *data[4],
														GLfloat  weight[4],
														void **dataOut,
														void *polygon_data) */

		/* TessError */
		public const uint GLU_TESS_ERROR1 = 100151;
		public const uint GLU_TESS_ERROR2 = 100152;
		public const uint GLU_TESS_ERROR3 = 100153;
		public const uint GLU_TESS_ERROR4 = 100154;
		public const uint GLU_TESS_ERROR5 = 100155;
		public const uint GLU_TESS_ERROR6 = 100156;
		public const uint GLU_TESS_ERROR7 = 100157;
		public const uint GLU_TESS_ERROR8 = 100158;

		public const uint GLU_TESS_MISSING_BEGIN_POLYGON = GLU_TESS_ERROR1;
		public const uint GLU_TESS_MISSING_BEGIN_CONTOUR = GLU_TESS_ERROR2;
		public const uint GLU_TESS_MISSING_END_POLYGON  = GLU_TESS_ERROR3;
		public const uint GLU_TESS_MISSING_END_CONTOUR  = GLU_TESS_ERROR4;
		public const uint GLU_TESS_COORD_TOO_LARGE  = GLU_TESS_ERROR5;
		public const uint GLU_TESS_NEED_COMBINE_CALLBACK = GLU_TESS_ERROR6;

		/**** NURBS constants ****/

		/* NurbsProperty */
		public const uint GLU_AUTO_LOAD_MATRIX = 100200;
		public const uint GLU_CULLING = 100201;
		public const uint GLU_SAMPLING_TOLERANCE= 100203;
		public const uint GLU_DISPLAY_MODE = 100204;
		public const uint GLU_PARAMETRIC_TOLERANCE  = 100202;
		public const uint GLU_SAMPLING_METHOD  = 100205;
		public const uint GLU_U_STEP = 100206;
		public const uint GLU_V_STEP = 100207;

		/* NurbsSampling */
		public const uint GLU_PATH_LENGTH = 100215;
		public const uint GLU_PARAMETRIC_ERROR = 100216;
		public const uint GLU_DOMAIN_DISTANCE  = 100217;

		/* NurbsTrim */
		public const uint GLU_MAP1_TRIM_2 = 100210;
		public const uint GLU_MAP1_TRIM_3 = 100211;

		/* NurbsDisplay */
		/* GLU_FILL 100012 */
		public const uint GLU_OUTLINE_POLYGON  = 100240;
		public const uint GLU_OUTLINE_PATCH= 100241;

		/* NurbsCallback */
		/* GLU_ERROR100103 */

		/* NurbsErrors */
		public const uint GLU_NURBS_ERROR1 = 100251;
		public const uint GLU_NURBS_ERROR2 = 100252;
		public const uint GLU_NURBS_ERROR3 = 100253;
		public const uint GLU_NURBS_ERROR4 = 100254;
		public const uint GLU_NURBS_ERROR5 = 100255;
		public const uint GLU_NURBS_ERROR6 = 100256;
		public const uint GLU_NURBS_ERROR7 = 100257;
		public const uint GLU_NURBS_ERROR8 = 100258;
		public const uint GLU_NURBS_ERROR9 = 100259;
		public const uint GLU_NURBS_ERROR10= 100260;
		public const uint GLU_NURBS_ERROR11= 100261;
		public const uint GLU_NURBS_ERROR12= 100262;
		public const uint GLU_NURBS_ERROR13= 100263;
		public const uint GLU_NURBS_ERROR14= 100264;
		public const uint GLU_NURBS_ERROR15= 100265;
		public const uint GLU_NURBS_ERROR16= 100266;
		public const uint GLU_NURBS_ERROR17= 100267;
		public const uint GLU_NURBS_ERROR18= 100268;
		public const uint GLU_NURBS_ERROR19= 100269;
		public const uint GLU_NURBS_ERROR20= 100270;
		public const uint GLU_NURBS_ERROR21= 100271;
		public const uint GLU_NURBS_ERROR22= 100272;
		public const uint GLU_NURBS_ERROR23= 100273;
		public const uint GLU_NURBS_ERROR24= 100274;
		public const uint GLU_NURBS_ERROR25= 100275;
		public const uint GLU_NURBS_ERROR26= 100276;
		public const uint GLU_NURBS_ERROR27= 100277;
		public const uint GLU_NURBS_ERROR28= 100278;
		public const uint GLU_NURBS_ERROR29= 100279;
		public const uint GLU_NURBS_ERROR30= 100280;
		public const uint GLU_NURBS_ERROR31= 100281;
		public const uint GLU_NURBS_ERROR32= 100282;
		public const uint GLU_NURBS_ERROR33= 100283;
		public const uint GLU_NURBS_ERROR34= 100284;
		public const uint GLU_NURBS_ERROR35= 100285;
		public const uint GLU_NURBS_ERROR36= 100286;
		public const uint GLU_NURBS_ERROR37= 100287;

		/**** Backwards compatibility for old tesselator ****/

		/* Contours types -- obsolete! */
		public const uint GLU_CW = 100120;
		public const uint GLU_CCW= 100121;
		public const uint GLU_INTERIOR= 100122;
		public const uint GLU_EXTERIOR= 100123;
		public const uint GLU_UNKNOWN = 100124;

		/* Names without"TESS_"prefix */
		public const uint GLU_BEGIN  = GLU_TESS_BEGIN;
		public const uint GLU_VERTEX = GLU_TESS_VERTEX;
		public const uint GLU_END= GLU_TESS_END;
		public const uint GLU_ERROR  = GLU_TESS_ERROR;
		public const uint GLU_EDGE_FLAG  = GLU_TESS_EDGE_FLAG;


		[DllImport(GLU_DLL, EntryPoint ="gluErrorString")]
		public static extern byte[] gluErrorString ( uint errCode );
		[DllImport(GLU_DLL, EntryPoint ="gluGetString")]
		public static extern byte[] gluGetString  ( uint name );
		[DllImport(GLU_DLL, EntryPoint ="gluOrtho2D")]
		public static extern void  gluOrtho2D( double left, double right, double bottom, double top );
		[DllImport(GLU_DLL, EntryPoint ="gluPerspective")]
		public static extern void  gluPerspective ( double fovy, double aspect, double zNear, double zFar );
		[DllImport(GLU_DLL, EntryPoint ="gluPickMatrix")]
		public static extern void  gluPickMatrix ( double x, double y, double width, double height, int viewport );
		[DllImport(GLU_DLL, EntryPoint ="gluLookAt")]
		public static extern void  gluLookAt ( double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz );
		[DllImport(GLU_DLL, EntryPoint ="gluProject")]
		public static extern int  gluProject( double objx, double objy, double objz, double modelMatrix, double projMatrix, int viewport, ref double winx, ref double winy, ref double winz );
		[DllImport(GLU_DLL, EntryPoint ="gluUnProject")]
		public static extern int  gluUnProject  ( double winx, double winy, double winz, double modelMatrix, double projMatrix, int viewport, ref double objx, ref double objy, ref double objz );
		[DllImport(GLU_DLL, EntryPoint ="gluScaleImage")]
		public static extern int  gluScaleImage ( uint format, int widthin, int heightin, uint typein, object[] datain, int widthout, int heightout, uint typeout, object[] dataout );
		[DllImport(GLU_DLL, EntryPoint ="gluBuild1DMipmaps")]
		public static extern int  gluBuild1DMipmaps  ( uint target, int components, int width, uint format, uint type, object[] data );
		[DllImport(GLU_DLL, EntryPoint ="gluBuild2DMipmaps")]
		public static extern int  gluBuild2DMipmaps  ( uint target, int components, int width, int height, uint format, uint type, object[] data );
		[DllImport(GLU_DLL, EntryPoint ="gluNewQuadric")]
		public static extern object gluNewQuadric ( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteQuadric")]
		public static extern void  gluDeleteQuadric  ( ref object state );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricNormals")]
		public static extern void  gluQuadricNormals  ( ref object quadObject, uint normals );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricTexture")]
		public static extern void  gluQuadricTexture  ( ref object quadObject, byte textureCoords );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricOrientation")]
		public static extern void  gluQuadricOrientation  ( ref object quadObject, uint orientation );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricDrawStyle")]
		public static extern void  gluQuadricDrawStyle ( ref object quadObject, uint drawStyle );
		[DllImport(GLU_DLL, EntryPoint ="gluCylinder")]
		public static extern void  gluCylinder  ( ref object qobj, double baseRadius, double topRadius, double height, int slices, int stacks );
		[DllImport(GLU_DLL, EntryPoint ="gluDisk")]
		public static extern void  gluDisk  ( ref object qobj, double innerRadius, double outerRadius, int slices, int loops );
		[DllImport(GLU_DLL, EntryPoint ="gluPartialDisk")]
		public static extern void  gluPartialDisk ( ref object qobj, double innerRadius, double outerRadius, int slices, int loops, double startAngle, double sweepAngle );
		[DllImport(GLU_DLL, EntryPoint ="gluSphere")]
		public static extern void  gluSphere ( ref object qobj, double radius, int slices, int stacks );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricCallback")]
		public static extern void  gluQuadricCallback ( ref object qobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluNewTess")]
		public static extern IntPtr gluNewTess( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteTess")]
		public static extern void  gluDeleteTess ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessBeginPolygon")]
		public static extern void  gluTessBeginPolygon ( IntPtr tess, object polygon_data );
		[DllImport(GLU_DLL, EntryPoint ="gluTessBeginContour")]
		public static extern void  gluTessBeginContour ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessVertex")]
		public static extern void  gluTessVertex ( IntPtr tess, double coords, object data );
		[DllImport(GLU_DLL, EntryPoint ="gluTessEndContour")]
		public static extern void  gluTessEndContour  ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessEndPolygon")]
		public static extern void  gluTessEndPolygon  ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessProperty")]
		public static extern void  gluTessProperty( IntPtr tess, uint which, double valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluTessNormal")]
		public static extern void  gluTessNormal ( IntPtr tess, double x, double y, double z );
		[DllImport(GLU_DLL, EntryPoint ="gluTessCallback")]
		public static extern void  gluTessCallback( IntPtr tess, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluGetTessProperty")]
		public static extern void  gluGetTessProperty ( IntPtr tess, uint which, ref double valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluNewNurbsRenderer")]
		public static extern IntPtr gluNewNurbsRenderer ( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteNurbsRenderer")]
		public static extern void  gluDeleteNurbsRenderer  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginSurface")]
		public static extern void  gluBeginSurface( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginCurve")]
		public static extern void  gluBeginCurve ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndCurve")]
		public static extern void  gluEndCurve  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndSurface")]
		public static extern void  gluEndSurface ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginTrim")]
		public static extern void  gluBeginTrim  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndTrim")]
		public static extern void  gluEndTrim( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluPwlCurve")]
		public static extern void  gluPwlCurve  ( IntPtr nobj, int count, float[] array, int stride, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsCurve")]
		public static extern void  gluNurbsCurve ( IntPtr nobj, int nknots, float[] knot, int stride, float[] ctlarray, int order, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsSurface")]
		public static extern void  gluNurbsSurface( IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[] tknot, int s_stride, int t_stride, float[] ctlarray, int sorder, int torder, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluLoadSamplingMatrices")]
		public static extern void  gluLoadSamplingMatrices ( IntPtr nobj, float modelMatrix, float projMatrix, int viewport );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsProperty")]
		public static extern void  gluNurbsProperty  ( IntPtr nobj, uint property, float valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluGetNurbsProperty")]
		public static extern void  gluGetNurbsProperty ( IntPtr nobj, uint property, float[] valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsCallback")]
		public static extern void  gluNurbsCallback  ( IntPtr nobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginPolygon")]
		public static extern void  gluBeginPolygon( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluNextContour")]
		public static extern void  gluNextContour ( IntPtr tess, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluEndPolygon")]
		public static extern void  gluEndPolygon ( IntPtr tess );

	}
}
