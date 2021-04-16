Shader "XianXia/UIStencilMask"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 1
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15//阻止UnityUI报错
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp Always
			Pass Replace
		}

		Cull Back
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f
			{
				float4 pos   : SV_POSITION;
				float2 uv  : TEXCOORD0;
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
                clip (col.a - 0.01);
				return fixed4(0, 0, 0, 0);
			}
		ENDCG
		}
	}
}
