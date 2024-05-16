﻿Shader "ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
	// Ambient light is applied uniformly to all surfaces on the object.
	[HDR]
	_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
	[HDR]
	_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		// Controls the size of the specular reflection.
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		// Control how smoothly the rim blends when approaching unlit parts of the surface.

		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

			// Controls the Brightness of the shader.
        _Brightness("Brightness", Range(0, 2)) = 1.0
	}
		SubShader
		{
			Pass
			{
				// Use pass to only receive data on the main directional light and ambient light.
				Tags
				{
					"LightMode" = "ForwardBase"
					"PassFlags" = "OnlyDirectional"
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			// Files below include macros and functions to assist
			// with lighting and shadows.
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;

				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				// Direction from the current vertex towards the camera
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//Transforms the vertex from world space to shadow-map space.
				TRANSFER_SHADOW(o)
				return o;
			}

			float4 _Color;

			float4 _AmbientColor;

			float4 _SpecularColor;
			float _Glossiness;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;

			float _Brightness;

			float4 frag(v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);

				// Lighting below is calculated using Blinn-Phong, to create the toon look.
				
				// Dot product to calculate illumination from directional light.
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// Returns a value between 0 and 1, 0 being no shadows and 1 fully shadowed.
				float shadow = SHADOW_ATTENUATION(i);

				// Smooths the extreme light and dark surfaces to blend them together.
				// Multiply shadow to impliment shadows on the object if needed.
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

				// Main directional light's intensity and color.
				float4 light = lightIntensity * _LightColor0;

				// Specular reflection.
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				// Multiply Gloss by iteself to allow for smaller values to have larger effects.
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				//Smooth out and blend the "highlights" of the object.
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				// Rim lighting.
				float rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				// Rim only appears on lit side of object.
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return (light + _AmbientColor + specular + rim) * _Color * sample *  _Brightness;
			}
			ENDCG
		}

			// Shadow casting.
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}