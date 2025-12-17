Shader "Custom/WaveShader"
{
    Properties
    {
        [MainColor]_BaseColor("Base Color", Color) = (1,1,1,1)
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        _Amplitude("Wave Amplitude", Range(0,1)) = 0.2
        _Frequency("Wave Frequency", Range(0,10)) = 2.0
        _Speed("Wave Speed", Range(0,10)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags{ "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                half _Amplitude;
                half _Frequency;
                half _Speed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // 顶点波浪位移 (对象空间)
                float wave = sin( (IN.positionOS.x + IN.positionOS.z) * _Frequency + _Time.y * _Speed ) * _Amplitude;
                float3 displacedPos = IN.positionOS.xyz;
                displacedPos.y += wave;

                // 法线简单近似：对原法线进行扰动 (可选更复杂切线空间重建)
                float3 normalOS = IN.normalOS;

                OUT.positionHCS = TransformObjectToHClip(displacedPos);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 color = tex.rgb * _BaseColor.rgb;
                return half4(color, _BaseColor.a * tex.a);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
