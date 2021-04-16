Shader "UI/xProject-Default"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		[Toggle(GAME_GREY)] _Grey("是否灰度", Float) = 0
		[HideInInspector] _ColorfulTex("_ColorfulTex", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="DefaultUI" 
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
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			Name "Default"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			#pragma multi_compile __ GAME_GREY
			#pragma multi_compile __ UI_ROUNDNESS_MASK UI_TEXT_OUTLINE UI_COLORFUL_TEXT

			struct appdata_t
			{
				float4 vertex   : POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;

				#if defined(UI_ROUNDNESS_MASK) || defined(UI_TEXT_OUTLINE)
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				#elif defined(UI_COLORFUL_TEXT)
				float2 uv1 : TEXCOORD1;
				#endif
				
				#ifdef UI_TEXT_OUTLINE
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
                float2 uv  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;

				#ifdef UI_ROUNDNESS_MASK
				float3 pos : TEXCOORD2;
				float2 param : TEXCOORD3;
				float3 center_pos : TEXCOORD4;
				#elif defined(UI_TEXT_OUTLINE)
				float4 uv_range : TEXCOORD2;
				fixed4 outline_color : TEXCOORD3;
				float uv_offset_scale : TEXCOORD4;
				#elif defined(UI_COLORFUL_TEXT)
				float2 uv2 : TEXCOORD2;
				#endif
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			#ifdef UI_COLORFUL_TEXT
			sampler2D _ColorfulTex;
			#endif

			v2f vert(appdata_t IN)
			{
				v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.uv = IN.texcoord;

				#ifdef UI_ROUNDNESS_MASK
				OUT.pos = mul(unity_ObjectToWorld, float4(IN.vertex.xyz, 1));
				OUT.center_pos = float3(IN.uv1, IN.uv2.x);
				OUT.param = float2(IN.uv2.y, IN.uv3.x);
				#endif

				#ifdef UI_TEXT_OUTLINE
				OUT.uv_range.xy = IN.uv1;
				OUT.uv_range.zw = IN.uv2;
				OUT.outline_color = fixed4(IN.uv3.x, IN.uv3.y, IN.tangent.z, IN.tangent.w);
				OUT.uv_offset_scale = IN.normal.z;
				#endif

				#ifdef UI_COLORFUL_TEXT
				OUT.uv2 = IN.uv1;
				#endif

				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0) * half2(-1, 1) * OUT.vertex.w;
				#endif
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			#ifdef UI_TEXT_OUTLINE
			//By Global Setting
			uniform float _OutlineSinArray[8];
			uniform float _OutlineCosArray[8];
			fixed IsInUVRange(float2 uv, v2f data)
			{
				fixed2 d = step(uv, data.uv_range.zw) * step(data.uv_range.xy, uv);
				return d.x * d.y;
			}
			fixed SampleOutlineAlpha(half idx, v2f data)
			{
				float2 uv = data.uv + float2(_OutlineCosArray[idx], _OutlineSinArray[idx]) * data.uv_offset_scale;
				fixed alpha = tex2D(_MainTex, uv).a + _TextureSampleAdd.a;
				alpha *= IsInUVRange(uv, data);
				return alpha;
			}
			#endif

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 tex_col = (tex2D(_MainTex, IN.uv) + _TextureSampleAdd);

				#ifdef UI_COLORFUL_TEXT
				fixed4 c_f = tex2D(_ColorfulTex, IN.uv2);
				tex_col.rgb = c_f.rgb;
				#endif

				fixed4 color = tex_col * IN.color;

				#ifdef UI_ROUNDNESS_MASK
				float dist = distance(IN.pos.xyz, IN.center_pos);
				float v = (dist - IN.param.x + IN.param.y) / IN.param.y;
				color.a *= lerp(1, 0, saturate(v));
				#endif

				#ifdef UI_TEXT_OUTLINE
				half4 o_col = half4(IN.outline_color.rgb, 0);
				o_col.a += SampleOutlineAlpha(0, IN);
				o_col.a += SampleOutlineAlpha(1, IN);
				o_col.a += SampleOutlineAlpha(2, IN);
				o_col.a += SampleOutlineAlpha(3, IN);
				o_col.a += SampleOutlineAlpha(4, IN);
				o_col.a += SampleOutlineAlpha(5, IN);
				o_col.a += SampleOutlineAlpha(6, IN);
				o_col.a += SampleOutlineAlpha(7, IN);

				o_col.a = IN.color.a * saturate(o_col.a) * IN.outline_color.a;
				color.a *= IsInUVRange(IN.uv, IN);
				color = color * color.a + o_col * (1 - color.a);

				#endif

                #ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				#ifdef GAME_GREY
				color.rgb = dot(color.rgb, unity_ColorSpaceLuminance.rgb);
				#endif
				return color;
			}

			ENDCG
		}
	}
}
