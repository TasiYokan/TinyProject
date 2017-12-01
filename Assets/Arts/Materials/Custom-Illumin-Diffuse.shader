// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Legacy Shaders/Custom-Self-Illumin/Diffuse" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Illum ("Illumin (A)", 2D) = "white" {}
    _Emission ("Emission (Lightmapper)", Float) = 1.0
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _Illum;
fixed4 _Color;
fixed _Emission;

struct Input {
    float2 uv_MainTex;
    float2 uv_Illum;
};

half4 LightingOriginalCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) 
{
    half NdotL = dot(s.Normal, lightDir);
    if (NdotL <= 0.5) NdotL = 0.5;
    else NdotL = 1;
    half4 c;
    c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 0.5) * 2;
    c.a = s.Alpha;
    return c;
}

half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) 
{
    half NdotL = dot(s.Normal, lightDir);
    if (NdotL <= 0.5) NdotL = 0.5;
    else NdotL = 1;
    half4 c;
    if (atten <= 0.5) atten = 0.5;
    else atten = 1;
    c.rgb = s.Albedo * atten;//*_LightColor0.rgb * (NdotL * atten * 0.5) * 2;
    c.a = s.Alpha;
    return c;
}

half4 LightingNolight(SurfaceOutput s, half3 lightDir, half atten)
{
    half4 c;
    if (atten <= 0.5) atten = 0.25;
    else atten = 1;

    half NdotL = dot(s.Normal, lightDir);
    // if (NdotL <= 0.5) NdotL = 0.5;
    // else NdotL = 1;

    c.rgb = s.Albedo  * atten * 1;
    c.a = s.Alpha;
    return c;
}

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
    fixed4 c = _Color * tex;
    o.Albedo = c.rgb;
    // o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
#if defined (UNITY_PASS_META)
    o.Emission *= _Emission.rrr;
#endif
    o.Alpha = c.a;
}
ENDCG
}
FallBack "Legacy Shaders/Self-Illumin/VertexLit"
CustomEditor "LegacyIlluminShaderGUI"
}
