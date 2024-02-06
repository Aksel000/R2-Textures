///////////////////////////////////////////////////////////////////////  
//  SpeedTree Shaders File
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
//
//  These shaders were written to be used as HLSL FX or CgFX files for the
//  following platforms/APIs:
//      - Windows DirectX 9.0c
//      - Windows OpenGL 1.4+
//      - Xbox(R) 360
//      - PLAYSTATION(R)3
//
//  SpeedTree.fx is the main fx files that's compiled by the application.  It
//  includes several other fx files that define shaders for each of the geometry
//  types composing a SpeedTree model.
//
//  As of the initial release of SpeedTreeRT 4.0, Cg 1.5 for the PC was still
//  in a beta release and still had a few critical bugs that prohibited it
//  for being used.  Cg 1.4 is used instead.  This earlier version does not
//  support some of the same keywords as HLSL FX files, especially in the
//  compile statements and sampler declaration.  As a result, some of the
//  commands are #ifdef'd around the macro IDV_OPENGL.
//
//  There are several macros used throughout these fx files that are defined
//  at runtime by the application (they're passed in with the compile command).
//  They are listed alphabetically below:
//
//  BILLBOARD_NORMAL_MAPPING
//  Used in the billboard shaders to support a special type of normal map that
//  allows very smooth transitions from 3D trees into dynamically lit billboard
//  images.
//
//  BRANCH_DETAIL_LAYER
//  The sample branch shaders include support for three texture layers:  diffuse
//  map, normal map, and detail map.  Support of the detail map layer is optional 
//  and is enabled by defining this macro.  It is used only in Branch.fx.
//
//  BRANCH_NORMAL_MAPPING
//  Used in the branch shaders to support standard normal mapping on the bark
//  textures.
//
//  IDV_OPENGL
//  Defined for Windows PC and PLAYSTATION3 builds.  When not defined, Windows
//  DirectX 9.0c and Xbox 360 builds are assumed.  It is used in most of the
//  fx files.
//
//  LOW_COMPILE_TARGET
//  When compiling to lower-end compile targets like vs_1_1 and ps_1_1, this should
//  be #defined.  It turns off some of the features that are not possible in these
//  more restrictive profiles (ps_1_1 being the most restrictive).
//
//  NUM_360_IMAGES
//  This macro defines maximum number of individual 360-degree images that can
//  uploaded per tree.  It is used only in Billboard.fx
//
//  NUM_LEAF_ANGLES
//  Each of the leaves in a particular tree model point to one of several
//  different rock and rustle angles.  This allows independent wind behavior for
//  the leaves, leading to a more realistic wind effect.  This macro is set to 
//  define the number of leaf rock/rustle angles that can be uploaded.  It is
//  used only in Leaf.fx.
//
//  NUM_WIND_MATRICES
//  Each main branch, and its corresponding children (smaller branches, leaves) 
//  refer to a single wind matrix to define its sway behavor.  This macro defines
//  the total number of wind matrices uploaded per frame. It is used in Utility.fx
//  where the wind function is housed, which is called by Branch.fx, Frond.fx, and
//  Leaf.fx.
//
//  PS3
//  Used to specify the unique vertex and pixel shader build types
//  (sce_vp_rsx and sce_fp_rsx).
//
//  SELF_SHADOW_LAYER
//  Enables self-shadow texture layer support in the branch and frond shaders.
//
//  SUPPORT_360_BILLBOARDS
//  Most applications should use this macro, which enables the billboard vertex
//  shaders BillboardVS1 and BillboardVS2 to compute the fade alpha values during
//  360 image transitions.  Disabling the macro will fade the shaders only compute
//  alpha values for the fade between 3D and billboard.
//
//  TWO_WEIGHT_WIND
//  In versions prior to 4.0, SpeedTree used one wind weight per vertex.  To enable
//  the newer 4.0-style two-weight wind, this must be #defined.
//
//  UPVECTOR_POS_Y
//  Should be enabled for applications that use +Y as the up axis (as opposed to +Z)
//
//  USE_FOG
//  The use of fog in a pixel shader is a non-trivial expense.  It can be toggled
//  using this macro.  It is used in most of the fx files.
//
//  VS_COMPILE_COMMAND and PS_COMPILE_COMMAND
//  Three of the four platforms have unique shader compile targets and 
//  commands. The complete compile commands, including the targets, are stored 
//  in these macros.  They're used in most of the fx files.

#ifdef IDV_OPENGL
    // if using arbvp1/arbfp1, see next comment section below - while the Cg documentation 
    // states that the vp20/fp20 profiles are virtually identical to arbvp1/arbfp1, our 
    // experience does not bear this out.  compiling with the vp20/fp20 profile using 
    // Cg 1.5 Beta 2 yields internal compiler errors and should be avoided.
    #define VS_COMPILE_COMMAND VertexProgram = compile vp30
    #define PS_COMPILE_COMMAND FragmentProgram = compile fp30
#else
    // if using vs_1_1/ps_1_1, see next comment section below
#ifdef LOW_COMPILE_TARGET
    #define VS_COMPILE_COMMAND VertexShader = compile vs_1_1
#else    
    #define VS_COMPILE_COMMAND VertexShader = compile vs_2_0
#endif    
    #define PS_COMPILE_COMMAND PixelShader = compile ps_2_0
#endif


//  Comment in LOW_COMPILE_TARGET when compiling to lower targets like vs_1_1/ps_1_1 and
//  vp20/fp20.  It will make sure some of the more advanced shader effects are disabled.
//  Also #undef BRANCH_NORMAL_MAPPING in the C++ project.

//#define LOW_COMPILE_TARGET
#ifdef LOW_COMPILE_TARGET
#undef BRANCH_NORMAL_MAPPING
#undef BILLBOARD_NORMAL_MAPPING
#endif


///////////////////////////////////////////////////////////////////////  
//  Because the PS3 does not support run-time compilation, it is necessary to
//  specify which registers the shared global variables should be stored in.  This
//  macro handles this appropriately with PS3 is #defined and is ignored otherwise.

#ifdef PS3
#define REG(x) : C##x
#else
#define REG(x)
#endif


///////////////////////////////////////////////////////////////////////  
//  Common Global Variables
//
//  These are all global variables that are shared by more than one of
//  the main shader types (branch, frond, leaf, and billboard).  The constant
//  registers are specified after many of the global parameters in order
//  to ensure that the variables will be successfully shared even when the
//  shaders are compiled off-line separately.
//
//  Note: What we call "composite maps" or "composite textures" are also known
//        as "texture atlases"

// textures
texture     g_tCompositeLeafMap;                        // this composite texture contains all of the leaf and frond images
texture     g_tSelfShadowMap;                           // this composite texture contains all of the self-shadow maps (greyscale)

// wind
float       g_fWindMatrixOffset REG(6);                 // keeps two instances of the same tree model from using the same wind matrix (range: [0,NUM_WIND_MATRICES])
float4x4    g_amWindMatrices[NUM_WIND_MATRICES] REG(21);// houses all of the wind matrices shared by all geometry types
float2      g_vSelfShadowMotion REG(7);                 // offsets the self-shadow coordinates based on wind strength

// lighting & materials
float3      g_vLightDir REG(9);                         // normalized light direction (shaders assume a single light source)
float4      g_vLightDiffuse REG(10);                    // light's diffuse color
float4      g_vMaterialDiffuse REG(11);                 // active material's diffuse color (set differently for branches, fronds, leaves, and billboards)
float       g_fDiffuseScale REG(12);                    // mirrors the diffuse scalar value in CAD, used to help combat the darkening effect of multiple texture layers
float4      g_vMaterialAmbient REG(13);                 // active material's ambient color (set differently for branches, fronds, leaves, and billboards)
float       g_fAmbientScale REG(14);                    // mirrors the ambient scalar value in CAD, used to help combat the darkening effect of multiple texture layers
float       g_fLeafLightingAdjust REG(15);              // used to clamp the dot product operation during the leaf vertex lighting operations

// other
float4x4    g_mModelViewProj REG(0);                    // composite modelview/projection matrix
float4      g_vFogColor REG(16);                        // fog color
float3      g_vFogParams REG(17);                       // used for fog distance calcs: .x = fog start, .y = fog end, .z = end - start
float4      g_vTreePos REG(4);                          // each tree is in a unique position and rotation: .xyz = pos, .w = rotation
float4      g_vTreeRotationTrig REG(19);                // stores (sin, cos, -sin, 0.0) for an instance's rotation angle (optimizes rotation code)
float       g_fTreeScale REG(5);                        // each tree has a unique scale (1.0 is no scale)
float2      g_vCameraAngles REG(8);                     // shared by Billboard.fx and Leaf.fx - stores camera azimuth and pitch for billboarding computations
float4      g_vCameraAzimuthTrig REG(20);               // stores (sin, cos, -sin, 0.0) for the camera azimuth angle for optimized rotation in billboard vs

// Rainbow Scattering
float3	    g_vViewPosition REG(25);                    
float3		g_vSkyColor REG(28);
float		g_fMultiplySunColor REG(29);
float		g_fMultiplyHorizonColor REG(30);
float3		g_vHorizonColor REG(31);
float		g_fHorizonExp REG(32);
float		g_fSunExp REG(33);
float		g_fScatterEnd REG(34);
float3		g_vScatterStartColor REG(35);
float3		g_vScatterEndColor REG(36);
float		g_fMultiplyObjectScatter REG(37);

///////////////////////////////////////////////////////////////////////  
//  Common Texture Samplers
//
//  These are samplers shared by more than one pixel shader

sampler2D samCompositeLeafMap : register(s0) = sampler_state
{
    Texture = <g_tCompositeLeafMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#else
    MagFilter = Linear;
    MipFilter = None;
    MinFilter = None;
#endif
};

#ifdef SELF_SHADOW_LAYER
sampler2D samSelfShadowMap : register(s1) = sampler_state
{
    Texture = <g_tSelfShadowMap>;
#ifndef IDV_OPENGL
    MagFilter = Linear;
    MipFilter = Linear;
    MinFilter = Linear;
#else
    MagFilter = Linear;
    MipFilter = None;
    MinFilter = None;
#endif
};
#endif

#include "Utility.fx"
#include "Branch.fx"
#include "Leaf.fx"
#include "Frond.fx"
#include "Billboard.fx"
