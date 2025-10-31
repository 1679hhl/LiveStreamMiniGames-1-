// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yyz/PotrolUV_003"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_Speed("Speed", Float) = 0.2
		[IntRange]_Count("Count", Range( 1 , 10)) = 3
		_Min_U("Min_U", Range( 0 , 1)) = 0.4
		_Length("Length", Range( 0 , 1)) = 0.3
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.35
		_Light("Light", Float) = 7.03

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent+1" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
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
			uniform float _Speed;
			uniform float _Count;
			uniform float _Smoothness;
			uniform float _Length;
			uniform float _Min_U;
			uniform float4 _Color;
			uniform float _Light;

			
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
				float2 texCoord1 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float2 temp_cast_0 = (_Length).xx;
				float smoothstepResult24 = smoothstep( 0.0 , _Smoothness , length( saturate( ( abs( texCoord1 ) - temp_cast_0 ) ) ));
				float temp_output_26_0 = (0.0 + (( 1.0 - smoothstepResult24 ) - _Min_U) * (1.0 - 0.0) / (( 1.0 - _Min_U ) - _Min_U));
				float2 appendResult11 = (float2(frac( ( frac( ( (( atan2( texCoord1.x , texCoord1.y ) / UNITY_PI )*0.5 + 0.5) + frac( ( _Time.y * _Speed ) ) ) ) * _Count ) ) , temp_output_26_0));
				float4 tex2DNode12 = tex2D( _MainTex, appendResult11 );
				float4 appendResult42 = (float4(( float4( (tex2DNode12).rgb , 0.0 ) * (_Color).rgba ).xyz , ( saturate( ( ( 1.0 - temp_output_26_0 ) * temp_output_26_0 * _Light ) ) * tex2DNode12.r * _Light )));
				
				
				finalColor = appendResult42;
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
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1116.134,-89.79652;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-861.114,-66.81981;Inherit;False;Property;_Length;Length;5;0;Create;True;0;0;0;False;0;False;0.3;0.42;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;50;-787.1368,-276.6919;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;51;-587.3099,-276.793;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-257.9526,237.4084;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;2;-693.2001,10.19999;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-238.9526,307.4084;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;4;-671.2001,221.2001;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;52;-379.9598,-276.7107;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-415.8582,229.5528;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;3;-473.0043,9.991536;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-95.95282,238.4084;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-317.7544,-67.41993;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;0;False;0;False;0.35;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;5;-214.6827,-277.3141;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;24;-50.89037,-210.8703;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-8.955439,-0.348747;Inherit;False;Property;_Min_U;Min_U;4;0;Create;True;0;0;0;False;0;False;0.4;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;6;-248.2075,11.05275;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;18;31.72032,238.5872;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;158.174,214.6588;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;253.7873,77.30912;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;193.7998,-209.8544;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;20;370.2342,213.647;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;278.0041,431.0808;Inherit;False;Property;_Count;Count;3;1;[IntRange];Create;True;0;0;0;False;0;False;3;2;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;26;378.414,-209.8158;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;547.9335,213.6409;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;23;762.8265,212.3441;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;49;811.1179,-14.68378;Inherit;False;Property;_Keyword0;反向;8;0;Create;False;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;829.4614,-139.3886;Inherit;False;Property;_Light;Light;7;0;Create;True;0;0;0;False;0;False;7.03;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;1119.526,46.42112;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;29;799.4312,-238.0221;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;1473.657,241.3897;Inherit;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;5.716741,2.352292,0.7280754,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;1337.436,17.84153;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;None;c04b3f1107b0a734ba42847cedd40654;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;35;1684.917,32.8044;Inherit;False;FLOAT3;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;34;1328.846,-235.2778;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;37;1688.657,243.3897;Inherit;False;FLOAT4;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1896.849,129.8096;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;1763.924,-232.2534;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;2161.359,-253.1703;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2539.489,-250.1573;Float;False;True;-1;2;ASEMaterialInspector;100;5;Yyz/PotrolUV_003;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;True;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=1;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.OneMinusNode;47;632.1179,41.31622;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;1007.219,-234.7621;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
WireConnection;50;0;1;0
WireConnection;51;0;50;0
WireConnection;51;1;25;0
WireConnection;2;0;1;1
WireConnection;2;1;1;2
WireConnection;52;0;51;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;5;0;52;0
WireConnection;24;0;5;0
WireConnection;24;2;53;0
WireConnection;6;0;3;0
WireConnection;6;1;7;0
WireConnection;6;2;7;0
WireConnection;18;0;17;0
WireConnection;19;0;6;0
WireConnection;19;1;18;0
WireConnection;28;0;27;0
WireConnection;8;0;24;0
WireConnection;20;0;19;0
WireConnection;26;0;8;0
WireConnection;26;1;27;0
WireConnection;26;2;28;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;23;0;21;0
WireConnection;49;1;26;0
WireConnection;49;0;47;0
WireConnection;11;0;23;0
WireConnection;11;1;26;0
WireConnection;29;0;26;0
WireConnection;12;1;11;0
WireConnection;35;0;12;0
WireConnection;34;0;32;0
WireConnection;37;0;36;0
WireConnection;38;0;35;0
WireConnection;38;1;37;0
WireConnection;39;0;34;0
WireConnection;39;1;12;1
WireConnection;39;2;33;0
WireConnection;42;0;38;0
WireConnection;42;3;39;0
WireConnection;0;0;42;0
WireConnection;47;0;26;0
WireConnection;32;0;29;0
WireConnection;32;1;26;0
WireConnection;32;2;33;0
ASEEND*/
//CHKSM=1CE330E17EA8D44674574D30B3892E4ABD9E8E21