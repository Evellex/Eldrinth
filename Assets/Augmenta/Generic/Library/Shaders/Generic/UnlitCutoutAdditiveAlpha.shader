Shader "Augmenta/Unlit/Cutout-AdditiveAlpha"
{
	Properties
	{
		_AlbedoTex("Colour & Alpha (RGBA)", 2D) = "white" {}
		_MainTex("Additive Alpha (A)", 2D) = "white" {}
		_Color("Colour Tint" , Color) = (1,1,1)
		_ColorMult("Colour Multiplier" , Float) = 1
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_AlphaCutoffDir1("Alpha Cutoff Direction", Range(0,1)) = 0
	}

	SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100
		Fog{ Mode Off }
		Lighting Off
		Cull Off
		ZWrite On
		ZTest LEqual
		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					half2 texcoord1 : TEXCOORD1;
				};

				sampler2D _AlbedoTex;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _AlbedoTex_ST;
				float4 _Color;
				float _ColorMult;
				fixed _Cutoff;
				fixed _AlphaCutoffDir1;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _AlbedoTex);
					o.texcoord1 = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_AlbedoTex, i.texcoord);
					fixed alpha = 1 - (clamp((1-tex2D(_MainTex, i.texcoord1).a) + (1-col.a),0,1));
					clip(abs(round(_AlphaCutoffDir1) - alpha) - _Cutoff);
					col = col * _Color * _ColorMult;
					col.a = alpha;
					return col;
				}
			ENDCG
		}
	}
}