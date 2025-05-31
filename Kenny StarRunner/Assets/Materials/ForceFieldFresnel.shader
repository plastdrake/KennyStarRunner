Shader "Custom/ForceFieldFresnelRippleWorld"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.2,0.8,1,0.3)
        _FresnelColor ("Fresnel Color", Color) = (0.5,1,1,0.7)
        _FresnelPower ("Fresnel Power", Range(1,8)) = 4
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.5
        _NoiseSpeed ("Noise Scroll Speed", Vector) = (0.5, 0.5, 0, 0)
        _RippleStrength ("Ripple Strength", Range(0,0.2)) = 0.08
        _RippleFrequency ("Ripple Frequency", Range(1,20)) = 8
        _RippleSpeed ("Ripple Speed", Range(0,10)) = 2
        _DistortTex ("Distort Noise (optional)", 2D) = "gray" {}
        _DistortStrength ("Distort Strength", Range(0,0.2)) = 0.08
        _DistortSpeed ("Distort Speed", Range(0,5)) = 1
        _WorldScale ("World Effect Scale", Float) = 1
        _WorldRotation ("World Effect Rotation (Euler XYZ)", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            sampler2D _DistortTex;
            float4 _DistortTex_ST;
            float4 _Color;
            float4 _FresnelColor;
            float _FresnelPower;
            float _NoiseStrength;
            float4 _NoiseSpeed;
            float _RippleStrength;
            float _RippleFrequency;
            float _RippleSpeed;
            float _DistortStrength;
            float _DistortSpeed;
            float _WorldScale;
            float4 _WorldRotation; // Euler angles in degrees

            // Helper: Euler XYZ to rotation matrix
            float3x3 EulerToMatrix(float3 euler)
            {
                float3 rad = radians(euler);
                float cx = cos(rad.x), sx = sin(rad.x);
                float cy = cos(rad.y), sy = sin(rad.y);
                float cz = cos(rad.z), sz = sin(rad.z);

                float3x3 mx = float3x3(1,0,0, 0,cx,-sx, 0,sx,cx);
                float3x3 my = float3x3(cy,0,sy, 0,1,0, -sy,0,cy);
                float3x3 mz = float3x3(cz,-sz,0, sz,cz,0, 0,0,1);

                return mul(mz, mul(my, mx));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPos = worldPos;
                o.worldViewDir = _WorldSpaceCameraPos - worldPos;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Apply world rotation to world position
                float3x3 rot = EulerToMatrix(_WorldRotation.xyz);
                float3 wp = mul(rot, i.worldPos) * _WorldScale;

                // Ripple effect: animate with sine wave in world space
                float ripple = sin((wp.x + wp.y + wp.z + _Time.y * _RippleSpeed) * _RippleFrequency) * _RippleStrength;

                // Optional: add distortion from a second noise texture in world space
                float2 distortUV = (wp.xy + _Time.y * _DistortSpeed);
                float distort = tex2D(_DistortTex, distortUV).r * 2.0 - 1.0; // -1 to 1

                // Use world position for noise UVs
                float2 noiseUV = (wp.xy + ripple + distort * _DistortStrength) + _Time.y * _NoiseSpeed.xy;
                float noise = tex2D(_NoiseTex, noiseUV).r;

                // Fresnel term
                float3 n = normalize(i.worldNormal);
                float3 v = normalize(i.worldViewDir);
                float fresnel = pow(1.0 - saturate(dot(n, v)), _FresnelPower);

                // Combine fresnel and noise
                float edge = fresnel + noise * _NoiseStrength;

                // Final color
                float alpha = saturate(edge) * _Color.a;
                float3 color = lerp(_Color.rgb, _FresnelColor.rgb, edge);

                return float4(color, alpha);
            }
            ENDCG
        }
    }
}
