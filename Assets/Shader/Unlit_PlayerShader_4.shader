// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/PlayerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackColor ("Shadow Color", Color) = (1,0,0,1)
        _BodyColor ("Body Color", Color) = (1,1,0,1)
        _VisorColor ("VisorColor", Color) = (0,1,1,1)
        _Outline ("Outline", Range(0,1)) = 0
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _Desaturate ("Desaturate", Range(0,1)) = 0
        _MaskLayer ("Mask Layer", Float) = 1
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
                Comp GEqual
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
            float4 _MainTex_TexelSize;
            float4 _BodyColor;
            float4 _VisorColor;
            float4 _BackColor;
            float _Outline;
            float4 _OutlineColor;
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
                float4 r0, r1, r2, r3, r4, r5;
                r0.xw = 0.0;
                r0.yz = _MainTex_TexelSize.yx * 2.0;
                r1 = r0 + inp.texcoord.xyxy;
                r0 = -r0 + inp.texcoord.xyxy;
                r2 = tex2D(_MainTex, r1.xy);
                r1 = tex2D(_MainTex, r1.zw);
                r3 = tex2D(_MainTex, r0.xy);
                r0 = tex2D(_MainTex, r0.zw);
                r0.x = r2.w + r3.w;
                r0.x = r1.w + r0.x;
                r0.x = r0.w + r0.x;
                r1.xw = 0.0;
                r1.yz = _MainTex_TexelSize.yx;
                r2 = r1 + inp.texcoord.xyxy;
                r1 = -r1 + inp.texcoord.xyxy;
                r3 = tex2D(_MainTex, r2.xy);
                r2 = tex2D(_MainTex, r2.zw);
                r4 = tex2D(_MainTex, r1.xy);
                r1 = tex2D(_MainTex, r1.zw);
                r0.y = r3.w + r4.w;
                r0.y = r2.w + r0.y;
                r0.y = r1.w + r0.y;
                r1 = tex2D(_MainTex, inp.texcoord);
                r0.z = dot(r1.xyz, r1.xyz);
                r0.z = rsqrt(r0.z);
                r2.xyz = r0.z * r1.xyz;
                r0.z = dot(r2.xyz, float3(0.57735026, 0.57735026, 0.57735026));
                r0.w = dot(r2.zxy, r2.xyz);
                r0.w = saturate(r0.w * 0.5);
                r0.w = -r0.w + 1.0;
                r0.w = ceil(r0.w);
                r0.z = r0.z + 0.01;
                r0.z = saturate(floor(r0.z));
                r2.xyz = r1.y * _VisorColor.xyz;
                r2.xyz = _BodyColor.xyz * r1.x + r2.xyz;
                r2.xyz = _BackColor.xyz * r1.z + r2.xyz;
                r3.xyz = r1.xyz + -r2.xyz;
                r2.xyz = r0.z * r3.xyz + r2.xyz;
                r2.xyz = -r1.xyz + r2.xyz;
                r2.w = 0.0;
                r1 = r0.w * r2 + r1;
                r0.y = r0.y + r1.w;
                r0.x = r0.x + r0.y;
                r2.xw = 0.0;
                r2.yz = _MainTex_TexelSize.yx * 3.0;
                r3 = r2 + inp.texcoord.xyxy;
                r2 = -r2 + inp.texcoord.xyxy;
                r4 = tex2D(_MainTex, r3.xy);
                r3 = tex2D(_MainTex, r3.zw);
                r5 = tex2D(_MainTex, r2.xy);
                r2 = tex2D(_MainTex, r2.zw);
                r0.y = r4.w + r5.w;
                r0.y = r3.w + r0.y;
                r0.y = r2.w + r0.y;
                r0.x = r0.y + r0.x;
                r0.y = saturate(_Outline);
                r0.w = r0.y * r0.x;
                r0.xyz = _OutlineColor.xyz;
                r2 = -r0 + r1;
                r1.x = r1.w + 0.05;
                r1.x = round(r1.x);
                r0 = r1.x * r2 + r0;
                o.sv_target = r0 * inp.color;
                return o;
            }
            ENDCG
        }
    }
}
