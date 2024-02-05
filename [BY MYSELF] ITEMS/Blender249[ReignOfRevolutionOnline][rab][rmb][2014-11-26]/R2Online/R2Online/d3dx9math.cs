using System;
using System.Runtime.InteropServices;

namespace DirectX9 {
	namespace D3DX9 {

		public class D3DX9MATH {

			#region Float16
			[DllImport("d3dx9_35.dll")]
			public static extern void D3DXFloat16To32Array([In, Out] ref float pOut, [In] ref  D3DXFLOAT16 pIn, [In] uint n);
			[DllImport("d3dx9_35.dll")]
			public static extern void D3DXFloat32To16Array([In, Out] ref D3DXFLOAT16 pOut, [In] ref float pIn, [In] uint n);
			#endregion

			#region Quaternion
			[DllImport("d3dx9_35.dll")]
			public static extern void D3DXQuaternionMultiply([In, Out] ref  D3DXQUATERNION pOut, [In] ref D3DXQUATERNION pQ1, [In] ref D3DXQUATERNION pQ2);
			#endregion

			#region 4D Matrix
			[DllImport("d3dx9_35.dll")]
			public static extern void D3DXMatrixMultiply([In, Out] ref D3DXMATRIX pOut, [In] ref  D3DXMATRIX pM1, [In] ref  D3DXMATRIX pM2);
			#endregion

			#region Inline functions

			#region 2D Vector
			public static float D3DXVec2Length(D3DXVECTOR2 pV) {
				return (float)Math.Sqrt(pV.x * pV.x + pV.y * pV.y);
			}
			public static float D3DXVec2LengthSq(D3DXVECTOR2 pV) {
				return pV.x * pV.x + pV.y * pV.y;
			}
			public static float D3DXVec2Dot(D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				return pV1.x * pV2.x + pV1.y * pV2.y;
			}
			public static float D3DXVec2CCW(D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				return pV1.x * pV2.y - pV1.y * pV2.x;
			}
			public static D3DXVECTOR2 D3DXVec2Add(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				pOut.x = pV1.x + pV2.x;
				pOut.y = pV1.y + pV2.y;
				return pOut;
			}
			public static D3DXVECTOR2 D3DXVec2Subtract(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				pOut.x = pV1.x - pV2.x;
				pOut.y = pV1.y - pV2.y;
				return pOut;
			}
			public static D3DXVECTOR2 D3DXVec2Minimize(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				pOut.x = pV1.x < pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y < pV2.y ? pV1.y : pV2.y;
				return pOut;
			}
			public static D3DXVECTOR2 D3DXVec2Maximize(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV1, D3DXVECTOR2 pV2) {
				pOut.x = pV1.x > pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y > pV2.y ? pV1.y : pV2.y;
				return pOut;
			}
			public static D3DXVECTOR2 D3DXVec2Scale(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV, [In] float s) {
				pOut.x = pV.x * s;
				pOut.y = pV.y * s;
				return pOut;
			}
			public static D3DXVECTOR2 D3DXVec2Lerp(ref D3DXVECTOR2 pOut, D3DXVECTOR2 pV1, D3DXVECTOR2 pV2, [In] float s) {
				pOut.x = pV1.x + s * (pV2.x - pV1.x);
				pOut.y = pV1.y + s * (pV2.y - pV1.y);
				return pOut;
			}
			#endregion

			#region 3D Vector
			public static float D3DXVec3Length(D3DXVECTOR3 pV) {
				return (float)Math.Sqrt(pV.x * pV.x + pV.y * pV.y + pV.z * pV.z);
			}
			public static float D3DXVec3LengthSq(D3DXVECTOR3 pV) {
				return pV.x * pV.x + pV.y * pV.y + pV.z * pV.z;
			}
			public static float D3DXVec3Dot(D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				return pV1.x * pV2.x + pV1.y * pV2.y + pV1.z * pV2.z;
			}
			public static D3DXVECTOR3 D3DXVec3Cross(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				D3DXVECTOR3 v;

				v.x = pV1.y * pV2.z - pV1.z * pV2.y;
				v.y = pV1.z * pV2.x - pV1.x * pV2.z;
				v.z = pV1.x * pV2.y - pV1.y * pV2.x;

				pOut = v;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Add(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				pOut.x = pV1.x + pV2.x;
				pOut.y = pV1.y + pV2.y;
				pOut.z = pV1.z + pV2.z;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Subtract(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				pOut.x = pV1.x - pV2.x;
				pOut.y = pV1.y - pV2.y;
				pOut.z = pV1.z - pV2.z;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Minimize(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				pOut.x = pV1.x < pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y < pV2.y ? pV1.y : pV2.y;
				pOut.z = pV1.z < pV2.z ? pV1.z : pV2.z;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Maximize(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2) {
				pOut.x = pV1.x > pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y > pV2.y ? pV1.y : pV2.y;
				pOut.z = pV1.z > pV2.z ? pV1.z : pV2.z;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Scale(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV, [In] float s) {
				pOut.x = pV.x * s;
				pOut.y = pV.y * s;
				pOut.z = pV.z * s;
				return pOut;
			}
			public static D3DXVECTOR3 D3DXVec3Lerp(ref D3DXVECTOR3 pOut, D3DXVECTOR3 pV1, D3DXVECTOR3 pV2, [In] float s) {
				pOut.x = pV1.x + s * (pV2.x - pV1.x);
				pOut.y = pV1.y + s * (pV2.y - pV1.y);
				pOut.z = pV1.z + s * (pV2.z - pV1.z);
				return pOut;
			}
			//D3DXVec3Normalize
			[DllImport("d3dx9_35.dll")]
			public static extern IntPtr D3DXVec3Normalize([In, Out] ref D3DXVECTOR3 pOut, ref  D3DXVECTOR3 pV);
			[DllImport("d3dx9_35.dll")]
			public static extern IntPtr D3DXVec3Normalize([In, Out] ref float pOut, ref  D3DXVECTOR3 pV);

			#endregion

			#region 4D Vector
			public static float D3DXVec4Length(D3DXVECTOR4 pV) {
				return (float)Math.Sqrt(pV.x * pV.x + pV.y * pV.y + pV.z * pV.z + pV.w * pV.w);
			}
			public static float D3DXVec4LengthSq(D3DXVECTOR4 pV) {
				return pV.x * pV.x + pV.y * pV.y + pV.z * pV.z + pV.w * pV.w;
			}
			public static float D3DXVec4Dot(D3DXVECTOR4 pV1, D3DXVECTOR4 pV2) {
				return pV1.x * pV2.x + pV1.y * pV2.y + pV1.z * pV2.z + pV1.w * pV2.w;
			}
			public static D3DXVECTOR4 D3DXVec4Add(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV1, D3DXVECTOR4 pV2) {
				pOut.x = pV1.x + pV2.x;
				pOut.y = pV1.y + pV2.y;
				pOut.z = pV1.z + pV2.z;
				pOut.w = pV1.w + pV2.w;
				return pOut;
			}
			public static D3DXVECTOR4 D3DXVec4Subtract(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV1, D3DXVECTOR4 pV2) {
				pOut.x = pV1.x - pV2.x;
				pOut.y = pV1.y - pV2.y;
				pOut.z = pV1.z - pV2.z;
				pOut.w = pV1.w - pV2.w;
				return pOut;
			}
			public static D3DXVECTOR4 D3DXVec4Minimize(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV1, D3DXVECTOR4 pV2) {
				pOut.x = pV1.x < pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y < pV2.y ? pV1.y : pV2.y;
				pOut.z = pV1.z < pV2.z ? pV1.z : pV2.z;
				pOut.w = pV1.w < pV2.w ? pV1.w : pV2.w;
				return pOut;
			}
			public static D3DXVECTOR4 D3DXVec4Maximize(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV1, D3DXVECTOR4 pV2) {
				pOut.x = pV1.x > pV2.x ? pV1.x : pV2.x;
				pOut.y = pV1.y > pV2.y ? pV1.y : pV2.y;
				pOut.z = pV1.z > pV2.z ? pV1.z : pV2.z;
				pOut.w = pV1.w > pV2.w ? pV1.w : pV2.w;
				return pOut;
			}
			public static D3DXVECTOR4 D3DXVec4Scale(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV, [In] float s) {
				pOut.x = pV.x * s;
				pOut.y = pV.y * s;
				pOut.z = pV.z * s;
				pOut.w = pV.w * s;
				return pOut;
			}
			public static D3DXVECTOR4 D3DXVec4Lerp(ref D3DXVECTOR4 pOut, D3DXVECTOR4 pV1, D3DXVECTOR4 pV2, [In] float s) {
				pOut.x = pV1.x + s * (pV2.x - pV1.x);
				pOut.y = pV1.y + s * (pV2.y - pV1.y);
				pOut.z = pV1.z + s * (pV2.z - pV1.z);
				pOut.w = pV1.w + s * (pV2.w - pV1.w);
				return pOut;
			}
			#endregion

			#region 4D Matrix
			public static D3DXMATRIX D3DXMatrixIdentity(ref D3DXMATRIX pOut) {
				pOut[0, 1] = pOut[0, 2] = pOut[0, 3] =
				pOut[1, 0] = pOut[1, 2] = pOut[1, 3] =
				pOut[2, 0] = pOut[2, 1] = pOut[2, 3] =
				pOut[3, 0] = pOut[3, 1] = pOut[3, 2] = 0.0f;

				pOut[0, 0] = pOut[1, 1] = pOut[2, 2] = pOut[3, 3] = 1.0f;
				return pOut;
			}
			public static bool D3DXMatrixIsIdentity(D3DXMATRIX pM) {
				return pM[0, 0] == 1.0f && pM[0, 1] == 0.0f && pM[0, 2] == 0.0f && pM[0, 3] == 0.0f &&
					   pM[1, 0] == 0.0f && pM[1, 1] == 1.0f && pM[1, 2] == 0.0f && pM[1, 3] == 0.0f &&
					   pM[2, 0] == 0.0f && pM[2, 1] == 0.0f && pM[2, 2] == 1.0f && pM[2, 3] == 0.0f &&
					   pM[3, 0] == 0.0f && pM[3, 1] == 0.0f && pM[3, 2] == 0.0f && pM[3, 3] == 1.0f;
			}
			[DllImport("d3dx9_35.dll")]
			public static extern IntPtr D3DXMatrixLookAtLH([In, Out] ref D3DXMATRIX pOut, [In] ref D3DXVECTOR3 pEye, [In] ref D3DXVECTOR3 pAt, [In] ref D3DXVECTOR3 pUp);
			[DllImport("d3dx9_35.dll")]
			public static extern IntPtr D3DXMatrixPerspectiveFovLH([In, Out] ref D3DXMATRIX pOut, [In]float fovy, [In]float Aspect, [In]float zn, [In]float zf);
			#endregion

			#region Quaternion
			public static float D3DXQuaternionLength(D3DXQUATERNION pQ) {
				return (float)Math.Sqrt(pQ.x * pQ.x + pQ.y * pQ.y + pQ.z * pQ.z + pQ.w * pQ.w);
			}
			public static float D3DXQuaternionLengthSq(D3DXQUATERNION pQ) {
				return pQ.x * pQ.x + pQ.y * pQ.y + pQ.z * pQ.z + pQ.w * pQ.w;
			}
			public static float D3DXQuaternionDot(D3DXQUATERNION pQ1, D3DXQUATERNION pQ2) {
				return pQ1.x * pQ2.x + pQ1.y * pQ2.y + pQ1.z * pQ2.z + pQ1.w * pQ2.w;
			}
			public static D3DXQUATERNION D3DXQuaternionIdentity(ref D3DXQUATERNION pOut) {
				pOut.x = pOut.y = pOut.z = 0.0f;
				pOut.w = 1.0f;
				return pOut;
			}
			public static bool D3DXQuaternionIsIdentity(D3DXQUATERNION pQ) {
				return pQ.x == 0.0f && pQ.y == 0.0f && pQ.z == 0.0f && pQ.w == 1.0f;
			}
			public static D3DXQUATERNION D3DXQuaternionConjugate(ref D3DXQUATERNION pOut, D3DXQUATERNION pQ) {
				pOut.x = -pQ.x;
				pOut.y = -pQ.y;
				pOut.z = -pQ.z;
				pOut.w = pQ.w;
				return pOut;
			}
			#endregion

			#region Plane
			public static float D3DXPlaneDot(D3DXPLANE pP, D3DXVECTOR4 pV) {
				return pP.a * pV.x + pP.b * pV.y + pP.c * pV.z + pP.d * pV.w;
			}
			public static float D3DXPlaneDotCoord(D3DXPLANE pP, D3DXVECTOR3 pV) {
				return pP.a * pV.x + pP.b * pV.y + pP.c * pV.z + pP.d;
			}
			public static float D3DXPlaneDotNormal(D3DXPLANE pP, D3DXVECTOR3 pV) {
				return pP.a * pV.x + pP.b * pV.y + pP.c * pV.z;
			}
			public static D3DXPLANE D3DXPlaneScale(out D3DXPLANE pOut, D3DXPLANE pP, float s) {
				pOut.a = pP.a * s;
				pOut.b = pP.b * s;
				pOut.c = pP.c * s;
				pOut.d = pP.d * s;
				return pOut;
			}
			#endregion

			#region Color
			public static D3DXCOLOR D3DXColorNegative(ref D3DXCOLOR pOut, D3DXCOLOR pC) {
				pOut.r = 1.0f - pC.r;
				pOut.g = 1.0f - pC.g;
				pOut.b = 1.0f - pC.b;
				pOut.a = pC.a;
				return pOut;
			}
			public static D3DXCOLOR D3DXColorAdd(ref D3DXCOLOR pOut, D3DXCOLOR pC1, D3DXCOLOR pC2) {
				pOut.r = pC1.r + pC2.r;
				pOut.g = pC1.g + pC2.g;
				pOut.b = pC1.b + pC2.b;
				pOut.a = pC1.a + pC2.a;
				return pOut;
			}
			public static D3DXCOLOR D3DXColorSubtract(ref D3DXCOLOR pOut, D3DXCOLOR pC1, D3DXCOLOR pC2) {
				pOut.r = pC1.r - pC2.r;
				pOut.g = pC1.g - pC2.g;
				pOut.b = pC1.b - pC2.b;
				pOut.a = pC1.a - pC2.a;
				return pOut;
			}
			public static D3DXCOLOR D3DXColorScale(ref D3DXCOLOR pOut, D3DXCOLOR pC, [In] float s) {
				pOut.r = pC.r * s;
				pOut.g = pC.g * s;
				pOut.b = pC.b * s;
				pOut.a = pC.a * s;
				return pOut;
			}
			public static D3DXCOLOR D3DXColorModulate(ref D3DXCOLOR pOut, D3DXCOLOR pC1, D3DXCOLOR pC2) {
				pOut.r = pC1.r * pC2.r;
				pOut.g = pC1.g * pC2.g;
				pOut.b = pC1.b * pC2.b;
				pOut.a = pC1.a * pC2.a;
				return pOut;
			}
			public static D3DXCOLOR D3DXColorLerp(ref D3DXCOLOR pOut, D3DXCOLOR pC1, D3DXCOLOR pC2, [In] float s) {
				pOut.r = pC1.r + s * (pC2.r - pC1.r);
				pOut.g = pC1.g + s * (pC2.g - pC1.g);
				pOut.b = pC1.b + s * (pC2.b - pC1.b);
				pOut.a = pC1.a + s * (pC2.a - pC1.a);
				return pOut;
			}
			#endregion

			#endregion

			#region General purpose utilities
			public const float D3DX_PI = ((float)3.141592654f);
			public const float D3DX_1BYPI = ((float)0.318309886f);

			public static float D3DXToRadian([In] float fDegree) {
				return ((fDegree) * (D3DX_PI / 180.0f));
			}
			public static float D3DXToDegree([In] float fRadian) {
				return ((fRadian) * (180.0f / D3DX_PI));
			}
			#endregion

			#region 16 bit floating point numbers
			public const float D3DX_16F_DIG = 3;  // # of decimal digits of precision
			public const float D3DX_16F_EPSILON = 4.8875809e-4f;  // smallest such that 1.0 + epsilon != 1.0
			public const float D3DX_16F_MANT_DIG = 11;  // # of bits in mantissa
			public const float D3DX_16F_MAX = 6.550400e+004f;   // max value
			public const float D3DX_16F_MAX_10_EXP = 4;  // max decimal exponent
			public const float D3DX_16F_MAX_EXP = 15;   // max binary exponent
			public const float D3DX_16F_MIN = 6.1035156e-5f;    // min positive value
			public const float D3DX_16F_MIN_10_EXP = (-4);           // min decimal exponent
			public const float D3DX_16F_MIN_EXP = (-14);            // min binary exponent
			public const float D3DX_16F_RADIX = 2;               // exponent radix
			public const float D3DX_16F_ROUNDS = 1;                // addition rounding: near
			#endregion

			#region Basic Spherical Harmonic lighting routines
			[DllImport("d3dx9_35.dll")]
			public static extern uint D3DXSHEvalDirectionalLight([In] uint Order, [In] ref D3DXVECTOR3 pDir, [In] float RIntensity, [In] float GIntensity, [In] float BIntensity, ref  float pROut, ref  float pGOut, ref  float pBOut);
			[DllImport("d3dx9_35.dll")]
			public static extern uint D3DXSHEvalSphericalLight([In] uint Order, [In] ref D3DXVECTOR3 pPos, [In] float Radius, [In] float RIntensity, [In] float GIntensity, [In] float BIntensity, ref  float pROut, ref  float pGOut, ref  float pBOut);
			[DllImport("d3dx9_35.dll")]
			public static extern uint D3DXSHEvalConeLight([In] uint Order, [In] ref D3DXVECTOR3 pDir, [In] float Radius, [In] float RIntensity, [In] float GIntensity, [In] float BIntensity, ref  float pROut, ref  float pGOut, ref float pBOut);
			[DllImport("d3dx9_35.dll")]
			public static extern uint D3DXSHEvalHemisphereLight([In] uint Order, [In] ref D3DXVECTOR3 pDir, D3DXCOLOR Top, D3DXCOLOR Bottom, ref  float pROut, ref  float pGOut, ref  float pBOut);
			#endregion

			#region Basic Spherical Harmonic math routines
			public const uint D3DXSH_MINORDER = 2;
			public const uint D3DXSH_MAXORDER = 6;
			[DllImport("d3dx9_35.dll", EntryPoint = "D3DXSHEvalDirection")]
			private static extern IntPtr _D3DXSHEvalDirection([In, Out] ref  float pOut, [In] uint Order, [In] ref D3DXVECTOR3 pDir);
			public static float[] D3DXSHEvalDirection(ref float[] pOut, uint Order, D3DXVECTOR3 pDir) {
				IntPtr ptr = _D3DXSHEvalDirection(ref pOut[0], Order, ref pDir);
				float[] f = new float[Order * Order];
				Marshal.Copy(ptr, f, 0, f.Length);
				//Marshal.FreeCoTaskMem(ptr);
				return f;
			}
			/*
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHRotate
    ( FLOAT *pOut, UINT Order, CONST D3DXMATRIX *pMatrix, CONST FLOAT *pIn );
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHRotateZ
    ( FLOAT *pOut, UINT Order, FLOAT Angle, CONST FLOAT *pIn );
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHAdd
    ( FLOAT *pOut, UINT Order, CONST FLOAT *pA, CONST FLOAT *pB );
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHScale
    ( FLOAT *pOut, UINT Order, CONST FLOAT *pIn, CONST FLOAT Scale );
			[DllImport("d3dx9_35.dll")]
FLOAT WINAPI D3DXSHDot
    ( UINT Order, CONST FLOAT *pA, CONST FLOAT *pB );
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHMultiply2( FLOAT *pOut, CONST FLOAT *pF, CONST FLOAT *pG);
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHMultiply3( FLOAT *pOut, CONST FLOAT *pF, CONST FLOAT *pG);
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHMultiply4( FLOAT *pOut, CONST FLOAT *pF, CONST FLOAT *pG);
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHMultiply5( FLOAT *pOut, CONST FLOAT *pF, CONST FLOAT *pG);
			[DllImport("d3dx9_35.dll")]
FLOAT* WINAPI D3DXSHMultiply6( FLOAT *pOut, CONST FLOAT *pF, CONST FLOAT *pG);
 
			*/
			#endregion

		}
	}
}
