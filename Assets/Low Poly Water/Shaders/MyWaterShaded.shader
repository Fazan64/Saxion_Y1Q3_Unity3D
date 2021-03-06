Shader "LowPolyWater/MyWaterShaded" {
Properties { 

	_BaseColor ("Base color", COLOR)  = ( .54, .95, .99, 0.5) 
	_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
    _Shininess ("Shininess", Float) = 10
	_ShoreTex ("Shore & Foam texture ", 2D) = "black" {} 
	 
	_InvFadeParemeter ("Auto blend parameter (Edge, Shore, Distance scale)", Vector) = (0.2 ,0.39, 0.5, 1.0)

	_BumpTiling ("Foam Tiling", Vector) = (1.0 ,1.0, -2.0, 3.0)
	_BumpDirection ("Foam movement", Vector) = (1.0 ,1.0, -1.0, 1.0) 

	_Foam ("Foam (intensity, cutoff)", Vector) = (0.1, 0.375, 0.0, 0.0) 
	[MaterialToggle] _isInnerAlphaBlendOrColor("Fade inner to color or alpha?", Float) = 0 

    _WaveOrigin ("Wave origin", Vector) = (0.0, 0.0, 0.0)
    _WaveLength ("Wave length", Float) = 0.75
    _WaveFrequency ("Wave frequency", Float) = 0.5
    _WaveHeight ("Wave height", Float) = 0.5
}


CGINCLUDE 

	#include "UnityCG.cginc" 
	#include "UnityLightingCommon.cginc" // for _LightColor0


	sampler2D _ShoreTex;
	sampler2D_float _CameraDepthTexture;
  
	uniform float4 _BaseColor;  
    uniform float _Shininess;
	 
	uniform float4 _InvFadeParemeter;
    
	uniform float4 _BumpTiling;
	uniform float4 _BumpDirection;
 
	uniform float4 _Foam; 
  	float _isInnerAlphaBlendOrColor; 

    uniform float3 _WaveOrigin;
    uniform float  _WaveLength;
    uniform float  _WaveFrequency;
    uniform float  _WaveHeight;

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
        float3 vertex1 : TEXCOORD1;
        float3 vertex2 : TEXCOORD2;
	};
 
	struct v2f
	{
		float4 pos : SV_POSITION;
        float3 normalDir : NORMAL;

		float4 bumpCoords : TEXCOORD0;
		float4 screenPos : TEXCOORD1;
		float4 posWorld : TEXCOORD2;

		UNITY_FOG_COORDS(3)
	}; 
 
	inline float4 Foam(sampler2D shoreTex, half4 coords) 
	{
		float4 foam = (tex2D(shoreTex, coords.xy) * tex2D(shoreTex,coords.zw)) - 0.125;
		return foam;
	}

    inline float3 getDisplacedPosition(float3 position) 
    {
        const float PI2 = 6.28318;
        float dist = fmod(distance(position, _WaveOrigin), _WaveLength) / _WaveLength;
        position.y += _WaveHeight * sin(_Time.y * PI2 * _WaveFrequency + dist * PI2);
        return position;
    }

	v2f vert(appdata v)
	{
		v2f o;
		UNITY_INITIALIZE_OUTPUT(v2f, o);

        v.vertex.xyz = getDisplacedPosition(v.vertex.xyz);

        v.vertex1 = getDisplacedPosition(v.vertex1);
        v.vertex2 = getDisplacedPosition(v.vertex2);
        v.normal = cross(v.vertex1 - v.vertex.xyz, v.vertex2 - v.vertex.xyz);
		 
		half2 tileableUv = mul(unity_ObjectToWorld, v.vertex).xz;
		o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;
        
		o.pos = UnityObjectToClipPos(v.vertex);
		o.screenPos = ComputeScreenPos(o.pos); 
	 	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); 
        UNITY_TRANSFER_FOG(o, o.pos);

		return o;
	}
 
	float4 calculateBaseColor(v2f input)  
    {
        float3 normalDirection = normalize(input.normalDir);

        float3 viewDirection = normalize(
           _WorldSpaceCameraPos - input.posWorld.xyz);
        float3 lightDirection;
        float attenuation;

        if (0.0 == _WorldSpaceLightPos0.w) // directional light?
        {
           attenuation = 1.0; // no attenuation
           lightDirection = normalize(_WorldSpaceLightPos0.xyz);
        } 
        else // point or spot light
        {
           float3 vertexToLightSource = 
              _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
           float distance = length(vertexToLightSource);
           attenuation = 1.0 / distance; // linear attenuation 
           lightDirection = normalize(vertexToLightSource);
        }

        float3 ambientLighting = 
           UNITY_LIGHTMODEL_AMBIENT.rgb * _BaseColor.rgb;

        float3 diffuseReflection = 
           attenuation * _LightColor0.rgb * _BaseColor.rgb
           * max(0.0, dot(normalDirection, lightDirection));

        float3 specularReflection;
        if (dot(normalDirection, lightDirection) < 0.0) 
           // light source on the wrong side?
        {
           specularReflection = float3(0.0, 0.0, 0.0); 
              // no specular reflection
        }
        else  
        {
           specularReflection = attenuation * _LightColor0.rgb  * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
        }

        return float4(ambientLighting + diffuseReflection  + specularReflection, 1.0);
    }

	half4 frag( v2f i ) : SV_Target
	{
		float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
		
		#ifdef WATER_EDGEBLEND_ON
			float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
			depth = LinearEyeDepth(depth);
			edgeBlendFactors = saturate(_InvFadeParemeter * (depth - i.screenPos.w));
			edgeBlendFactors.y = 1.0 - edgeBlendFactors.y;
		#endif

        float4 baseColor = calculateBaseColor(i);
 
		float4 foam = Foam(_ShoreTex, i.bumpCoords * 2.0);
		baseColor.rgb += foam.rgb * _Foam.x * (edgeBlendFactors.y + saturate(1-_Foam.y));

        if(_isInnerAlphaBlendOrColor == 0)
			baseColor.rgb += 1.0-edgeBlendFactors.x;
		if(_isInnerAlphaBlendOrColor == 1.0)
			baseColor.a =  edgeBlendFactors.x;
            
		UNITY_APPLY_FOG(i.fogCoord, baseColor);
		return baseColor;
	}
	
ENDCG

Subshader
{
	Tags {"RenderType"="Transparent" "Queue"="Transparent"}
	
	Lod 500
	ColorMask RGB
	
	GrabPass { "_RefractionTex" }
	
	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
		
			CGPROGRAM
		
			#pragma target 3.0
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
		
			#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF 
		
			ENDCG
	}
}


Fallback "Transparent/Diffuse"
}
