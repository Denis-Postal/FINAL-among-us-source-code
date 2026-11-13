// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/BlurredColoredVertices"
{
    Properties
    {
        _Size ("Size", Range(0,20)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
			Cull Off
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Stencil {
                Ref 1
                Pass Replace
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
            };
            struct v2f
            {
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
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
                o.sv_target = inp.color;
                return o;
            }
            ENDCG
        }
        GrabPass { }
        Pass
        {
			Cull Off
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 texcoord : TEXCOORD0;
                float4 position : SV_Position0;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _GrabTexture_TexelSize;
            float _Size;
            sampler2D _GrabTexture;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r0 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1 = r0.y * unity_MatrixVP._m01_m11_m21_m31;
                r1 = unity_MatrixVP._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_MatrixVP._m02_m12_m22_m32 * r0.z + r1;
                r0 = unity_MatrixVP._m03_m13_m23_m33 * r0.w + r1;
                o.position = r0;
                r0.xy = r0.xy * float2(1.0, -1.0) + r0.w;
                o.texcoord.zw = r0.zw;
                o.texcoord.xy = r0.xy * 0.5;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3, r4;
                r0.yw = inp.texcoord.y;
                r1.x = _GrabTexture_TexelSize.x * _Size;
                r2 = r1.x * float4(3.0, -4.0, -3.0, -2.0) + inp.texcoord.x;
                r0.xz = r2.yz;
                r0 = r0 / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r0.zw);
                r0 = tex2D(_GrabTexture, r0.xy);
                r3 = r3 * 0.09;
                r0 = r0 * 0.05 + r3;
                r3.x = r2.w;
                r3.yw = inp.texcoord.y;
                r1.yz = r3.xy / inp.texcoord.w;
                r4 = tex2D(_GrabTexture, r1.yz);
                r0 = r4 * 0.12 + r0;
                r3.z = -_GrabTexture_TexelSize.x * _Size + inp.texcoord.x;
                r1.yz = r3.zw / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r1.yz);
                r0 = r3 * 0.15 + r0;
                r1.yz = inp.texcoord.xy / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r1.yz);
                r0 = r3 * 0.18 + r0;
                r3.x = _GrabTexture_TexelSize.x * _Size + inp.texcoord.x;
                r3.yw = inp.texcoord.y;
                r1.yz = r3.xy / inp.texcoord.w;
                r4 = tex2D(_GrabTexture, r1.yz);
                r0 = r4 * 0.15 + r0;
                r3.z = r1.x * 2.0 + inp.texcoord.x;
                r2.z = r1.x * 4.0 + inp.texcoord.x;
                r1.xy = r3.zw / inp.texcoord.w;
                r1 = tex2D(_GrabTexture, r1.xy);
                r0 = r1 * 0.12 + r0;
                r2.yw = inp.texcoord.y;
                r1 = r2 / inp.texcoord.w;
                r2 = tex2D(_GrabTexture, r1.zw);
                r1 = tex2D(_GrabTexture, r1.xy);
                r0 = r1 * 0.09 + r0;
                o.sv_target = r2 * 0.05 + r0;
                return o;
            }
            ENDCG
        }
        GrabPass { }
        Pass
        {
			Cull Off
            Tags { "LightMode"="ALWAYS" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 texcoord : TEXCOORD0;
                float4 position : SV_Position0;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _GrabTexture_TexelSize;
            float _Size;
            sampler2D _GrabTexture;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r0 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1 = r0.y * unity_MatrixVP._m01_m11_m21_m31;
                r1 = unity_MatrixVP._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_MatrixVP._m02_m12_m22_m32 * r0.z + r1;
                r0 = unity_MatrixVP._m03_m13_m23_m33 * r0.w + r1;
                o.position = r0;
                r0.xy = r0.xy * float2(1.0, -1.0) + r0.w;
                o.texcoord.zw = r0.zw;
                o.texcoord.xy = r0.xy * 0.5;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3, r4;
                r0.xz = inp.texcoord.x;
                r1.x = _GrabTexture_TexelSize.y * _Size;
                r2 = r1.x * float4(-4.0, 3.0, -3.0, -2.0) + inp.texcoord.y;
                r0.yw = r2.xz;
                r0 = r0 / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r0.zw);
                r0 = tex2D(_GrabTexture, r0.xy);
                r3 = r3 * 0.09;
                r0 = r0 * 0.05 + r3;
                r3.y = r2.w;
                r3.xz = inp.texcoord.x;
                r1.yz = r3.xy / inp.texcoord.w;
                r4 = tex2D(_GrabTexture, r1.yz);
                r0 = r4 * 0.12 + r0;
                r3.w = -_GrabTexture_TexelSize.y * _Size + inp.texcoord.y;
                r1.yz = r3.zw / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r1.yz);
                r0 = r3 * 0.15 + r0;
                r1.yz = inp.texcoord.xy / inp.texcoord.w;
                r3 = tex2D(_GrabTexture, r1.yz);
                r0 = r3 * 0.18 + r0;
                r3.y = _GrabTexture_TexelSize.y * _Size + inp.texcoord.y;
                r3.xz = inp.texcoord.x;
                r1.yz = r3.xy / inp.texcoord.w;
                r4 = tex2D(_GrabTexture, r1.yz);
                r0 = r4 * 0.15 + r0;
                r3.w = r1.x * 2.0 + inp.texcoord.y;
                r2.w = r1.x * 4.0 + inp.texcoord.y;
                r1.xy = r3.zw / inp.texcoord.w;
                r1 = tex2D(_GrabTexture, r1.xy);
                r0 = r1 * 0.12 + r0;
                r2.xz = inp.texcoord.x;
                r1 = r2 / inp.texcoord.w;
                r2 = tex2D(_GrabTexture, r1.zw);
                r1 = tex2D(_GrabTexture, r1.xy);
                r0 = r1 * 0.09 + r0;
                o.sv_target = r2 * 0.05 + r0;
                return o;
            }
            ENDCG
        }
    }
}
