Shader "LowPolyWater/MyWaterShadedGeometry" {
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

    _WaveOrigin ("Wave origin", Vector) = (0.0, 0.0, 0.0, 0.0)
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
        float3 tangent : TANGENT;
	};
	
	struct v2g
	{
		float4 pos : SV_POSITION;
        half3 offsets : TEXCOORD0;
	};

    struct g2f 
    {
        float4 pos : SV_POSITION;
        float3 normalDir : TEXCOORD5;
        float4 viewInterpolator : TEXCOORD1;
        float4 bumpCoords : TEXCOORD2;
        float4 screenPos : TEXCOORD3;
        float4 posWorld : TEXCOORD4;

        UNITY_FOG_COORDS(0)
    };
 
	inline half4 Foam(sampler2D shoreTex, half4 coords) 
 	{
		half4 foam = (tex2D(shoreTex, coords.xy) * tex2D(shoreTex,coords.zw)) - 0.125;
		return foam;
	}

    inline float3 getDisplacedPosition(float3 position, out half3 offsets) 
    {
        const float PI2 = 6.28318;
        half dist = (distance(position, _WaveOrigin) % _WaveLength) / _WaveLength;

        offsets = half3(0,0,0);
        offsets.y = _WaveHeight * sin(_Time.y * PI2 * _WaveFrequency + dist * PI2);

        return position + offsets;
    }

	v2g vert(appdata v)
	{
		v2g o;
		UNITY_INITIALIZE_OUTPUT(v2g, o);
        
        half3 offsets = half3(0,0,0);
		v.vertex.xyz = getDisplacedPosition(v.vertex.xyz, offsets);

        o.pos     = v.vertex;
        o.offsets = offsets;

		return o;
	}

    inline void fillg2f(v2g i, inout g2f o) {

        half2 tileableUv = mul(unity_ObjectToWorld, i.pos).xz;
        o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;

        half3 worldSpaceVertex = mul(unity_ObjectToWorld, i.pos).xyz;
        o.viewInterpolator.xyz = worldSpaceVertex - _WorldSpaceCameraPos;
        o.pos = UnityObjectToClipPos(i.pos);
        o.screenPos = ComputeScreenPos(o.pos);
        o.viewInterpolator.w = saturate(i.offsets.y);

        UNITY_TRANSFER_FOG(o, o.pos);
        o.posWorld = mul(unity_ObjectToWorld, i.pos);
        //o.normalDir = normalize(mul(float4(i.normal, 0.0), modelMatrixInverse).xyz);
    }
    
    [maxvertexcount(3)]
    void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
    {
        float3 v0 = IN[0].pos.xyz;
        float3 v1 = IN[1].pos.xyz;
        float3 v2 = IN[2].pos.xyz;

        float3 centerPos = (v0 + v1 + v2) / 3.0;
        float3 vn = normalize(cross(v1 - v0, v2 - v0));

        g2f OUT;
        OUT.normalDir = vn;
        fillg2f(IN[0], OUT);
        //OUT.pos = mul(UNITY_MATRIX_MVP, IN[0].pos);
        triStream.Append(OUT);

        OUT.normalDir = vn;
        fillg2f(IN[1], OUT);
        //OUT.pos = mul(UNITY_MATRIX_MVP, IN[1].pos);
        triStream.Append(OUT);

        OUT.normalDir = vn;
        fillg2f(IN[2], OUT);
        //OUT.pos = mul(UNITY_MATRIX_MVP, IN[2].pos);
        triStream.Append(OUT);
    }
 
	half4 calculateBaseColor(g2f input)  
    {
        float3 normalDirection = normalize(input.normalDir);

        float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
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

        float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _BaseColor.rgb;

        float3 diffuseReflection = 
           attenuation * _LightColor0.rgb * _BaseColor.rgb
           * max(0.0, dot(normalDirection, lightDirection));

        float3 specularReflection;
        if (dot(normalDirection, lightDirection) < 0.0) // light source on the wrong side?
        {
           specularReflection = float3(0.0, 0.0, 0.0); // no specular reflection
        }
        else  
        {
           specularReflection = attenuation * _LightColor0.rgb  * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
        }

        return half4(ambientLighting + diffuseReflection  + specularReflection, 1.0);
    }

	half4 frag(g2f i) : SV_Target
	{ 
 
		half4 edgeBlendFactors = half4(1.0, 0.0, 0.0, 0.0);
		
		#ifdef WATER_EDGEBLEND_ON
			half depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
			depth = LinearEyeDepth(depth);
			edgeBlendFactors = saturate(_InvFadeParemeter * (depth - i.screenPos.w));
			edgeBlendFactors.y = 1.0 - edgeBlendFactors.y;
		#endif
        
        half4 baseColor = calculateBaseColor(i);
 
		half4 foam = Foam(_ShoreTex, i.bumpCoords * 2.0);
		baseColor.rgb += foam.rgb * _Foam.x * (edgeBlendFactors.y + saturate(i.viewInterpolator.w - _Foam.y));
		if(_isInnerAlphaBlendOrColor == 0)
			baseColor.rgb += 1.0 - edgeBlendFactors.x;
            
		if(_isInnerAlphaBlendOrColor == 1.0)
			baseColor.a = edgeBlendFactors.x;
            
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
			ZWrite Off
			Cull Off
		
			CGPROGRAM
		
			#pragma target 4.0
		
			#pragma vertex vert
            #pragma geometry geom
			#pragma fragment frag
			#pragma multi_compile_fog
		
			#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF 
		
			ENDCG
	}
}


Fallback "Transparent/Diffuse"
}
