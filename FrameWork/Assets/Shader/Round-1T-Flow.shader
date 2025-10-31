// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yyz/UI/Round-1T-Flow"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[HDR]_MainColor("MainColor", Color) = (1,1,1,0)
		_ColorSpeed("变色速度", Float) = 0.5
		_Speed("Speed", Range( -1 , 1)) = 0
		_TailMax("Tail-Max", Range( 0 , 0.9)) = 0.9
		_RoundMin("RoundMin", Range( 0 , 0.2)) = 0.2
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Rotator("Rotator", Float) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One
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

			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _Rotator;
			uniform float _ColorSpeed;
			uniform float4 _MainColor;
			uniform float _RoundMin;
			uniform sampler2D _MainTex;
			uniform float _Speed;
			uniform float _TailMax;
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			
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
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float cos359 = cos( ( (0.0 + (_Rotator - 0.0) * (6.28 - 0.0) / (1.0 - 0.0)) / 360.0 ) );
				float sin359 = sin( ( (0.0 + (_Rotator - 0.0) * (6.28 - 0.0) / (1.0 - 0.0)) / 360.0 ) );
				float2 rotator359 = mul( uv_TextureSample0 - float2( 0.5,0.5 ) , float2x2( cos359 , -sin359 , sin359 , cos359 )) + float2( 0.5,0.5 );
				float2 panner355 = ( 1.0 * _Time.y * float2( 0,0.2 ) + rotator359);
				float mulTime347 = _Time.y * _ColorSpeed;
				float3 hsvTorgb346 = RGBToHSV( _MainColor.rgb );
				float3 hsvTorgb349 = HSVToRGB( float3(( mulTime347 + hsvTorgb346.x ),hsvTorgb346.y,hsvTorgb346.z) );
				float2 texCoord235 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_243_0 = ( abs( ( texCoord235 + -0.5 ) ) + -0.4 );
				float temp_output_292_0 = saturate( (0.0 + (( saturate( length( saturate( temp_output_243_0 ) ) ) / 0.5 ) - _RoundMin) * (1.0 - 0.0) / (0.2588236 - _RoundMin)) );
				float2 texCoord266 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break270 = ( texCoord266 + -0.5 );
				float mulTime279 = _Time.y * _Speed;
				float2 appendResult298 = (float2(saturate( (0.0 + (frac( ( ( atan2( break270.x , break270.y ) / 6.28318548202515 ) + mulTime279 ) ) - _TailMax) * (1.0 - 0.0) / (1.0 - _TailMax)) ) , temp_output_292_0));
				float4 tex2DNode288 = tex2D( _MainTex, appendResult298 );
				
				
				finalColor = ( ( tex2D( _TextureSample0, panner355 ) + float4( hsvTorgb349 , 0.0 ) ) * ( ( ( fwidth( temp_output_292_0 ) * 43.78 ) * tex2DNode288 ) * saturate( ( ( tex2DNode288.a + 0.0 ) / 1.0 ) ) ) );
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
Node;AmplifyShaderEditor.CommentaryNode;265;6837.658,-549.3663;Inherit;False;2057.028;805.2681;Comment;13;239;238;253;255;249;245;247;243;244;242;236;235;237;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;237;6923.107,-31.99148;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;308;7061.046,277.8725;Inherit;False;2720.512;541.4301;Comment;15;307;303;284;285;280;278;276;279;269;277;286;270;267;268;266;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;235;6887.658,-340.0044;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;236;7180.19,-283.7085;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;268;7171.966,604.7412;Inherit;False;Constant;_Float3;Float 3;3;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;266;7111.046,327.8725;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;267;7474.966,421.7411;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;242;7423.167,-246.0678;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;244;7343.068,27.83198;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;-0.4;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;270;7687.966,427.7411;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;243;7668.642,-98.83654;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TauNode;277;8067.714,707.4675;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;286;8235.104,691.4779;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;0;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;247;7903.22,-92.55893;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ATan2OpNode;269;7967.444,427.27;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;276;8278.744,425.7737;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;245;8081.163,-106.7482;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;279;8574.924,695.4138;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;320;8938.483,-150.9851;Inherit;False;1088.134;383.5109;Comment;6;327;331;292;289;326;291;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;278;8771.354,434.6736;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;255;8388.748,110.1918;Inherit;False;Constant;_Round;Round;3;0;Create;True;0;0;0;False;0;False;0.5;5;0;12;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;249;8276.789,-121.0861;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;253;8676.313,-75.13062;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;280;9082.755,463.269;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;285;8972.194,700.5179;Inherit;False;Property;_TailMax;Tail-Max;4;0;Create;True;0;0;0;False;0;False;0.9;0;0;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;291;8948.429,27.79356;Inherit;False;Constant;_MainUVMax;MainUV-Max;6;0;Create;True;0;0;0;False;0;False;0.2588236;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;326;8943.448,-54.55214;Inherit;False;Property;_RoundMin;RoundMin;5;0;Create;True;0;0;0;False;0;False;0.2;0.075;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;289;9341.783,-108.9852;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;284;9347.83,488.7819;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;292;9604.592,-109.4018;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;319;9833.053,267.7564;Inherit;False;1214.045;499.7879;Comment;7;332;318;316;322;317;288;298;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;303;9640.523,486.469;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;298;9862.256,324.5969;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;288;10105.17,317.7564;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;9556b6acf6ac0b541942fefaec6624b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FWidthOpNode;327;9803.611,-108.8773;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;317;10446.41,652.6057;Inherit;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;322;10414.02,425.9175;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;331;9857.905,125.6306;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;43.78;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;316;10652.47,427.5137;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;332;10419.8,306.4647;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;239;8253.651,-499.3663;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;307;9338.362,707.7039;Inherit;False;Constant;_Float6;Float 6;7;0;Create;True;0;0;0;False;0;False;0.7176471;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;238;7971.223,-438.3787;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;318;10871.8,426.1399;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;323;11176.41,302.5547;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;10068.41,18.93333;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;345;10695.42,7.554534;Inherit;False;Property;_ColorSpeed;变色速度;2;0;Create;False;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;346;10814.47,84.80352;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;347;10853.7,12.83847;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;348;11033.13,83.52544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;349;11166.97,108.3035;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;325;10603.9,84.2793;Inherit;False;Property;_MainColor;MainColor;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;343;12162.15,295.9808;Float;False;True;-1;2;ASEMaterialInspector;100;5;Yyz/UI/Round-1T-Flow;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;4;1;False;;1;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;353;11520.42,108.6524;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;11768.09,270.9654;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;355;10949.02,-287.3853;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;350;11127.54,-310.8119;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;6f6cbb95a039b44438ff6bdea951b09a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;359;10705.08,-287.0252;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;354;10435.99,-286.5934;Inherit;False;0;350;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;357;10360.87,-162.7756;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;6.28;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;358;10536.87,-162.7756;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;356;10179.68,-161.7558;Inherit;False;Property;_Rotator;Rotator;7;0;Create;True;0;0;0;False;0;False;0;-45;0;0;0;1;FLOAT;0
WireConnection;236;0;235;0
WireConnection;236;1;237;0
WireConnection;267;0;266;0
WireConnection;267;1;268;0
WireConnection;242;0;236;0
WireConnection;270;0;267;0
WireConnection;243;0;242;0
WireConnection;243;1;244;0
WireConnection;247;0;243;0
WireConnection;269;0;270;0
WireConnection;269;1;270;1
WireConnection;276;0;269;0
WireConnection;276;1;277;0
WireConnection;245;0;247;0
WireConnection;279;0;286;0
WireConnection;278;0;276;0
WireConnection;278;1;279;0
WireConnection;249;0;245;0
WireConnection;253;0;249;0
WireConnection;253;1;255;0
WireConnection;280;0;278;0
WireConnection;289;0;253;0
WireConnection;289;1;326;0
WireConnection;289;2;291;0
WireConnection;284;0;280;0
WireConnection;284;1;285;0
WireConnection;292;0;289;0
WireConnection;303;0;284;0
WireConnection;298;0;303;0
WireConnection;298;1;292;0
WireConnection;288;1;298;0
WireConnection;327;0;292;0
WireConnection;322;0;288;4
WireConnection;316;0;322;0
WireConnection;316;1;317;0
WireConnection;332;0;330;0
WireConnection;332;1;288;0
WireConnection;239;0;238;0
WireConnection;238;0;243;0
WireConnection;318;0;316;0
WireConnection;323;0;332;0
WireConnection;323;1;318;0
WireConnection;330;0;327;0
WireConnection;330;1;331;0
WireConnection;346;0;325;0
WireConnection;347;0;345;0
WireConnection;348;0;347;0
WireConnection;348;1;346;1
WireConnection;349;0;348;0
WireConnection;349;1;346;2
WireConnection;349;2;346;3
WireConnection;343;0;324;0
WireConnection;353;0;350;0
WireConnection;353;1;349;0
WireConnection;324;0;353;0
WireConnection;324;1;323;0
WireConnection;355;0;359;0
WireConnection;350;1;355;0
WireConnection;359;0;354;0
WireConnection;359;2;358;0
WireConnection;357;0;356;0
WireConnection;358;0;357;0
ASEEND*/
//CHKSM=92CEEE5248388C522B3FC0E4166E4DC374BE0674