Shader "Custom/BOTW-Rain"
{
	Properties
	{
		_MainTex("Camera Texture (leave none)", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_RainDepth("Rain Depth", float) = 1
		_RainDensity("Rain Density", float) = .5
		_RainEdgeHeight("Rain Edge Height", float) = .05
		_RainColor("Rain Color", color) = (1,1,1,.5)
		[Header(Droplets)]
		_RainTex("Raindrop Texture", 2D) = "white" {}
		_RotAngle("Rotation Angle", float) = 30
		_Speed("Speed", float) = 1
		_Attenuation("Attenuation", float) = 0
		_RainScale("Scale", Vector) = (1, 1, 1, 1)
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos: TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _RainTex;
			sampler2D _NoiseTex;
			sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;
			float4x4 _CameraMV;

			float _RainDepth;
			float _RainDensity;
			float _RainEdgeHeight;
			float4 _RainColor;

			float _RotAngle;
			float _Speed;
			float _Attenuation;
			
			float Pi = 3.14;
			float2 _RainScale;

			fixed4 frag (v2f i) : SV_Target
			{
			
				
				// // drops
				// float2 speed = float2(1, 1);
				// float2 scale = float2(1, 4);
				// float4 ScalesLayer12 = float4(1, 1, 1, 1);
				// float2 UV = i.scrPos.xy;
				// float2 SinT = sin(_Time.xy * 2.0f * Pi / speed.xy) * scale.xy;
				// // rotate and scale UV
				// float4 Cosines = float4(cos(SinT), sin(SinT));
				// float2 CenteredUV = UV - float2(0.5f, 0.5f);
				// float4 RotatedUV = float4(dot(Cosines.xz*float2(1,-1), CenteredUV)
				//                          , dot(Cosines.zx, CenteredUV)
				//                          , dot(Cosines.yw*float2(1,-1), CenteredUV)
				//                          , dot(Cosines.wy, CenteredUV) ) + 0.5f;
				// float4 UVLayer12 = ScalesLayer12 * RotatedUV.xyzw;
				//
				// // return float4(sin());
				// // return float4(1, 1, 1, 1);
				// // return ScalesLayer12;
				// // return float4(CenteredUV, 1, 1);
				// return RotatedUV;
				//
				// splashes
				float3 normalValues1;
				float depthValue1;
				float3 normalValues2;
				float depthValue2;
				
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy), depthValue1, normalValues1);
				float3 worldNormal = mul((float3x3)_CameraMV, normalValues1);
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, (i.scrPos.xy - float2(0, _RainEdgeHeight * (1.1-depthValue1) * (1 -worldNormal.g)))), depthValue2, normalValues2);
				
				fixed4 noise = tex2D(_NoiseTex, (i.uv * (depthValue2 + 2)) + round(_Time[0] * 750) * .1f);
				fixed4 col = tex2D(_MainTex, i.uv);
				
				// return float4(depthValue1, depthValue2, 0, 0);
				if (depthValue1 > depthValue2 * _RainDepth && worldNormal.g > .5f) {
					col += step(noise.r, _RainDensity) * _RainColor;
				}

				
				// texture
				float sinX = sin ( _RotAngle );
	            float cosX = cos ( _RotAngle );
	            float sinY = sin ( _RotAngle );
	            float2x2 rot = float2x2( cosX, -sinX, sinY, cosX);
				float2 uv = mul(i.scrPos.xy, rot);
				uv.y +=  _Time.y * _Speed;
				float4 rainTex00 = tex2D(_RainTex, uv * _RainScale);
				rainTex00 -= _Attenuation;
				// rainTex00 *= float4(1, 0, 0, 0);


				float4 rainTex01 = tex2D(_RainTex, uv * _RainScale);
				rainTex01 -= _Attenuation;
				// rainTex01 *= float4(0, 1, 0, 0);

				float4 rainTex = max(rainTex00, rainTex01);
				return max(rainTex, col);
			}
			ENDCG
		}
	}
}
