// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/EitherOrShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AltTex ("Alt Texture", 2D) = "white" {}
        _Perc ("Perc", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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
            float _Perc;
            sampler2D _MainTex;
            sampler2D _AltTex;
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
                float4 r0, r1, r2;
                r0 = tex2D(_AltTex, inp.texcoord);
                r1.x = saturate(-_Perc * _MainTex_ST.x + inp.texcoord.x);
                r1.y = -r1.x + 1.0;
                r0 = r0 * r1.y;
                r2 = tex2D(_MainTex, inp.texcoord);
                r0 = r2 * r1.x + r0;
                o.sv_target = r0 * inp.color;
                return o;
            }
            ENDCG
        }
    }
}
