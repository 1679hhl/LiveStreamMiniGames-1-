// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yyz/ScaleAndOffset"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		[Toggle(_KEYWORD0_ON)] _Keyword0("呼吸", Float) = 1
		_ScaleX("ScaleX", Float) = 10
		_ScaleY("ScaleY", Float) = 10
		[Toggle(_KEYWORD1_ON)] _Keyword1("漂浮", Float) = 1
		_Rotator("Rotator", Float) = 0
		[Toggle(_KEYWORD2_ON)] _Keyword2("摇摆", Float) = 1
		_Rotator1("幅度", Float) = 360
		_Float2("频率", Float) = 1

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
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
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"
			#pragma shader_feature_local _KEYWORD2_ON
			#pragma shader_feature_local _KEYWORD1_ON
			#pragma shader_feature_local _KEYWORD0_ON

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _ScaleX;
			uniform float _ScaleY;
			uniform float _Rotator;
			uniform float _Float2;
			uniform float _Rotator1;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float temp_output_29_0 = ( _SinTime.w * _ScaleY );
				float2 appendResult18 = (float2(( _ScaleX * _SinTime.w ) , temp_output_29_0));
				float2 appendResult16 = (float2(0.0 , temp_output_29_0));
				#ifdef _KEYWORD0_ON
				float2 staticSwitch46 = ((uv_MainTex*1.0 + -0.5)*appendResult18 + appendResult16);
				#else
				float2 staticSwitch46 = uv_MainTex;
				#endif
				float2 temp_cast_0 = (_Rotator).xx;
				float cos39 = cos( 1.0 * _Time.y );
				float sin39 = sin( 1.0 * _Time.y );
				float2 rotator39 = mul( staticSwitch46 - temp_cast_0 , float2x2( cos39 , -sin39 , sin39 , cos39 )) + temp_cast_0;
				float2 panner40 = ( 1.0 * _Time.y * float2( 0,0 ) + rotator39);
				#ifdef _KEYWORD1_ON
				float2 staticSwitch47 = panner40;
				#else
				float2 staticSwitch47 = staticSwitch46;
				#endif
				float cos53 = cos( ( _SinTime.w * _Float2 ) );
				float sin53 = sin( ( _SinTime.w * _Float2 ) );
				float2 rotator53 = mul( staticSwitch47 - float2( 0.5,0.5 ) , float2x2( cos53 , -sin53 , sin53 , cos53 )) + float2( 0.5,0.5 );
				#ifdef _KEYWORD2_ON
				float2 staticSwitch55 = ( rotator53 * _Rotator1 );
				#else
				float2 staticSwitch55 = staticSwitch47;
				#endif
				
				
				OUT.worldPosition.xyz += float3( staticSwitch55 ,  0.0 );
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
8;401;1904;590;423.3766;380.298;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;44;-1794.263,-229.2949;Inherit;False;1104.481;512.877;呼吸;12;46;8;16;18;20;21;22;29;32;19;12;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinTimeNode;28;-1767.215,109.4475;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-1627.177,89.44682;Inherit;False;Property;_ScaleX;ScaleX;1;0;Create;True;0;0;0;False;0;False;10;-3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1626.909,203.9903;Inherit;False;Property;_ScaleY;ScaleY;2;0;Create;True;0;0;0;False;0;False;10;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;42;-2229.211,-299.2587;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1606.786,-81.75293;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1620.452,-4.752714;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2034.396,-200.0706;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1481.199,94.47034;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1482.603,184.4785;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1326.643,183.1936;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;20;-1434.165,-139.2002;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1326.111,92.99003;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;8;-1153.581,-140.9156;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;49;-589.627,-226.5154;Inherit;False;783.2532;211.9848;漂浮;4;47;40;36;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;46;-887.5447,-204.3378;Inherit;False;Property;_Keyword0;呼吸;0;0;Create;False;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-569.0801,-89.83052;Inherit;False;Property;_Rotator;Rotator;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;39;-419.0115,-145.9241;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;51;304.1072,-229.3276;Inherit;False;858.2531;303.9848;漂浮;7;55;66;52;53;68;67;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;67;334.0694,-19.00958;Inherit;False;Property;_Float2;频率;7;0;Create;False;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;40;-225.7742,-147.0347;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinTimeNode;58;326.8558,-164.6;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;47;-22.22733,-207.7153;Inherit;False;Property;_Keyword1;漂浮;3;0;Create;False;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;467.1429,-37.97468;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;53;615.7234,-180.236;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;52;668.9643,-40.43222;Inherit;False;Property;_Rotator1;幅度;6;0;Create;False;0;0;0;False;0;False;360;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;806.6147,-180.6349;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;55;959.5109,-207.8282;Inherit;False;Property;_Keyword2;摇摆;5;0;Create;False;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;7;1254.815,-226.1815;Float;False;True;-1;2;ASEMaterialInspector;0;4;Yyz/ScaleAndOffset;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;14;2;42;0
WireConnection;32;0;12;0
WireConnection;32;1;28;4
WireConnection;29;0;28;4
WireConnection;29;1;19;0
WireConnection;16;1;29;0
WireConnection;20;0;14;0
WireConnection;20;1;21;0
WireConnection;20;2;22;0
WireConnection;18;0;32;0
WireConnection;18;1;29;0
WireConnection;8;0;20;0
WireConnection;8;1;18;0
WireConnection;8;2;16;0
WireConnection;46;1;14;0
WireConnection;46;0;8;0
WireConnection;39;0;46;0
WireConnection;39;1;36;0
WireConnection;40;0;39;0
WireConnection;47;1;46;0
WireConnection;47;0;40;0
WireConnection;68;0;58;4
WireConnection;68;1;67;0
WireConnection;53;0;47;0
WireConnection;53;2;68;0
WireConnection;66;0;53;0
WireConnection;66;1;52;0
WireConnection;55;1;47;0
WireConnection;55;0;66;0
WireConnection;7;1;55;0
ASEEND*/
//CHKSM=7E1A47102CB99C3AC5BBE78E92BB0355F7C424C5