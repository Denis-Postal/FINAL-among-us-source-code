// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/ColorStretchTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _CenterHeight ("Center Height", Float) = 0.3333
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
            float _CenterHeight;
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
                float4 r0, r1;
                r0.x = -_CenterHeight + 1.0;
                r0.y = -r0.x * 0.5 + inp.texcoord.y;
                r0.y = r0.y / _CenterHeight;
                r0.zw = -r0.x * float2(0.5, 0.05) + 1.0;
                r1.xy = r0.x * float2(0.5, 0.05);
                r0.x = asfloat((inp.texcoord.y >= r0.z) ? 0xffffffffu : 0u);
                r0.x = (asuint(r0.x) != 0u) ? r0.w : r0.y;
                r0.y = asfloat((r1.x >= inp.texcoord.y) ? 0xffffffffu : 0u);
                r0.y = (asuint(r0.y) != 0u) ? r1.y : r0.x;
                r0.x = 0.5;
                r0 = tex2D(_MainTex, r0.xy);
                o.sv_target = r0 * _Color;
                return o;
            }
            ENDCG
        }
    }
}
