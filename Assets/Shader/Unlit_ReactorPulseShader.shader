// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/ReactorPulseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Pass
        {
			Cull Off
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _MainTex_ST;
            float4 _Color;
            float _Speed;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                o.texcoord = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r0 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1 = r0.y * unity_MatrixVP._m01_m11_m21_m31;
                r1 = unity_MatrixVP._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_MatrixVP._m02_m12_m22_m32 * r0.z + r1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r0.w + r1;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3;
                r0.xyz = _Color.xyz;
                r1.xy = inp.texcoord * 2.0 + -1.0;
                r1.x = dot(r1.xy, r1.xy);
                r1.x = sqrt(r1.x);
                r1.y = _Speed * _Time.w;
                r1.y = r1.y * 1.25;
                r1.y = sin(r1.y);
                r2 = r1.y * float4(0.5, 0.1, 0.2, 0.2) + float4(0.5, -0.05, -0.1, 0.5);
                r1.y = r1.y * 0.1 + 0.8;
                r1.x = r1.x + r2.y;
                r1.z = r1.x + -0.02;
                r1.w = -r2.y + r2.z;
                r2.yz = r1.w + float2(0.25, 0.23);
                r1.z = saturate(r1.z / r2.z);
                r3.xyz = -1.0;
                r3.w = r2.x * -0.35000002;
                r3 = r3 + float4(1.0, 0.8, 0.0, 0.0);
                r3 = r1.z * r3 + 1.0;
                r0.w = r2.x * -0.35000002 + 1.0;
                r0 = r0 + -r3;
                r1.z = -r2.y + r2.w;
                r1.w = r1.x + -r2.y;
                r1.z = saturate(r1.w / r1.z);
                r0 = r1.z * r0 + r3;
                r3 = -r0 + float4(0.25, 0.23, 0.5, 0.5);
                r1.zw = -r2.w + r1.xy;
                r1.z = saturate(r1.z / r1.w);
                r0 = r1.z * r3 + r0;
                r1.x = -r1.y + r1.x;
                r1.y = -r1.y + 0.95;
                r1.y = saturate(r1.x / r1.y);
                r1.x = saturate(r1.x * 1e+06);
                r2.xyz = _Color.xyz;
                r2.w = 0.5;
                r2 = -r0 + r2;
                r0 = r1.x * r2 + r0;
                r2.xyz = _Color.xyz;
                r2.w = 0.0;
                r2 = -r0 + r2;
                r0 = r1.y * r2 + r0;
                r1 = tex2D(_MainTex, inp.texcoord);
                o.sv_target = r0 * r1;
                return o;
            }
            ENDCG
        }
    }
}
