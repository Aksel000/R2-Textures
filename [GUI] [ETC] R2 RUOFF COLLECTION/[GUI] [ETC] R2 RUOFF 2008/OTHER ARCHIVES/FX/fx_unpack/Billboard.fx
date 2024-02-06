///////////////////////////////////////////////////////////////////////  
//  Billboard Shaders
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
//  Billboard-specific global variables

// vs
#if !defined( LOW_COMPILE_TARGET )
float4      g_v360TexCoords[NUM_360_IMAGES] REG(22); // each element defines the texcoords for a single billboard image - each element has:
#endif                                                     //     x = s-coord (rightmost s-coord of billboard)
                                                     //     y = t-coord (topmost t-coord of billboard) 
                                                     //     z = s-axis width (leftmost s-coord = x - z)
                                                     //     w = t-axis height (bottommost t-coord = y - w)
float       g_fSpecialAzimuth;                       // camera azimuth, adjusted to speed billboard vs computations
float4x4    g_amBBNormals;                           // all billboards share the same normals since they all face the camera
float4x4    g_amBBBinormals;                         // all billboards share the same binormals since they all face the camera  
float4x4    g_amBBTangents;                          // all billboards share the same tangents since they all face the camera
float4x4    g_mBBUnitSquare =                        // unit billboard card that's turned towards the camera.  card is aligned on 
            {                                        // YZ plane and centered at (0.0f, 0.0f, 0.5f)
                float4(0.0f, 0.5f, 1.0f, 0.0f), 
                float4(0.0f, -0.5f, 1.0f, 0.0f), 
                float4(0.0f, -0.5f, 0.0f, 0.0f), 
                float4(0.0f, 0.5f, 0.0f, 0.0f) 
            };
#if !defined( LOW_COMPILE_TARGET )            
float4x4    g_afTexCoordScales =                  // used to compress & optimize the g_v360TexCoord lookups (x = s scale, y = t scale)
            { 
                float4(0.0f, 0.0f, 0.0f, 0.0f), 
                float4(1.0f, 0.0f, 0.0f, 0.0f), 
                float4(1.0f, 1.0f, 0.0f, 0.0f), 
                float4(0.0f, 1.0f, 0.0f, 0.0f) 
            };
#endif
// ps
texture     g_tBillboardDiffuseMap;                  // composite billboard diffuse map
texture     g_tBillboardNormalMap;                   // composite billboard normal map

float       g_fBillboardLightAdjust REG(18);         // used to assist in accuracy of normal map lit bb's matching the 3D tree


#if !defined( LOW_COMPILE_TARGET )
///////////////////////////////////////////////////////////////////////  
//  Billboard texture samplers

sampler2D samBillboardDiffuseMap : register(s0) = sampler_state
{
    Texture = <g_tBillboardDiffuseMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#endif
};

#ifdef BILLBOARD_NORMAL_MAPPING
sampler2D samBillboardNormalMap : register(s1) = sampler_state
{
    Texture = <g_tBillboardNormalMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#endif
};
#endif
#endif

///////////////////////////////////////////////////////////////////////  
//  SBillboardOutput structure

struct SBillboardOutput
{
#ifdef LOW_COMPILE_TARGET
    float4  vPosition         : POSITION;
    float2  vDiffuseTexCoords : TEXCOORD0;
    float4  vDiffuse		  : COLOR;
#else
    float4  vPosition         : POSITION;
    float2  vDiffuseTexCoords : TEXCOORD0;
    float2  vNormalTexCoords  : TEXCOORD1;

#ifdef BILLBOARD_NORMAL_MAPPING
    float4  vNormal           : TEXCOORD2;
#else
    float   fNormal           : TEXCOORD2;
#endif
    float3  vLightAdjusts     : TEXCOORD3;
#ifdef USE_FOG
    float   fFog              : COLOR; // using FOG here causes a ps_2_0 compilation failure
#endif
    float3 Fex				  : TEXCOORD4;
    float3 Lin				  : TEXCOORD5;
#endif
};


///////////////////////////////////////////////////////////////////////  
//  Billboard1VS
//
//  In order to ensure smooth LOD transitions, two billboards are rendered
//  per tree instance.  Each billboard represents a partially faded rendering
//  of the two closest billboard images for the current camera azimuth and
//  current tree instance rotation.
//
//  Separate shaders are necessary because since different equations are used
//  to pick the billboard index and fade values for the two bb's.

SBillboardOutput Billboard1VS(float4 vPosition      : POSITION,     // xyz = position, w = corner index
                              float4 vGeom          : TEXCOORD0,    // x = width, y = height, z = tree azimuth, w = lod fade
                              float3 vMiscParams    : TEXCOORD1,    // x = scale, y = texcoord offset, z = num images
                              float3 vLightAdjusts  : TEXCOORD2)    // x = bright side adjustment, y = dark side adjustment, z = ambient scale
{
    //global float4 g_v360TexCoords[NUM_360_IMAGES];
    
    // this will be fed to the frond pixel shader
    SBillboardOutput sOutput;
#if !defined( LOW_COMPILE_TARGET )    
    // define attribute aliases for readability
    float fAzimuth = g_vCameraAngles.x;         // current camera azimuth
    float fPitch = g_vCameraAngles.y;           // current camera pitch
    float c_fTwoPi = 6.28318530f;               // 2 * pi
    int nNumImages = vMiscParams.z;             // # of 360-degree images
    float c_fSliceDiameter = c_fTwoPi / float(nNumImages); // diameter = 360 / g_nNum360Images
    int nTexCoordTableOffset = vMiscParams.y;   // offset into g_v360TexCoords where this instance's texcoords begin
#endif
    float c_fClearAlpha = 255.0f;               // alpha testing, 255 means not visible
    int nCorner = vPosition.w;                  // which card corner this vertex represents [0,3]    
    float c_fLodFade = vGeom.w;                 // computed on CPU - the amount the billboard as a whole is faded from 3D geometry    
    float c_fOpaqueAlpha = 84.0f;               // alpha testing, 84 means fully visible
    float c_fAlphaSpread = 171.0f;              // 171 = 255 - 84
    float c_fTreeScale = vMiscParams.x;         // uniform scale of tree instance
    // there are two azimuth values to consider:
    //    1) fAzimuth: the azimuth of the camera position
    //    2) fAdjustedAzimuth: the azimuth of the camera plus the orientation of the tree the billboard 
    //                         represents (used to determine which bb image to use and its alpha value)
    
#if !defined( LOW_COMPILE_TARGET )
    // modify the adjusted azimuth to appear in range [0,2*pi]
    float fAdjustedAzimuth = g_fSpecialAzimuth + vGeom.z;
    if (fAdjustedAzimuth < 0.0f)
        fAdjustedAzimuth += c_fTwoPi;
    else if (fAdjustedAzimuth > c_fTwoPi)
		fAdjustedAzimuth -= c_fTwoPi;

    // pick the billboard image index and access the extract texcoords from the table
    int nIndex0 = int(fAdjustedAzimuth / c_fSliceDiameter + 1);
    if (nIndex0 > nNumImages - 1)
        nIndex0 = 0;
    float4 vTexCoords = g_v360TexCoords[nIndex0 + nTexCoordTableOffset];
    sOutput.vDiffuseTexCoords.x = vTexCoords.x - vTexCoords.z * g_afTexCoordScales[nCorner].x;
    sOutput.vDiffuseTexCoords.y = vTexCoords.y - vTexCoords.w * g_afTexCoordScales[nCorner].y;
    sOutput.vNormalTexCoords = sOutput.vDiffuseTexCoords;
        
#else
    // determine texcoords based on corner position - while not a straighforward method for determining the texcoords
    // for a specific corner, this one provided a good compromise of speed and space
    sOutput.vDiffuseTexCoords.x = vMiscParams.y;
    sOutput.vDiffuseTexCoords.y = vMiscParams.z;    
#endif    
    // compute the alpha fade value
#ifdef SUPPORT_360_BILLBOARDS
    float fAlpha0 = 1.0f - Modulate_Float(fAdjustedAzimuth, c_fSliceDiameter) / c_fSliceDiameter;
#else
    float fAlpha0 = 0.0f;
#endif
    float fFadePoint = lerp(c_fClearAlpha, c_fOpaqueAlpha, c_fLodFade);
    fAlpha0 = lerp(fFadePoint, c_fClearAlpha, pow(fAlpha0, 1.7));
    
    // each billboard may be faded at a distinct value, but it isn't efficient to change
    // the alpha test value per billboard.  instead we adjust the alpha value of the 
    // billboards's outgoing color to achieve the same effect against a static alpha test 
    // value (c_gOpaqueAlpha).
    fAlpha0 = 1.0f - (fAlpha0 - c_fOpaqueAlpha) / c_fAlphaSpread;

    // multiply by the correct corner
#ifdef UPVECTOR_POS_Y
    float3 vecCorner = g_mBBUnitSquare[nCorner].xzy * vGeom.xyx * c_fTreeScale;
#else
    float3 vecCorner = g_mBBUnitSquare[nCorner].xyz * vGeom.xxy * c_fTreeScale;
#endif

    // apply rotation to scaled corner
#ifdef UPVECTOR_POS_Y
    vecCorner.xz = float2(dot(g_vCameraAzimuthTrig.ywz, vecCorner.xyz), dot(g_vCameraAzimuthTrig.xwy, vecCorner.xyz));
#else
    vecCorner.xy = float2(dot(g_vCameraAzimuthTrig.yxw, vecCorner.xyz), dot(g_vCameraAzimuthTrig.zyw, vecCorner.xyz));
#endif
    vPosition.xyz += vecCorner;
    vPosition.w = 1.0f;
    
#ifdef BILLBOARD_NORMAL_MAPPING
    // setup per-pixel normal mapping  (assumes normalized light direction)

    // 0.5f + 0.5f * expr not used since texcoords aren't clamped to [0,1] range - this approach saves
    // several instructions
    sOutput.vNormal.x = dot(g_vLightDir.xyz, g_amBBTangents[nCorner].xyz);
    sOutput.vNormal.y = dot(g_vLightDir.xyz, g_amBBBinormals[nCorner].xyz);
    sOutput.vNormal.z = dot(g_vLightDir.xyz, g_amBBNormals[nCorner].xyz);
    sOutput.vNormal.w = fAlpha0;
#else
#if !defined( LOW_COMPILE_TARGET )
    sOutput.fNormal = fAlpha0;
#endif    
#endif
    
    // project to the screen
    sOutput.vPosition = mul(g_mModelViewProj, vPosition);

#ifdef LOW_COMPILE_TARGET
	sOutput.vDiffuse = float4( vLightAdjusts.z, vLightAdjusts.z, vLightAdjusts.z, fAlpha0 ) * g_vMaterialAmbient;
	sOutput.vDiffuse.xyz += 0.3f;
#else
    // pass through light adjustments
    sOutput.vLightAdjusts = vLightAdjusts;

#ifdef USE_FOG      
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    sOutput.fFog = FogValue(sOutput.vPosition.z);
#else
	sOutput.fFog = 1;    
#endif
	float3 vecFex, vecLin;
	
	HoffmanScattering( vPosition, vecLin, vecFex );        
	
	sOutput.Fex.xyz = vecFex;	
	sOutput.Lin = vecLin * 0.4f;
#endif
    return sOutput;
}


///////////////////////////////////////////////////////////////////////  
//  Billboard2VS
//
//  In order to ensure smooth LOD transitions, two billboards are rendered
//  per tree instance.  Each billboard represents a partially faded rendering
//  of the two closest billboard images for the current camera azimuth and
//  current tree instance rotation.
//
//  Separate shaders are necessary because since different equations are used
//  to pick the billboard index and fade values for the two bb's.

SBillboardOutput Billboard2VS(float4 vPosition      : POSITION,     // xyz = position, w = corner index
                              float4 vGeom          : TEXCOORD0,    // x = width, y = height, z = tree azimuth, w = lod fade
                              float3 vMiscParams    : TEXCOORD1,    // x = scale, y = texcoord offset, z = num images
                              float3 vLightAdjusts  : TEXCOORD2)    // x = bright side adjustment, y = dark side adjustment, z = ambient scale
{
    // this will be fed to the frond pixel shader
    SBillboardOutput sOutput;
    
#if !defined( LOW_COMPILE_TARGET )    
    // define attribute aliases for readability
    float fAzimuth = g_vCameraAngles.x;         // current camera azimuth
    float fPitch = g_vCameraAngles.y;           // current camera pitch
    float c_fTwoPi = 6.28318530f;               // 2 * pi
    int nNumImages = vMiscParams.z;             // # of 360-degree images
    float c_fSliceDiameter = c_fTwoPi / float(nNumImages); // diameter = 360 / g_nNum360Images
    int nTexCoordTableOffset = vMiscParams.y;   // offset into g_v360TexCoords where this instance's texcoords begin
#endif    
    float c_fClearAlpha = 255.0f;               // alpha testing, 255 means not visible    
    int nCorner = vPosition.w;                  // which card corner this vertex represents [0,3]    
    float c_fLodFade = vGeom.w;                 // computed on CPU - the amount the billboard as a whole is faded from 3D geometry
    float c_fOpaqueAlpha = 84.0f;               // alpha testing, 84 means fully visible
    float c_fAlphaSpread = 171.0f;              // 171 = 255 - 84    
    float c_fTreeScale = vMiscParams.x;         // uniform scale of tree instance
    // there are two azimuth values to consider:
    //    1) fAzimuth: the azimuth of the camera position
    //    2) fAdjustedAzimuth: the azimuth of the camera plus the orientation of the tree the billboard 
    //                         represents (used to determine which bb image to use and its alpha value)

    // modify the adjusted azimuth to appear in range [0,2*pi]
#if !defined( LOW_COMPILE_TARGET )    
    float fAdjustedAzimuth = g_fSpecialAzimuth + vGeom.z;
    if (fAdjustedAzimuth < 0.0f)
        fAdjustedAzimuth += c_fTwoPi;
    else if (fAdjustedAzimuth > c_fTwoPi)
		fAdjustedAzimuth -= c_fTwoPi;
    
    // pick the index and access the texcoords
    int nIndex1 = int(fAdjustedAzimuth / c_fSliceDiameter);
    if (nIndex1 > nNumImages - 1)
        nIndex1 = 0;
        
    // determine texcoords based on corner position - while not a straighforward method for determining the texcoords
    // for a specific corner, this one provided a good compromise of speed and space
    float4 vTexCoords = g_v360TexCoords[nIndex1 + nTexCoordTableOffset];
    sOutput.vDiffuseTexCoords.x = vTexCoords.x - vTexCoords.z * g_afTexCoordScales[nCorner].x;
    sOutput.vDiffuseTexCoords.y = vTexCoords.y - vTexCoords.w * g_afTexCoordScales[nCorner].y;
    sOutput.vNormalTexCoords = sOutput.vDiffuseTexCoords;
#else
    sOutput.vDiffuseTexCoords.x = vMiscParams.y;
    sOutput.vDiffuseTexCoords.y = vMiscParams.z;    
#endif
 
    // compute the lpha fade value
#ifdef SUPPORT_360_BILLBOARDS
    float fAlpha1 = (fAdjustedAzimuth - (nIndex1 * c_fSliceDiameter)) / c_fSliceDiameter;
#else
    float fAlpha1 = 0.0f;
#endif
    float fFadePoint = lerp(c_fClearAlpha, c_fOpaqueAlpha, c_fLodFade);
    fAlpha1 = lerp(fFadePoint, c_fClearAlpha, pow(fAlpha1, 1.7));
    
    // each billboard may be faded at a distinct value, but it isn't efficient to change
    // the alpha test value per billboard.  instead we adjust the alpha value of the 
    // billboards's outgoing color to achieve the same effect against a static alpha test 
    // value (c_gOpaqueAlpha).
    fAlpha1 = 1.0f - (fAlpha1 - c_fOpaqueAlpha) / c_fAlphaSpread;

    // multiply by the correct corner
#ifdef UPVECTOR_POS_Y
    float3 vecCorner = g_mBBUnitSquare[nCorner].xzy * vGeom.xyx * c_fTreeScale;
#else
    float3 vecCorner = g_mBBUnitSquare[nCorner].xyz * vGeom.xxy * c_fTreeScale;
#endif

    // apply rotation to scaled corner
#ifdef UPVECTOR_POS_Y
    vecCorner.xz = float2(dot(g_vCameraAzimuthTrig.ywz, vecCorner.xyz), dot(g_vCameraAzimuthTrig.xwy, vecCorner.xyz));
#else
    vecCorner.xy = float2(dot(g_vCameraAzimuthTrig.yxw, vecCorner.xyz), dot(g_vCameraAzimuthTrig.zyw, vecCorner.xyz));
#endif
    vPosition.xyz += vecCorner;
    vPosition.w = 1.0f;
    
#ifdef BILLBOARD_NORMAL_MAPPING
    // setup per-pixel normal mapping  (assumes normalized light direction)

    // 0.5f + 0.5f * expr not used since texcoords aren't clamped to [0,1] range - this approach saves
    // several instructions
    sOutput.vNormal.x = dot(g_vLightDir.xyz, g_amBBTangents[nCorner].xyz);
    sOutput.vNormal.y = dot(g_vLightDir.xyz, g_amBBBinormals[nCorner].xyz);
    sOutput.vNormal.z = dot(g_vLightDir.xyz, g_amBBNormals[nCorner].xyz);
    sOutput.vNormal.w = fAlpha1;
#else
#if !defined( LOW_COMPILE_TARGET )
    sOutput.fNormal = fAlpha1;
#endif    
#endif

    // project to the screen
    sOutput.vPosition = mul(g_mModelViewProj, vPosition);

#ifdef LOW_COMPILE_TARGET
	sOutput.vDiffuse = float4( vLightAdjusts.z, vLightAdjusts.z, vLightAdjusts.z, fAlpha1 );
#else
    // pass through light adjustments
    sOutput.vLightAdjusts = vLightAdjusts;

#ifdef USE_FOG      
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    sOutput.fFog = FogValue(sOutput.vPosition.z);
#else
	sOutput.fFog = 1;    
   
#endif
	float3 vecFex, vecLin;
	
	HoffmanScattering( vPosition, vecLin, vecFex );        
	
	sOutput.Fex.xyz = vecFex;	
	sOutput.Lin = vecLin * 0.4f;
#endif
    return sOutput;
}


///////////////////////////////////////////////////////////////////////  
//  BillboardPS
//
//  Billboard geometry pixel shader
//
//  Not having BILLBOARD_NORMAL_MAPPING #define'd makes for a much shorter
//  pixel shader - one that should be used on lower compile targets

float4 BillboardPS(SBillboardOutput In) : COLOR
{
#if !defined( LOW_COMPILE_TARGET )    
#ifndef BILLBOARD_NORMAL_MAPPING
    float4 texDiffuse = tex2D(samBillboardDiffuseMap, In.vDiffuseTexCoords.xy);
    texDiffuse.a *= In.fNormal;

#ifdef USE_FOG
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    texDiffuse.xyz = lerp(g_vFogColor.xyz, texDiffuse.xyz, In.fFog);
#endif
    
    return texDiffuse;
    
#else // BILLBOARD_NORMAL_MAPPING

    // look up the diffuse and normal-map layers
    float4 texDiffuse = tex2D(samBillboardDiffuseMap, In.vDiffuseTexCoords.xy);
    float4 texNormal = tex2D(samBillboardNormalMap, In.vNormalTexCoords.xy);

    // compute the ambient value
    const float4 vecNoiseOffset = { -0.32f, -0.32f, -0.32f, 0.0f };
    float4 vecAmbient = (texNormal.aaaa + vecNoiseOffset) * In.vLightAdjusts.z;
    vecAmbient = vecAmbient * texDiffuse;
    
    // since the normal map normal values (normally ranged [-1,1]) are store
    // as a color value (ranged [0,1]), they must be uncompressed.  a dot product 
    // is used to compute the diffuse lighting contribution.
    const float3 vecHalves = { 0.5f, 0.5f, 0.5f };
    float fDot = saturate(dot(texNormal.rgb - vecHalves, In.vNormal.rgb)) * 2.0f;

    // lighting adjustment to use SpeedTreeCAD parameters to help make the bb
    // match the 3D geometry
    float fLightAdjust = lerp(In.vLightAdjusts.x, In.vLightAdjusts.y, g_fBillboardLightAdjust );
	float4 vColor = (fDot * texDiffuse * fLightAdjust) + vecAmbient;

    // use an alpha value that is the diffuse map's noisy alpha channel multiplied
    // against the faded alpha color from the vertex shader
    vColor.a = texDiffuse.a * In.vNormal.w;
#if !defined( LOW_COMPILE_TARGET )    
	vColor.xyz = vColor.xyz * In.Fex + In.Lin;
#endif
	
#ifdef USE_FOG
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    vColor.xyz = lerp(g_vFogColor.xyz, vColor.xyz, In.fFog);
   
#endif

    return vColor;
#endif // BILLBOARD_NORMAL_MAPPING
#endif
}


///////////////////////////////////////////////////////////////////////  
//  Techniques

technique Billboards1
{
    pass P0
    {          
        VS_COMPILE_COMMAND Billboard1VS( );
#if !defined( LOW_COMPILE_TARGET )
        PS_COMPILE_COMMAND BillboardPS( );
#endif        
    }
}

technique Billboards2
{
    pass P0
    {          
        VS_COMPILE_COMMAND Billboard2VS( );
#if !defined( LOW_COMPILE_TARGET )
        PS_COMPILE_COMMAND BillboardPS( );
#endif        
    }
}

