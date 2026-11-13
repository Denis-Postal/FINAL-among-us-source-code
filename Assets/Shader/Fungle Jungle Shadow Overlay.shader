// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Fungle Jungle Shadow Overlay"
{
    Properties
    {
        _Color ("Color", Color) = (0,0.01176471,0.09411766,0.6)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Size ("Size", Float) = 0
        _NoiseSize ("NoiseSize", Float) = 0
        _NoiseDisplacement ("NoiseDisplacement", Float) = 0
        _NoiseSpeed ("NoiseSpeed", Float) = 1
        [HideInInspector] __dirty ("__dirty", Float) = 1
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "IsEmissive"="True" "Queue"="Transparent+0" "RenderType"="Transparent" }
        Pass
        {
            Tags { "IgnoreProjector"="True" "IsEmissive"="True" "LightMode"="FORWARDBASE" "Queue"="Transparent+0" "RenderType"="Transparent" "ShadowSupport"="True" }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 color : COLOR0;
                float3 normal : NORMAL0;
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
                float3 texcoord2 : TEXCOORD2;
                float4 texcoord4 : TEXCOORD4;
                float4 texcoord5 : TEXCOORD5;
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _Color;
            float _Size;
            float _NoiseSpeed;
            float _NoiseSize;
            float _NoiseDisplacement;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r1 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1 = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r0.xyz;
                r0 = r1.y * unity_MatrixVP._m01_m11_m21_m31;
                r0 = unity_MatrixVP._m00_m10_m20_m30 * r1.x + r0;
                r0 = unity_MatrixVP._m02_m12_m22_m32 * r1.z + r0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r1.w + r0;
                r0.x = dot(v.normal, unity_WorldToObject._m00_m10_m20);
                r0.y = dot(v.normal, unity_WorldToObject._m01_m11_m21);
                r0.z = dot(v.normal, unity_WorldToObject._m02_m12_m22);
                r0.w = dot(r0.xyz, r0.xyz);
                r0.w = rsqrt(r0.w);
                o.texcoord = r0.w * r0.xyz;
                o.color = v.color;
                o.texcoord2 = 0.0;
                o.texcoord4 = 0.0;
                o.texcoord5 = 0.0;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11;
                r0.xw = float2(0.0, 1.0);
                r1.xy = inp.texcoord1.y * unity_WorldToObject._m01_m11;
                r1.xy = unity_WorldToObject._m00_m10 * inp.texcoord1.x + r1.xy;
                r1.xy = unity_WorldToObject._m02_m12 * inp.texcoord1.z + r1.xy;
                r1.xy = r1.xy + unity_WorldToObject._m03_m13;
                r1.z = _NoiseSpeed * _Time.y;
                r2.xyz = r1.xyz * _NoiseSize;
                r1.w = dot(r2.xyz, float3(0.33333334, 0.33333334, 0.33333334));
                r2.xyz = r1.xyz * _NoiseSize + r1.w;
                r2.xyz = floor(r2.xyz);
                r3.xyz = r2.xyz * 0.0034602077;
                r3.xyz = floor(r3.xyz);
                r3.xyz = -r3.xyz * 289.0 + r2.xyz;
                r4.xyz = r1.xyz * _NoiseSize + -r2.xyz;
                r1.z = dot(r2.xyz, float3(0.16666667, 0.16666667, 0.16666667));
                r2.xyz = r1.z + r4.xyz;
                r4.x = asfloat((r2.z >= r2.x) ? 0xffffffffu : 0u);
                r4.y = asfloat((r2.x >= r2.y) ? 0xffffffffu : 0u);
                r4.z = asfloat((r2.y >= r2.z) ? 0xffffffffu : 0u);
                r5.xyz = asfloat(asuint(r4.yzx) & uint3(0x3f800000u, 0x3f800000u, 0x3f800000u));
                r4.x = (asuint(r4.x) != 0u) ? 0.0 : 1.0;
                r4.y = (asuint(r4.y) != 0u) ? 0.0 : 1.0;
                r4.z = (asuint(r4.z) != 0u) ? 0.0 : 1.0;
                r6.xyz = min(r4.xyz, r5.xyz);
                r4.xyz = max(r4.xyz, r5.xyz);
                r0.y = r6.z;
                r0.z = r4.z;
                r0 = r0 + r3.z;
                r5 = r0 * 34.0 + 1.0;
                r0 = r0 * r5;
                r5 = r0 * 0.0034602077;
                r5 = floor(r5);
                r0 = -r5 * 289.0 + r0;
                r0 = r3.y + r0;
                r5.xw = float2(0.0, 1.0);
                r5.y = r6.y;
                r5.z = r4.y;
                r0 = r0 + r5;
                r5 = r0 * 34.0 + 1.0;
                r0 = r0 * r5;
                r5 = r0 * 0.0034602077;
                r5 = floor(r5);
                r0 = -r5 * 289.0 + r0;
                r0 = r3.x + r0;
                r3.xw = float2(0.0, 1.0);
                r3.y = r6.x;
                r5.xyz = r2.xyz + -r6.xyz;
                r5.xyz = r5.xyz + 0.16666667;
                r3.z = r4.x;
                r4.xyz = r2.xyz + -r4.xyz;
                r4.xyz = r4.xyz + 0.33333334;
                r0 = r0 + r3;
                r3 = r0 * 34.0 + 1.0;
                r0 = r0 * r3;
                r3 = r0 * 0.0034602077;
                r3 = floor(r3);
                r0 = -r3 * 289.0 + r0;
                r3 = r0 * 0.020408163;
                r3 = floor(r3);
                r0 = -r3 * 49.0 + r0;
                r3 = r0 * 0.14285715;
                r3 = floor(r3);
                r0 = -r3 * 7.0 + r0;
                r3 = r3 * 2.0 + 0.5;
                r3 = r3.zxwy * 0.14285715 + -1.0;
                r0 = r0 * 2.0 + 0.5;
                r0 = r0 * 0.14285715 + -1.0;
                r6.yw = r0.xy;
                r6.xz = r3.yw;
                r7.yw = floor(r0.xy);
                r7.xz = floor(r3.yw);
                r7 = r7 * 2.0 + 1.0;
                r8 = -abs(r3.ywxz) + 1.0;
                r8 = -abs(r0.xywz) + r8.xywz;
                r9.x = asfloat((0.0 >= r8.x) ? 0xffffffffu : 0u);
                r9.y = asfloat((0.0 >= r8.y) ? 0xffffffffu : 0u);
                r9.z = asfloat((0.0 >= r8.w) ? 0xffffffffu : 0u);
                r9.w = asfloat((0.0 >= r8.z) ? 0xffffffffu : 0u);
                r9.x = (asuint(r9.x) != 0u) ? -1.0 : -0.0;
                r9.y = (asuint(r9.y) != 0u) ? -1.0 : -0.0;
                r9.z = (asuint(r9.z) != 0u) ? -1.0 : -0.0;
                r9.w = (asuint(r9.w) != 0u) ? -1.0 : -0.0;
                r6 = r7.zwxy * r9.yyxx + r6.zwxy;
                r7.xy = r6.zw;
                r7.z = r8.x;
                r10.x = dot(r7.xyz, r7.xyz);
                r6.z = r8.y;
                r10.y = dot(r6.xyz, r6.xyz);
                r11.yw = floor(r0.zw);
                r3.yw = r0.zw;
                r11.xz = floor(r3.xz);
                r0 = r11 * 2.0 + 1.0;
                r0 = r0 * r9.zzww + r3;
                r8.xy = r0.zw;
                r0.z = r8.w;
                r10.z = dot(r0.xyz, r0.xyz);
                r10.w = dot(r8.xyz, r8.xyz);
                r3 = -r10 * 0.85373473 + 1.7928429;
                r6.xyz = r3.y * r6.xyz;
                r6.y = dot(r5.xyz, r6.xyz);
                r5.y = dot(r5.xyz, r5.xyz);
                r0.xyz = r0.xyz * r3.z;
                r6.z = dot(r4.xyz, r0.xyz);
                r5.z = dot(r4.xyz, r4.xyz);
                r0.xyz = r3.x * r7.xyz;
                r3.xyz = r3.w * r8.xyz;
                r6.x = dot(r2.xyz, r0.xyz);
                r0.xyz = r2.xyz + -0.5;
                r5.x = dot(r2.xyz, r2.xyz);
                r6.w = dot(r0.xyz, r3.xyz);
                r5.w = dot(r0.xyz, r0.xyz);
                r0 = -r5 + 0.6;
                r0 = max(r0, float4(0.0, 0.0, 0.0, 0.0));
                r0 = r0 * r0;
                r0 = r0 * r0;
                r0.x = dot(r0, r6);
                r0.x = r0.x * 21.0 + 0.5;
                r0.x = r0.x * 2.0 + -1.0;
                r0.x = r0.x * _NoiseDisplacement;
                r0.xy = r1.xy * _Size + r0.x;
                r0 = tex2D(_MainTex, r0.xy);
                r0.x = r0.w * _Color.w;
                o.sv_target.w = r0.x * inp.color.x;
                o.sv_target.xyz = _Color.xyz;
                return o;
            }
            ENDCG
        }
    }
}
