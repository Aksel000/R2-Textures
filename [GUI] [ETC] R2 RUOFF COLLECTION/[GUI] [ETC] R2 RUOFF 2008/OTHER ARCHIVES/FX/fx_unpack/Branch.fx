///////////////////////////////////////////////////////////////////////  
//  Branch Shaders
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
//  Branch-specific global variables

// vs (none)

// ps
texture     g_tBranchDiffuseMap;            // bark image, .rgb = diffuse color, .a = alpha noise map
texture     g_tBranchNormalMap;             // bark normal map, .rgb = xyz normals, .a = unused
#ifdef BRANCH_DETAIL_LAYER
texture     g_tBranchDetailMap;             // bark detail image, .rgb = color, .a = amount of detail to use
#endif


///////////////////////////////////////////////////////////////////////  
//  Texture samplers
//
//  These are specific to the branch pixel shader.  The self-shadow layer
//  is assumed to be bound to s1.

sampler2D samBranchDiffuseMap : register(s0) = sampler_state
{
    Texture = <g_tBranchDiffuseMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#endif
};

sampler2D samBranchNormalMap : register(s2) = sampler_state
{
    Texture = <g_tBranchNormalMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#endif
};

#ifdef BRANCH_DETAIL_LAYER
sampler2D samBranchDetailMap : register(s3) = sampler_state
{
    Texture = <g_tBranchDetailMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#endif
};
#endif


///////////////////////////////////////////////////////////////////////  
//  Branch VS Ouput, PS Input
//
//  Cannot use more than four texture layers in order to compile down to ps_1_1

struct SBranchOutput
{
    float4  vPosition             : POSITION;
    float2  vDiffuseTexCoords     : TEXCOORD0;
#ifdef BRANCH_NORMAL_MAPPING
    float2  vNormalTexCoords      : TEXCOORD2;
    float4  vNormal               : TEXCOORD4;
#else
    float4  vNormal               : COLOR;
#endif
#ifdef SELF_SHADOW_LAYER
    float2  vSelfShadowTexCoords  : TEXCOORD1;
#endif
#ifdef BRANCH_DETAIL_LAYER
    float2  vDetailTexCoords      : TEXCOORD3;
#endif
#if !defined( LOW_COMPILE_TARGET )
    float3 Fex					  : TEXCOORD5;
    float3 Lin					  : TEXCOORD6;
#endif    
};


///////////////////////////////////////////////////////////////////////  
//  BranchVS
//
//  Branch geometry vertex shader

SBranchOutput BranchVS(float4   vPosition   : POSITION,     // xyz = coords, w = self-shadow texcoord s
                       float4   vNormal     : TEXCOORD0,    // xyz = normal, w = self-shadow texcoord t
                       float4   vTexCoords1 : TEXCOORD1,    // xy = diffuse texcoords, zw = compressed wind parameters
                       float4   vTexCoords2 : TEXCOORD2,    // xy = detail texcoords, zw = normal-map texcoords
                       float3   vTexCoords3 : TEXCOORD3)    // xyz = tangent (binormal is derived from normal and tangent)
{
    // this will be fed to the branch pixel shader
    SBranchOutput sOutput;

    // define attribute aliases for readability
    float2 vShadowTexCoords = float2(vPosition.w, vNormal.w);
    float2 vDiffuseTexCoords = float2(vTexCoords1.xy);
    float2 vWindParams = float2(vTexCoords1.zw);
    float2 vDetailTexCoords = float2(vTexCoords2.xy);
    float2 vNormalTexCoords = float2(vTexCoords2.zw);
    float3 vTangent = float3(vTexCoords3.xyz);

    // scale the geometry (each tree instance can be uniquely scaled)
    vPosition.xyz *= g_fTreeScale;

    // rotate the whole tree (each tree instance can be uniquely rotated) - use optimized z-axis rotation
    // algorithm where float(sin(a), cos(a), -sin(a), 0.0f) is uploaded instead of angle a
#ifdef UPVECTOR_POS_Y
    vPosition.xz = float2(dot(g_vTreeRotationTrig.ywz, vPosition.xyz), dot(g_vTreeRotationTrig.xwy, vPosition.xyz));
    vNormal.xz = float2(dot(g_vTreeRotationTrig.ywz, vNormal.xyz), dot(g_vTreeRotationTrig.xwy, vNormal.xyz));
#else
    vPosition.xy = float2(dot(g_vTreeRotationTrig.yxw, vPosition.xyz), dot(g_vTreeRotationTrig.zyw, vPosition.xyz));
    vNormal.xy = float2(dot(g_vTreeRotationTrig.yxw, vNormal.xyz), dot(g_vTreeRotationTrig.zyw, vNormal.xyz));
#endif
        
    // compute new position from wind effect - improved lighting accuracy may be achieved by also transforming 
    // the normal, binormal, and tangent by the wind matrix, but this was judged too expensive for the minimal 
    // effect it achieved.
    vPosition.xyz = WindEffect(vPosition.xyz, vWindParams);
    
    // translate tree into position (must be done after the rotation)
    vPosition.xyz += g_vTreePos.xyz;

#ifdef BRANCH_NORMAL_MAPPING
    // setup per-pixel normal mapping  (assumes normalized light direction)
    
    // derive the binormal from the normal and tangent in order to keep vertex size down
    vTangent.xy = float2(dot(g_vTreeRotationTrig.yxw, vTangent.xyz), dot(g_vTreeRotationTrig.zyw, vTangent.xyz));
    float3 vBinormal = cross(vNormal.xyz, vTangent);
    
    // 0.5f + 0.5f * expr not used since texcoords aren't clamped to [0,1] range - this approach saves
    // several instructions
    sOutput.vNormal.x = dot(g_vLightDir.xyz, vTangent.xyz);
    sOutput.vNormal.y = dot(g_vLightDir.xyz, vBinormal.xyz);
    sOutput.vNormal.z = dot(g_vLightDir.xyz, vNormal.xyz);
#else
    // simple per-vertex lighting (nearly a dozen instructions are still generated, so normal-mapping is about
    // the same expense as far as the vertex shader goes)
#ifdef LOW_COMPILE_TARGET
    sOutput.vNormal.rgb = LightDiffuse(vPosition.xyz, vNormal.xyz, g_vLightDir.xyz, g_vLightDiffuse.xyz, g_vMaterialDiffuse.xyz) + g_vMaterialAmbient;
#else    
    sOutput.vNormal.rgb = LightDiffuse(vPosition.xyz, vNormal.xyz, g_vLightDir.xyz, g_vLightDiffuse.xyz, g_vMaterialDiffuse.xyz);
#endif    

#endif
    
#ifdef SELF_SHADOW_LAYER
    // move self-shadow texture coordinates based on wind strength - motion computed on CPU and uploaded
    sOutput.vSelfShadowTexCoords = vShadowTexCoords + g_vSelfShadowMotion;
#endif
    
    // project position to the screen
    sOutput.vPosition = mul(g_mModelViewProj, float4(vPosition.xyz, 1.0f));

    // pass through other texcoords exactly as they were received
    sOutput.vDiffuseTexCoords.xy = vDiffuseTexCoords.xy;
#ifdef BRANCH_NORMAL_MAPPING
    sOutput.vNormalTexCoords.xy = vNormalTexCoords;
#endif
#ifdef BRANCH_DETAIL_LAYER
    sOutput.vDetailTexCoords = vDetailTexCoords;
#endif

#ifdef USE_FOG 
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    sOutput.vNormal.w = FogValue(sOutput.vPosition.z);
#else
    sOutput.vNormal.w = 1;
#endif
#if !defined( LOW_COMPILE_TARGET )
	float3 vecFex, vecLin;
	
	HoffmanScattering( vPosition, vecLin, vecFex );        
	
	sOutput.Fex.xyz = vecFex;	
	sOutput.Lin = vecLin * 0.4f;
#endif
    return sOutput;
}


///////////////////////////////////////////////////////////////////////  
//  BranchPS
//
//  Branch geometry pixel shader.  This shader can process up to four texture layers:
//      - diffuse bark map
//      - normal map
//      - detail map (lerped against the diffuse map using detail's alpha layer)
//      - self-shadow map (greyscale composite map generated via SpeedTreeCAD)

float4 BranchPS(SBranchOutput In) : COLOR
{
#ifdef BRANCH_NORMAL_MAPPING
    // since the normal map normal values (normally ranged [-1,1]) are stored
    // as a color value (ranged [0,1]), they must be uncompressed.  a dot product 
    // is used to compute the diffuse lighting contribution.
    float4 texNormal = tex2D(samBranchNormalMap, In.vNormalTexCoords);
    const float3 vHalves = { 0.5f, 0.5f, 0.5f };
    float fDot = saturate(dot(2.0f * (texNormal.rgb - vHalves), In.vNormal.rgb));
	//return lerp( float4( 0,0,0,1 ), float4( 1,1,1,1 ), fDot );
#endif

    // look up the diffuse map layer
    float4 texDiffuse = tex2D(samBranchDiffuseMap, In.vDiffuseTexCoords.xy);
    
#ifdef BRANCH_DETAIL_LAYER
    // if branch detail layer is active, look it up and lerp between it
    // and the base layer.  this allows for a smooth transition between diffuse
    // and detail layers if the detail layer's alpha map is done correctly
    // (the diffuse map's alpha layer doesn't figure into it)
    float4 texDetail = tex2D(samBranchDetailMap, In.vDetailTexCoords);
    texDiffuse.rgb = lerp(texDiffuse.rgb, texDetail.rgb, texDetail.a);
#endif
    
    // compute the ambient contribution (pulled from the diffuse map)
    float3 vAmbient = texDiffuse.xyz * g_vMaterialAmbient.xyz;
    
    // compute the diffuse contribution, using normal mapping or standard vertex-based lighting
#ifdef BRANCH_NORMAL_MAPPING
    float3 vColor3 = texDiffuse.rgb * fDot * g_fDiffuseScale * g_vLightDiffuse.rgb;
#else
    float3 vColor3 = texDiffuse.rgb * In.vNormal.rgb * g_fDiffuseScale;
#endif

#ifdef SELF_SHADOW_LAYER
    // overlay the shadow if active
    float4 texShadow = tex2D(samSelfShadowMap, In.vSelfShadowTexCoords);
    vColor3 *= texShadow.xyz;
#endif    

    // compute the full lighting value
    float4 vColor = float4(vColor3 + vAmbient, texDiffuse.a);
#if !defined( LOW_COMPILE_TARGET )
	vColor.xyz = vColor.xyz * In.Fex + In.Lin;
#endif
	
#ifdef USE_FOG
    // if fog is active, interpolate between the unfogged color and the fog color
    // based on vertex shader fog value
    vColor.xyz = lerp(g_vFogColor, vColor, In.vNormal.w).xyz;
#endif
    
    return vColor;
}


///////////////////////////////////////////////////////////////////////  
//  Techniques

technique Branches
{
    pass P0
    {          
        VS_COMPILE_COMMAND BranchVS( );
#if !defined( LOW_COMPILE_TARGET )
        PS_COMPILE_COMMAND BranchPS( );
#endif        
    }
}


