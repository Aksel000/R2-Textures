///////////////////////////////////////////////////////////////////////  
//  Frond Shaders
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
//  Frond-specific global variables

// vs (none)

// ps (none)


///////////////////////////////////////////////////////////////////////  
//  Texture samplers

// (none specific to the frond geometry)


///////////////////////////////////////////////////////////////////////  
//  Frond VS Ouput, PS Input

struct SFrondOutput
{
    float4 vPosition            : POSITION;
    float2 vDiffuseTexCoords    : TEXCOORD0;
#ifdef SELF_SHADOW_LAYER
    float2 vSelfShadowTexCoords : TEXCOORD1;
#endif
    float4 vColor               : COLOR0;
#ifdef USE_FOG
    float fFog                  : TEXCOORD2; // using FOG here causes a ps_2_0 compilation failure
#endif
#if !defined( LOW_COMPILE_TARGET )
    float3 Fex					: TEXCOORD3;
    float3 Lin					: TEXCOORD4;
#endif
};


///////////////////////////////////////////////////////////////////////  
//  FrondVS
//
//  Frond geometry vertex shader

SFrondOutput FrondVS(float4     vPosition   : POSITION,
                     float4     vNormal     : TEXCOORD0,
                     float4     vTexCoords1 : TEXCOORD1)
{
    // this will be fed to the frond pixel shader
    SFrondOutput sOutput;

    // define attribute aliases for readability
    float2 vSelfShadowTexCoords = float2(vPosition.w, vNormal.w);
    float2 vDiffuseTexCoords = float2(vTexCoords1.xy);
    float2 vWindParams = float2(vTexCoords1.zw);
    
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
    
    // compute new position from wind effect
    vPosition.xyz = WindEffect(vPosition.xyz, vWindParams);
    
    // translate tree into position (must be done after the rotation)
    vPosition.xyz += g_vTreePos.xyz;

    // compute the frond lighting (not using normal mapping, but per-vertex lighting)
    sOutput.vColor.rgb = LightDiffuse(vPosition.xyz, vNormal.xyz, g_vLightDir.xyz, g_vLightDiffuse.xyz, g_vMaterialDiffuse.xyz);
#ifdef LOW_COMPILE_TARGET
	sOutput.vColor.rgb += g_vMaterialAmbient.xyz;
#endif    
    sOutput.vColor.a = 1.0f;
    
#ifdef SELF_SHADOW_LAYER
    // move self-shadow texture coordinates based on wind strength - motion computed on CPU and uploaded
    sOutput.vSelfShadowTexCoords = vSelfShadowTexCoords + g_vSelfShadowMotion;;
#endif
    
    // project position to the screen
    sOutput.vPosition = mul(g_mModelViewProj, float4(vPosition.xyz, 1.0f));
    
    // pass through other texcoords exactly as they were received
    sOutput.vDiffuseTexCoords = vDiffuseTexCoords;

#ifdef USE_FOG      
    // calc fog (cheap in vertex shader, relatively expensive later in the pixel shader)
    sOutput.fFog = FogValue(sOutput.vPosition.z);
    
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
//  FrondPS
//
//  Frond geometry pixel shader.  This shader can process up to two texture layers:
//      - diffuse composite leaf/frond map
//      - self-shadow map (greyscale composite map generated via SpeedTreeCAD)

float4 FrondPS(SFrondOutput In) : COLOR
{
    // look up the diffuse layer
    float4 texDiffuse = tex2D(samCompositeLeafMap, In.vDiffuseTexCoords.xy);
    
    // compute the ambient contribution (pulled from the diffuse map)
    float3 vAmbient = texDiffuse.xyz * g_vMaterialAmbient.xyz;

    // compute the full lighting equation, including diffuse and ambient values and
    // their respective scales.
    float3 vColor3 = In.vColor.rgb * texDiffuse.rgb * g_fDiffuseScale;
 
#ifdef SELF_SHADOW_LAYER
    // overlay the shadow if active
    float4 texShadow = tex2D(samSelfShadowMap, In.vSelfShadowTexCoords.xy);
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
    vColor.xyz = lerp(g_vFogColor, vColor, In.fFog).xyz;
#endif

    return vColor;
}


///////////////////////////////////////////////////////////////////////  
//  Techniques

technique Fronds
{
    pass P0
    {          
        VS_COMPILE_COMMAND FrondVS( );
#if !defined( LOW_COMPILE_TARGET )
        PS_COMPILE_COMMAND FrondPS( );
#endif        
    }
}
