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
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert

        struct Input
        {
            float3 worldPos;
            float3 viewDir;
        };

        fixed4 _WaterColor;
        fixed4 _ShallowColor;
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

            fixed3 color = lerp(_ShallowColor.rgb, _WaterColor.rgb, depthFactor);

           
            float fresnel = pow(1 - dot(normalize(IN.viewDir), o.Normal), _FresnelPower);

            color += fresnel;

            o.Albedo = color;
            o.Smoothness = 0.8;
            o.Metallic = 0;
        }
        ENDCG
    }

    FallBack "Diffuse"
}