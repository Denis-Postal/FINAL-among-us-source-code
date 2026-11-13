// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Light/PointLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowBleedDistance ("Shadow Bleed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Pass
        {
            Tags { "RenderType"="Transparent" }
            Cull Off
            ZTest Always
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float3 texcoord1 : TEXCOORD1;
                float3 texcoord2 : TEXCOORD2;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float _ShadowBleedDistance;
            float4 _TexelSize;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                o.texcoord = v.texcoord;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r1 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord2 = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r0.xyz;
                r0 = r1.y * unity_MatrixVP._m01_m11_m21_m31;
                r0 = unity_MatrixVP._m00_m10_m20_m30 * r1.x + r0;
                r0 = unity_MatrixVP._m02_m12_m22_m32 * r1.z + r0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r1.w + r0;
                o.texcoord1 = unity_ObjectToWorld._m03_m13_m23;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1;
                r0.xyz = inp.texcoord1 + -inp.texcoord2;
                r0.z = dot(r0.xyz, r0.xyz);
                r0.z = rsqrt(r0.z);
                r0.zw = r0.z * r0.yx;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.y = max(abs(r0.w), abs(r0.z));
                r0.y = 1.0 / r0.y;
                r1.x = min(abs(r0.w), abs(r0.z));
                r0.y = r0.y * r1.x;
                r1.x = r0.y * r0.y;
                r1.y = r1.x * 0.0208351 + -0.085133;
                r1.y = r1.x * r1.y + 0.180141;
                r1.y = r1.x * r1.y + -0.3302995;
                r1.x = r1.x * r1.y + 0.999866;
                r1.y = r0.y * r1.x;
                r1.y = r1.y * -2.0 + 1.5707964;
                r1.z = asfloat((abs(r0.w) < abs(r0.z)) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.z) & asuint(r1.y));
                r0.y = r0.y * r1.x + r1.y;
                r1.x = asfloat((-r0.w < r0.w) ? 0xffffffffu : 0u);
                r1.x = asfloat(asuint(r1.x) & 0xc0490fdbu);
                r0.y = r0.y + r1.x;
                r1.x = min(-r0.w, -r0.z);
                r0.z = max(-r0.w, -r0.z);
                r0.z = asfloat((r0.z >= -r0.z) ? 0xffffffffu : 0u);
                r0.w = asfloat((r1.x < -r1.x) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & asuint(r0.w));
                r0.y = (asuint(r0.z) != 0u) ? -r0.y : r0.y;
                r0.z = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                r0.w = r0.y + 6.283185;
                r0.y = (asuint(r0.z) != 0u) ? r0.w : r0.y;
                r1.y = r0.y * 0.15915495;
                r1.x = _TexelSize.x * 1.5;
                r1 = tex2D(_MainTex, r1.xy);
                r0.x = r0.x + -r1.x;
                r0.x = max(r0.x, 0.0);
                r0.x = saturate(r0.x / _ShadowBleedDistance);
                r0.x = -r0.x + 1.0;
                r0.yz = inp.texcoord + -0.5;
                r0.y = dot(r0.yz, r0.yz);
                r0.y = sqrt(r0.y);
                r0.y = -r0.y * 2.0 + 1.0;
                r0.y = max(r0.y, 0.0);
                r0.y = r0.y * r0.y;
                o.sv_target.xyz = r0.x * r0.y;
                o.sv_target.w = 0.0;
                return o;
            }
            ENDCG
        }
    }
}
