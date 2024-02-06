///////////////////////////////////////////////////////////////////////  
//  Utility Shaders
//
//  *** INTERACTIVE DATA VISUALIZATION (IDV) PROPRIETARY INFORMATION ***
//
//  This software is supplied under the terms of a license agreement or
//  nondisclosure agreement with Interactive Data Visualization and may
//  not be copied or disclosed except in accordance with the terms of
//  that agreement.
//
//      Copyright (c) 2003-6 IDV, Inc.
//      All Rights Reserved.
//
//      IDV, Inc.
//      Web: http://www.idvinc.com
//
//  *** Release Version 4.0 ***


///////////////////////////////////////////////////////////////////////  
//  Modulate_Float
//
//  Returns x % y (some compilers generate way too many instructions when
//  using the native '%' operator)

float Modulate_Float(float x, float y)
{
    return x - (int(x / y) * y);
}


///////////////////////////////////////////////////////////////////////  
//  WindEffect
//
//  New with 4.0 is a two-weight wind system that allows the tree model
//  to bend at more than one branch level.
//
//  In order to keep the vertex size small, the wind parameters have been
//  compressed as detailed here:
//
//      vWindInfo.x = (wind_matrix_index1 * 10.0) / NUM_WIND_MATRICES  + wind_weight1
//      vWindInfo.y = (wind_matrix_index2 * 10.0) / NUM_WIND_MATRICES  + wind_weight2
//
//  * Note: NUM_WIND_MATRICES cannot be larger than 10 in this case
//
//	* Caution: Negative wind weights will not work with this scheme.  We rely on the
//		       fact that the SpeedTreeRT library clamps wind weights to [0.0, 1.0]

#define TWO_WEIGHT_WIND


float3 WindEffect(float3 vPosition, float2 vWindInfo)
{
    // decode both wind weights and matrix indices at the same time in order to save
    // vertex instructions
    
    //float2 vWeights = frac(vWindInfo.xy);
    //float2 vIndices = float2(2.0f, 2.0f);//vWindInfo;//(vWindInfo - vWeights) - 1.0f;
    vWindInfo.xy += g_fWindMatrixOffset.xx;
    float2 vWeights = frac(vWindInfo.xy);
    float2 vIndices = (vWindInfo - vWeights) * 0.05f * NUM_WIND_MATRICES;
    
    // first-level wind effect - interpolate between static position and fully-blown
    // wind position by the wind weight value
#ifdef IDV_OPENGL
    float3 vWindEffect = lerp(vPosition.xyz, mul(vPosition, float3x3(g_amWindMatrices[int(vIndices.x)])), vWeights.x);
#else
    float3 vWindEffect = lerp(vPosition.xyz, mul(vPosition, g_amWindMatrices[int(vIndices.x)]), vWeights.x);
#endif
    
    // second-level wind effect - interpolate between first-level wind position and 
    // the fully-blown wind position by the second wind weight value
#ifdef TWO_WEIGHT_WIND
	#ifdef IDV_OPENGL
		return lerp(vWindEffect, mul(vWindEffect, float3x3(g_amWindMatrices[int(vIndices.y)])), vWeights.y);
	#else
		return lerp(vWindEffect, mul(vWindEffect, g_amWindMatrices[int(vIndices.y)]), vWeights.y);
	#endif
#else
    return vWindEffect;
#endif
}



///////////////////////////////////////////////////////////////////////  
//  LightDiffuse
//
//  Very simple lighting equation, used by the fronds and leaves (branches
//  and billboards are normal mapped).

float3 LightDiffuse(float3 vVertex,
                    float3 vNormal,
                    float3 vLightDir,
                    float3 vLightColor,
                    float3 vDiffuseMaterial)
{
    return vDiffuseMaterial * vLightColor * max(dot(vNormal, vLightDir), 0.0f);
}


///////////////////////////////////////////////////////////////////////  
//  LightDiffuse_Capped
//
//  Slightly modified version of LightDiffuse, used by the leaf shader in
//  order to cap the dot contribution

float3 LightDiffuse_Capped(float3 vVertex,
                           float3 vNormal,
                           float3 vLightDir,
                           float3 vLightColor,
                           float3 vDiffuseMaterial)
{
    float fDotProduct = max(dot(vNormal, vLightDir), 0.0f);
    fDotProduct = lerp(g_fLeafLightingAdjust, 1.0f, fDotProduct);
    
    return vDiffuseMaterial * vLightColor * fDotProduct;
}


///////////////////////////////////////////////////////////////////////  
//  FogValue
//
//  Simple LINEAR fog computation.  If an exponential equation is desired,
//  it can be placed here - all of the shaders call this one function.

#ifdef USE_FOG
float FogValue(float fPoint)
{
    float fFogEnd = g_vFogParams.y;
    float fFogDist = g_vFogParams.z;
    
    return saturate((fFogEnd - fPoint) / fFogDist);
}
#endif


///////////////////////////////////////////////////////////////////////  
//  RotationMatrix_zAxis
//
//  Constructs a Z-axis rotation matrix

float3x3 RotationMatrix_zAxis(float fAngle)
{
    // compute sin/cos of fAngle
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float3x3(vSinCos.y, -vSinCos.x, 0.0f, 
                    vSinCos.x, vSinCos.y, 0.0f, 
                    0.0f, 0.0f, 1.0f);
}


///////////////////////////////////////////////////////////////////////  
//  Rotate_zAxis
//
//  Returns an updated .xy value

float2 Rotate_zAxis(float fAngle, float3 vPoint)
{
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float2(dot(vSinCos.yx, vPoint.xy), dot(float2(-vSinCos.x, vSinCos.y), vPoint.xy));
}


///////////////////////////////////////////////////////////////////////  
//  RotationMatrix_yAxis
//
//  Constructs a Y-axis rotation matrix

float3x3 RotationMatrix_yAxis(float fAngle)
{
    // compute sin/cos of fAngle
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float3x3(vSinCos.y, 0.0f, vSinCos.x,
                    0.0f, 1.0f, 0.0f,
                    -vSinCos.x, 0.0f, vSinCos.y);
}


///////////////////////////////////////////////////////////////////////  
//  Rotate_yAxis
//
//  Returns an updated .xz value

float2 Rotate_yAxis(float fAngle, float3 vPoint)
{
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float2(dot(float2(vSinCos.y, -vSinCos.x), vPoint.xz), dot(vSinCos.xy, vPoint.xz));
}


///////////////////////////////////////////////////////////////////////  
//  RotationMatrix_xAxis
//
//  Constructs a X-axis rotation matrix

float3x3 RotationMatrix_xAxis(float fAngle)
{
    // compute sin/cos of fAngle
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float3x3(1.0f, 0.0f, 0.0f,
                    0.0f, vSinCos.y, -vSinCos.x,
                    0.0f, vSinCos.x, vSinCos.y);
}


///////////////////////////////////////////////////////////////////////  
//  Rotate_xAxis
//
//  Returns an updated .yz value

float2 Rotate_xAxis(float fAngle, float3 vPoint)
{
    float2 vSinCos;
    sincos(fAngle, vSinCos.x, vSinCos.y);
    
    return float2(dot(vSinCos.yx, vPoint.yz), dot(float2(-vSinCos.x, vSinCos.y), vPoint.yz));
}

void HoffmanScattering( in float4 vecPosition, out float3 vecLin, out float3 vecFex )
{
	float4 vecTransformPosition = vecPosition;
	float3 vecViewDirection = vecTransformPosition - g_vViewPosition;
	float fLens = length( vecViewDirection );
		
	vecViewDirection = normalize( vecViewDirection );
	
	float fTheta = saturate( dot( g_vLightDir, vecViewDirection ) );
	float fSunTheta = pow( fTheta, g_fSunExp );
	
	float3 vecXYDirection = vecViewDirection;	
	vecXYDirection.y = 0.0f;	
	vecXYDirection = normalize( vecXYDirection );		
		
	float3 vecHorizon = pow( saturate( dot( vecXYDirection, vecViewDirection ) ), g_fHorizonExp ) * g_vHorizonColor * g_fMultiplyHorizonColor;
	
	float vSunColor = float3( 0.0f, 0.0f, 0.0f );
	
	float3 vecInscatter = vSunColor * fSunTheta * g_fMultiplySunColor + g_vFogColor;// + vecHorizon;		
	
	float fPhase1 = 1.0f + fTheta * fTheta;		
	
	float fFogRate = saturate( ( fLens - g_vFogParams.x ) / g_vFogParams.z );
	fFogRate *= fPhase1;	
		
	vecInscatter = lerp( float3( 0.0f, 0.0f, 0.0f ), vecInscatter * fPhase1, fFogRate );		
	
	vecLin = vecInscatter;	
		
	float fScatterRate = saturate( fLens / g_fScatterEnd );
	vecFex = lerp( g_vScatterStartColor, g_vScatterEndColor, fScatterRate ) * g_fMultiplyObjectScatter;		
}
