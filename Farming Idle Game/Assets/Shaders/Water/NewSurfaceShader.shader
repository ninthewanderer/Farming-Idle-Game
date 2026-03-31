Shader "Custom/ToonWater"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (0,0.5,1,1)
        _ShallowColor ("Shallow Color", Color) = (0.2,0.7,1,1)
        _WaveStrength ("Wave Strength", Float) = 0.2
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _FresnelPower ("Fresnel Power", Float) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 200

        HLSLPROGRAM
        #pragma surface surf Standard vertex:vert

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Input
        {
            float3 worldPos;
            float3 viewDir;
        };

        struct appdata_full {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
            float4 texcoord3 : TEXCOORD3;
            half4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        half4 _WaterColor;
        half4 _ShallowColor;
        float _WaveStrength;
        float _WaveSpeed;
        float _FresnelPower;

        
        void vert (inout appdata_full v)
        {
            float wave = sin(v.vertex.x * 2 + _Time.y * _WaveSpeed) *
                         cos(v.vertex.z * 2 + _Time.y * _WaveSpeed);

            v.vertex.y += wave * _WaveStrength;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           
            float depthFactor = saturate(IN.worldPos.y);

            half3 color = lerp(_ShallowColor.rgb, _WaterColor.rgb, depthFactor);

           
            float fresnel = pow(1 - dot(normalize(IN.viewDir), o.Normal), _FresnelPower);

            color += fresnel;

            o.Albedo = color;
            o.Smoothness = 0.8;
            o.Metallic = 0;
        }
        ENDHLSL
    }

    FallBack "Diffuse"
}