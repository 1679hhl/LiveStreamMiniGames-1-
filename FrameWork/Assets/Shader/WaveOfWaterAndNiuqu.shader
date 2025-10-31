// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yyz_WaveOfWaterAndNiuqu"
{
	Properties
	{
		_Control("整体亮度", Range( 0 , 1)) = 1
		_MapTex("MapTex", 2D) = "white" {}
		_LightNum("LightNum", Range( 0 , 10)) = 2
		_MaskA("MaskA", Range( 0 , 1)) = 0
		_MaskTex("MaskTex", 2D) = "white" {}
		_RaoDongTex("RaoDongTex", 2D) = "white" {}
		_RaoDongTex1("RaoDongTex1", 2D) = "white" {}
		_RaodongSpeed("RaodongSpeed", Vector) = (0.1,-0.1,0.2,0)
		_RaoDongNum("RaoDongNum", Range( 0 , 0.1)) = 0.05
		_DissTex("DissTex", 2D) = "white" {}
		_DissUVL("DissUVL", Vector) = (0.2,-0.2,0,0)
		_JianBianNum("JianBianNum", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One Zero
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MapTex;
			uniform float4 _MapTex_ST;
			uniform float _JianBianNum;
			uniform sampler2D _RaoDongTex;
			uniform float4 _RaodongSpeed;
			uniform float4 _RaoDongTex_ST;
			uniform sampler2D _RaoDongTex1;
			uniform float4 _RaoDongTex1_ST;
			uniform float _RaoDongNum;
			uniform sampler2D _MaskTex;
			uniform float4 _MaskTex_ST;
			uniform float _MaskA;
			uniform float _LightNum;
			uniform sampler2D _DissTex;
			uniform float3 _DissUVL;
			uniform float4 _DissTex_ST;
			uniform float _Control;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_MapTex = i.ase_texcoord1.xy * _MapTex_ST.xy + _MapTex_ST.zw;
				float4 tex2DNode132 = tex2D( _MapTex, uv_MapTex );
				float2 texCoord9 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult16 = (float2(_RaodongSpeed.x , _RaodongSpeed.y));
				float2 uv_RaoDongTex = i.ase_texcoord1.xy * _RaoDongTex_ST.xy + _RaoDongTex_ST.zw;
				float2 panner5 = ( 1.0 * _Time.y * appendResult16 + uv_RaoDongTex);
				float2 appendResult17 = (float2(_RaodongSpeed.z , _RaodongSpeed.w));
				float2 uv_RaoDongTex1 = i.ase_texcoord1.xy * _RaoDongTex1_ST.xy + _RaoDongTex1_ST.zw;
				float2 panner12 = ( 1.0 * _Time.y * appendResult17 + uv_RaoDongTex1);
				float4 tex2DNode13 = tex2D( _RaoDongTex1, panner12 );
				float4 temp_output_138_0 = saturate( ( ( ( 1.0 - texCoord9.y ) * _JianBianNum ) * ( tex2D( _RaoDongTex, panner5 ) * tex2DNode13 ) ) );
				float4 lerpResult7 = lerp( float4( texCoord9, 0.0 , 0.0 ) , temp_output_138_0 , _RaoDongNum);
				float2 uv_MaskTex = i.ase_texcoord1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float4 tex2DNode72 = tex2D( _MaskTex, uv_MaskTex );
				float4 screenPos = i.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult122 = (float2(_DissUVL.x , _DissUVL.y));
				float2 uv_DissTex = i.ase_texcoord1.xy * _DissTex_ST.xy + _DissTex_ST.zw;
				float2 panner95 = ( 1.0 * _Time.y * appendResult122 + uv_DissTex);
				float4 lerpResult81 = lerp( float4( (ase_screenPosNorm).xy, 0.0 , 0.0 ) , ( tex2DNode13 * tex2D( _DissTex, panner95 ) ) , _DissUVL.z);
				float4 lerpResult135 = lerp( tex2DNode132 , saturate( ( tex2DNode132 + ( tex2D( _MapTex, lerpResult7.rg ) * ( temp_output_138_0 * ( tex2DNode72.r + ( _MaskA * 0.1 * _LightNum ) ) ) ) + ( tex2DNode72.r * _MaskA * tex2D( _MapTex, lerpResult81.rg ) ) ) ) , _Control);
				
				
				finalColor = lerpResult135;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.CommentaryNode;107;-2279.442,637.6415;Inherit;False;1829.846;389.3255;niuqu;11;95;92;80;79;121;122;93;131;120;98;81;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;-2901.03,346.318;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;14;-3118.13,255.3179;Inherit;False;Property;_RaodongSpeed;RaodongSpeed;8;0;Create;True;0;0;0;False;0;False;0.1,-0.1,0.2,0;-0.1,-0.1,0.1,0.1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;12;-2744.817,454.5514;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-2558.476,429.9247;Inherit;True;Property;_RaoDongTex1;RaoDongTex1;7;0;Create;True;0;0;0;False;0;False;-1;4a05449a0833b4d47868c8d2c9633e54;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;5;-2751.289,100.2119;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-2564.95,75.58526;Inherit;True;Property;_RaoDongTex;RaoDongTex;6;0;Create;True;0;0;0;False;0;False;-1;4a05449a0833b4d47868c8d2c9633e54;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2899.73,246.2179;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3008.077,99.8368;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-3001.605,454.1763;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1934.177,157.4538;Inherit;False;Property;_RaoDongNum;RaoDongNum;9;0;Create;True;0;0;0;False;0;False;0.05;0.1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;90;-1622.694,292.0753;Inherit;True;Property;_MapTex;MapTex;1;0;Create;True;0;0;0;False;0;False;None;bc362597f3d44f241a6b1879bc44aac6;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;81;-1280.263,718.0773;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-1436.402,743.1212;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;93;-2204.715,740.7479;Inherit;False;0;92;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;122;-2080.414,880.3892;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;121;-2254.419,871.2993;Inherit;False;Property;_DissUVL;DissUVL;11;0;Create;True;0;0;0;False;0;False;0.2,-0.2,0;0.1,-0.1,0.1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScreenPosInputsNode;79;-1939.678,687.2564;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;80;-1664.925,688.2473;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;92;-1728.021,767.346;Inherit;True;Property;_DissTex;DissTex;10;0;Create;True;0;0;0;False;0;False;-1;None;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;95;-1908.057,854.881;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1392.974,8.22197;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;132;-965.9197,-181.6683;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-697.8818,647.0054;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;98;-1007.155,695.5521;Inherit;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;130;-909.405,298.0173;Inherit;False;Property;_MainColor;MainColor;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,1.733333,1.717647,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;72;-1392.992,317.61;Inherit;True;Property;_MaskTex;MaskTex;5;0;Create;True;0;0;0;False;0;False;-1;None;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;128;-1096.641,339.7692;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1561.352,507.9151;Inherit;False;Property;_MaskA;MaskA;4;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-1558.468,578.8982;Inherit;False;Property;_LightNum;LightNum;2;0;Create;True;0;0;0;False;0;False;2;6.96;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-387.6948,-3.387819;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;46;269.0535,-29.02377;Float;False;True;-1;2;ASEMaterialInspector;100;5;Yyz_WaveOfWaterAndNiuqu;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;1;1;False;;0;False;;0;5;True;_Src1;10;True;_Dst1;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;False;;True;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.LerpOp;135;75.04388,-25.35386;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2648.378,-1.382282;Inherit;False;Property;_JianBianNum;JianBianNum;12;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;63;-141.6445,-3.124244;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-1635.566,32.31199;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-649.6558,20.59096;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-402.0348,215.668;Inherit;False;Property;_Control;整体亮度;0;0;Create;False;0;0;0;False;0;False;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-1252.685,506.6057;Inherit;False;3;3;0;FLOAT;0.1;False;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-908.3229,69.65428;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2238.53,81.42749;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2027.636,57.56563;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;138;-1843.446,57.67114;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;137;-2349.27,-22.46252;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2182.496,-24.02455;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-2615.809,-142.8033;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;17;0;14;3
WireConnection;17;1;14;4
WireConnection;12;0;11;0
WireConnection;12;2;17;0
WireConnection;13;1;12;0
WireConnection;5;0;4;0
WireConnection;5;2;16;0
WireConnection;2;1;5;0
WireConnection;16;0;14;1
WireConnection;16;1;14;2
WireConnection;81;0;80;0
WireConnection;81;1;131;0
WireConnection;81;2;121;3
WireConnection;131;0;13;0
WireConnection;131;1;92;0
WireConnection;122;0;121;1
WireConnection;122;1;121;2
WireConnection;80;0;79;0
WireConnection;92;1;95;0
WireConnection;95;0;93;0
WireConnection;95;2;122;0
WireConnection;1;0;90;0
WireConnection;1;1;7;0
WireConnection;132;0;90;0
WireConnection;120;0;72;1
WireConnection;120;1;77;0
WireConnection;120;2;98;0
WireConnection;98;0;90;0
WireConnection;98;1;81;0
WireConnection;128;0;72;1
WireConnection;128;1;127;0
WireConnection;24;0;132;0
WireConnection;24;1;25;0
WireConnection;24;2;120;0
WireConnection;46;0;135;0
WireConnection;135;0;132;0
WireConnection;135;1;63;0
WireConnection;135;2;136;0
WireConnection;63;0;24;0
WireConnection;7;0;9;0
WireConnection;7;1;138;0
WireConnection;7;2;10;0
WireConnection;25;0;1;0
WireConnection;25;1;123;0
WireConnection;127;0;77;0
WireConnection;127;2;133;0
WireConnection;123;0;138;0
WireConnection;123;1;128;0
WireConnection;18;0;2;0
WireConnection;18;1;13;0
WireConnection;29;0;34;0
WireConnection;29;1;18;0
WireConnection;138;0;29;0
WireConnection;137;0;9;2
WireConnection;34;0;137;0
WireConnection;34;1;37;0
ASEEND*/
//CHKSM=0469392401A9F0C729523873C29C2EE5795EF0EF