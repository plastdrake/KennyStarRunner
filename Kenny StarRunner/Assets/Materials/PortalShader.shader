Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Portal Color", Color) = (0.3,0.7,1,1)
        _Distortion ("Distortion", Range(0,3)) = 0.4
        _Speed ("Swirl Speed", Range(0,20)) = 1.5
        _EdgeSoftness ("Edge Softness", Range(0.01,0.5)) = 0.25
        _Radius ("Portal Radius", Range(0.5,2.0)) = 1.0
        _RippleStrength ("Ripple Strength", Range(0,0.5)) = 0.1
        _RippleFrequency ("Ripple Frequency", Range(1,20)) = 8
        _RippleSpeed ("Ripple Speed", Range(0,10)) = 2
        _GlowStrength ("Glow Strength", Range(0,2)) = 0.7
        _GlowWidth ("Glow Width", Range(0.01,0.5)) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Distortion;
            float _Speed;
            float _EdgeSoftness;
            float _Radius;
            float _RippleStrength;
            float _RippleFrequency;
            float _RippleSpeed;
            float _GlowStrength;
            float _GlowWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                float radius = length(uv) * 2.0;

                float angle = atan2(uv.y, uv.x);

                // Swirl effect
                float swirlAmount = _Distortion * (1.0 - radius / _Radius);
                float swirl = angle + (_Time.x * _Speed) + swirlAmount * sin(_Time.x * 2.0 + radius * 8.0);

                // Ripple effect (radial, animated)
                float ripple = sin(radius * _RippleFrequency - _Time.x * _RippleSpeed) * _RippleStrength;

                // Swirl the UVs and add ripple to radius
                float r = radius + ripple;
                float2 swirlUV;
                swirlUV.x = cos(swirl) * r;
                swirlUV.y = sin(swirl) * r;
                swirlUV += 0.5;

                // Sample texture and modulate with portal color
                float4 texCol = tex2D(_MainTex, swirlUV);

                // Animated color bands, modulated by portal color
                float band = 0.5 + 0.5 * sin(8.0 * swirl + _Time.x * 2.0 + ripple * 10.0);
                float3 bandColor = lerp(_Color.rgb, float3(1,1,1), band * 0.5);

                // Final portal color: modulate bandColor with portal color and texture
                float3 portalColor = bandColor * _Color.rgb * texCol.rgb;

                // Edge fade (soft outer edge)
                float alpha = 1.0 - smoothstep(_Radius - _EdgeSoftness, _Radius, radius);

                // Glow: soft halo outside the portal
                float glow = 1.0 - smoothstep(_Radius, _Radius + _GlowWidth, radius);
                float3 glowColor = _Color.rgb * _GlowStrength;

                // Combine portal and glow
                float3 finalColor = portalColor;
                float finalAlpha = alpha * _Color.a * texCol.a;

                // Add glow outside the portal
                finalColor = lerp(finalColor, glowColor, glow * (1.0 - alpha));
                finalAlpha = max(finalAlpha, glow * _GlowStrength * 0.5);

                // Fully transparent outside glow region
                if (radius > _Radius + _GlowWidth)
                {
                    finalAlpha = 0.0;
                }

                return float4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
}
