// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/SimpleMaskedPlayer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackColor ("Shadow Color", Color) = (1,0,0,1)
        _BodyColor ("Body Color", Color) = (1,1,0,1)
        _VisorColor ("VisorColor", Color) = (0,1,1,1)
        _MaskLayer ("MaskLayer", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            Stencil {
                Ref [_MaskLayer]
                Comp Equal
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 color : COLOR0;
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _MainTex_ST;
            float4 _BodyColor;
            float4 _VisorColor;
            float4 _BackColor;
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
                o.color = v.color;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3;
                r0 = tex2D(_MainTex, inp.texcoord);
                r1.x = dot(r0.xyz, r0.xyz);
                r1.x = rsqrt(r1.x);
                r1.xyz = r0.xyz * r1.x;
                r1.w = dot(r1.xyz, float3(0.57735026, 0.57735026, 0.57735026));
                r1.x = dot(r1.zxy, r1.xyz);
                r1.x = saturate(r1.x * 0.5);
                r1.x = -r1.x + 1.0;
                r1.x = ceil(r1.x);
                r1.y = r1.w + 0.01;
                r1.y = saturate(floor(r1.y));
                r2.xyz = r0.y * _VisorColor.xyz;
                r2.xyz = _BodyColor.xyz * r0.x + r2.xyz;
                r2.xyz = _BackColor.xyz * r0.z + r2.xyz;
                r3.xyz = r0.xyz + -r2.xyz;
                r1.yzw = r1.y * r3.xyz + r2.xyz;
                r2.xyz = -r0.xyz + r1.yzw;
                r2.w = 0.0;
                r0 = r1.x * r2 + r0;
                o.sv_target = r0 * inp.color;
                return o;
            }
            ENDCG
        }
    }
}
