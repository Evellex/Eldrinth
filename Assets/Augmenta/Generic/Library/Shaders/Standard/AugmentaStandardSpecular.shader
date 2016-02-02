Shader "Augmenta/Standard (Specular setup)"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_SpecColor("Specular", Color) = (0.2,0.2,0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}

		 _BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Height Map", 2D) = "black" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		_TemperatureMap("Temperature" , 2D) = "black" {}
		_TemperatureMin("Black Temperature", Float) = 295
		_TemperatureMax("White Temperature", Float) = 305
		[HideInInspector] _TemperatureLookupMap("Temperature Lookup" , 2D) = "grey" {}

		_DetailMask("Detail Mask", 2D) = "white" {}

		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0

			// Blending state
			[HideInInspector] _Mode("__mode", Float) = 0.0
			[HideInInspector] _SrcBlend("__src", Float) = 1.0
			[HideInInspector] _DstBlend("__dst", Float) = 0.0
			[HideInInspector] _ZWrite("__zw", Float) = 1.0

			//Cull state
			[HideInInspector] _CullMode("__cull", Float) = 2.0
	}

		CGINCLUDE
#define UNITY_SETUP_BRDF_INPUT SpecularSetup
			ENDCG

			SubShader
		{
			Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
			LOD 600

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)
			Pass
			{
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }

				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_CullMode]

				CGPROGRAM
				#pragma target 5.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define TEMPERATURE_Q_HIGH
			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 50)
				#define DISPLACEMENT_Q_HIGH
			#elif (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
			{
				Name "FORWARD_DELTA"
				Tags { "LightMode" = "ForwardAdd" }
				Blend[_SrcBlend] One
				Fog { Color(0,0,0,0) } // in additive pass fog should be black
				ZWrite Off
				ZTest LEqual

				CGPROGRAM
				#pragma target 5.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 50)
				#define DISPLACEMENT_Q_HIGH
			#elif (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 5.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#if (SHADER_TARGET >= 50)
				#define DISPLACEMENT_Q_HIGH
			#elif (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Deferred pass
			Pass
			{
				Name "DEFERRED"
				Tags { "LightMode" = "Deferred" }

				Cull[_CullMode]

				CGPROGRAM
				#pragma target 5.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define TEMPERATURE_Q_HIGH
			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 50)
				#define DISPLACEMENT_Q_HIGH
			#elif (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.

			Pass
			{
				Name "META"
				Tags { "LightMode" = "Meta" }

				Cull Off

				CGPROGRAM
				#pragma target 5.0
				#pragma vertex vert_meta
				#pragma fragment frag_meta

				#pragma shader_feature _EMISSION
				#pragma shader_feature _RADIANCE
				#pragma shader_feature _TEMPERATURE
				#pragma shader_feature _SPECGLOSSMAP
				#pragma shader_feature ___ _DETAIL_MULX2

				#define TEMPERATURE_Q_HIGH
				#define BRDF_Q_HIGH
				#if (SHADER_TARGET >= 50)
					#define DISPLACEMENT_Q_HIGH
				#elif (SHADER_TARGET >= 30)
					#define DISPLACEMENT_Q_LOW
				#endif

				#include "AugmentaStandardMeta.cginc"

				ENDCG
			}
		}

			SubShader
		{
			Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
			LOD 500

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)
			Pass
			{
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }

				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define TEMPERATURE_Q_HIGH
			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
			{
				Name "FORWARD_DELTA"
				Tags { "LightMode" = "ForwardAdd" }
				Blend[_SrcBlend] One
				Fog { Color(0,0,0,0) } // in additive pass fog should be black
				ZWrite Off
				ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Deferred pass
			Pass
			{
				Name "DEFERRED"
				Tags { "LightMode" = "Deferred" }

				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#define TEMPERATURE_Q_HIGH
			#define BRDF_Q_HIGH
			#if (SHADER_TARGET >= 30)
				#define DISPLACEMENT_Q_LOW
			#endif

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.

			Pass
			{
				Name "META"
				Tags { "LightMode" = "Meta" }

				Cull Off

				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert_meta
				#pragma fragment frag_meta

				#pragma shader_feature _EMISSION
				#pragma shader_feature _RADIANCE
				#pragma shader_feature _TEMPERATURE
				#pragma shader_feature _SPECGLOSSMAP
				#pragma shader_feature ___ _DETAIL_MULX2

				#define TEMPERATURE_Q_HIGH
				#define BRDF_Q_HIGH

				#include "AugmentaStandardMeta.cginc"

				ENDCG
			}
		}

			SubShader
		{
			Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
			LOD 400

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)
			Pass
			{
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }

				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_MED

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
			{
				Name "FORWARD_DELTA"
				Tags { "LightMode" = "ForwardAdd" }
				Blend[_SrcBlend] One
				Fog { Color(0,0,0,0) } // in additive pass fog should be black
				ZWrite Off
				ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2

			#define BRDF_Q_MED

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Deferred pass
			Pass
			{
				Name "DEFERRED"
				Tags { "LightMode" = "Deferred" }

				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_MED

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.

			Pass
			{
				Name "META"
				Tags { "LightMode" = "Meta" }

				Cull Off

				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert_meta
				#pragma fragment frag_meta

				#pragma shader_feature _EMISSION
				#pragma shader_feature _RADIANCE
				#pragma shader_feature _TEMPERATURE
				#pragma shader_feature _SPECGLOSSMAP
				#pragma shader_feature ___ _DETAIL_MULX2

				#define TEMPERATURE_Q_LOW
				#define BRDF_Q_MED

				#include "AugmentaStandardMeta.cginc"

				ENDCG
			}
		}

			SubShader
		{
			Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
			LOD 300

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)
			Pass
			{
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }

				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_MED

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
			{
				Name "FORWARD_DELTA"
				Tags { "LightMode" = "ForwardAdd" }
				Blend[_SrcBlend] One
				Fog { Color(0,0,0,0) } // in additive pass fog should be black
				ZWrite Off
				ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP

			#define BRDF_Q_MED

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Deferred pass
			Pass
			{
				Name "DEFERRED"
				Tags { "LightMode" = "Deferred" }

				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_MED

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.

			Pass
			{
				Name "META"
				Tags { "LightMode" = "Meta" }

				Cull Off

				CGPROGRAM
				#pragma vertex vert_meta
				#pragma fragment frag_meta

				#pragma shader_feature _EMISSION
				#pragma shader_feature _RADIANCE
				#pragma shader_feature _TEMPERATURE
				#pragma shader_feature _SPECGLOSSMAP

				#define TEMPERATURE_Q_LOW
				#define BRDF_Q_MED

				#include "AugmentaStandardMeta.cginc"

				ENDCG
			}
		}

			SubShader
		{
			Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
			LOD 200

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)
			Pass
			{
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }

				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP
			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_LOW

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
			{
				Name "FORWARD_DELTA"
				Tags { "LightMode" = "ForwardAdd" }
				Blend[_SrcBlend] One
				Fog { Color(0,0,0,0) } // in additive pass fog should be black
				ZWrite Off
				ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma skip_variants SHADOWS_SOFT

			#define BRDF_Q_LOW

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles
			#pragma skip_variants SHADOWS_SOFT

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
			// ------------------------------------------------------------------
			//  Deferred pass
			Pass
			{
				Name "DEFERRED"
				Tags { "LightMode" = "Deferred" }

				Cull[_CullMode]

				CGPROGRAM
				#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles

			// -------------------------------------

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RADIANCE
			#pragma shader_feature _TEMPERATURE
			//ALWAYS ON shader_feature _GLOSSYENV
			#pragma shader_feature _SPECGLOSSMAP

			#define TEMPERATURE_Q_LOW
			#define BRDF_Q_LOW

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "AugmentaStandardCore.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.

			Pass
			{
				Name "META"
				Tags { "LightMode" = "Meta" }

				Cull Off

				CGPROGRAM
				#pragma vertex vert_meta
				#pragma fragment frag_meta

				#pragma shader_feature _EMISSION
				#pragma shader_feature _RADIANCE
				#pragma shader_feature _TEMPERATURE
				#pragma shader_feature _SPECGLOSSMAP

				#define TEMPERATURE_Q_LOW
				#define BRDF_Q_LOW

				#include "AugmentaStandardMeta.cginc"

				ENDCG
			}
		}

			FallBack "VertexLit"
			CustomEditor "AugmentaEditor.StandardShader_Editor"
}