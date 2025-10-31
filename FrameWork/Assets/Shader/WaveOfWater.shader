// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yyz_WaveOfWater"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_RaoDongTex("RaoDongTex", 2D) = "white" {}
		_RaoDongTex1("RaoDongTex1", 2D) = "white" {}
		_RaoDongNum("RaoDongNum", Range( 0 , 0.1)) = 0.05326087
		_RaodongSpeed("RaodongSpeed", Vector) = (0.1,-0.1,0.2,0)
		_JianBianNum("JianBianNum", Range( 0 , 1)) = 1
		_LightNum("LightNum", Float) = 2

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float _JianBianNum;
			uniform sampler2D _RaoDongTex;
			uniform float4 _RaodongSpeed;
			uniform float4 _RaoDongTex_ST;
			uniform sampler2D _RaoDongTex1;
			uniform float4 _RaoDongTex1_ST;
			uniform float _RaoDongNum;
			uniform float _LightNum;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

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
				float2 texCoord9 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult16 = (float2(_RaodongSpeed.x , _RaodongSpeed.y));
				float2 uv_RaoDongTex = i.ase_texcoord1.xy * _RaoDongTex_ST.xy + _RaoDongTex_ST.zw;
				float2 panner5 = ( 1.0 * _Time.y * appendResult16 + uv_RaoDongTex);
				float2 appendResult17 = (float2(_RaodongSpeed.z , _RaodongSpeed.w));
				float2 uv_RaoDongTex1 = i.ase_texcoord1.xy * _RaoDongTex1_ST.xy + _RaoDongTex1_ST.zw;
				float2 panner12 = ( 1.0 * _Time.y * appendResult17 + uv_RaoDongTex1);
				float4 temp_output_29_0 = ( ( ( 1.0 - texCoord9.y ) * _JianBianNum ) * ( tex2D( _RaoDongTex, panner5 ) * tex2D( _RaoDongTex1, panner12 ) ) );
				float4 lerpResult7 = lerp( float4( texCoord9, 0.0 , 0.0 ) , temp_output_29_0 , _RaoDongNum);
				float4 tex2DNode1 = tex2D( _MainTex, lerpResult7.rg );
				
				
				finalColor = saturate( ( tex2DNode1 + ( tex2DNode1 * temp_output_29_0 * _LightNum ) ) );
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
Node;AmplifyShaderEditor.DynamicAppendNode;17;-2901.03,346.318;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;14;-3118.13,255.3179;Inherit;False;Property;_RaodongSpeed;RaodongSpeed;4;0;Create;True;0;0;0;False;0;False;0.1,-0.1,0.2,0;0.5,-0.2,-0.2,-0.15;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;12;-2744.817,454.5514;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-2558.476,429.9247;Inherit;True;Property;_RaoDongTex1;RaoDongTex1;2;0;Create;True;0;0;0;False;0;False;-1;4a05449a0833b4d47868c8d2c9633e54;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;5;-2751.289,100.2119;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-2564.95,75.58526;Inherit;True;Property;_RaoDongTex;RaoDongTex;1;0;Create;True;0;0;0;False;0;False;-1;4a05449a0833b4d47868c8d2c9633e54;4a05449a0833b4d47868c8d2c9633e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2899.73,246.2179;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3008.077,99.8368;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2237.203,74.79226;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-3001.605,454.1763;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1934.177,157.4538;Inherit;False;Property;_RaoDongNum;RaoDongNum;3;0;Create;True;0;0;0;False;0;False;0.05326087;0.1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;-1635.566,32.31199;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1051.197,164.7997;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2020.662,49.59249;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;46;-378.0638,5.077355;Float;False;True;-1;2;ASEMaterialInspector;100;5;Yyz_WaveOfWater;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SaturateNode;63;-556.6272,9.968781;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-813.1727,7.983468;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1202.734,214.1827;Inherit;False;Property;_LightNum;LightNum;6;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1398.354,8.22197;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;ad274256a28ef5047b2e37e41d433796;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2228.555,-148.6037;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;65;-2397.334,-179.5419;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2495.412,-110.0052;Inherit;False;Property;_JianBianNum;JianBianNum;5;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-2623.205,-287.9022;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
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
WireConnection;18;0;2;0
WireConnection;18;1;13;0
WireConnection;7;0;9;0
WireConnection;7;1;29;0
WireConnection;7;2;10;0
WireConnection;25;0;1;0
WireConnection;25;1;29;0
WireConnection;25;2;26;0
WireConnection;29;0;34;0
WireConnection;29;1;18;0
WireConnection;46;0;63;0
WireConnection;63;0;24;0
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;1;1;7;0
WireConnection;34;0;65;0
WireConnection;34;1;37;0
WireConnection;65;0;9;2
ASEEND*/
//CHKSM=7FBB36DAD661106D0C655080527988B358C1B1F3