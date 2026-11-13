// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/FadingColorShader"
{
    Properties
    {
        _Fade ("Fade", Float) = 0.5
        _Color ("Color", Color) = (1,1,1,1)
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
            float _Fade;
            float4 _Color;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                o.texcoord = v.texcoord;
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
                float4 r0;
                r0.x = asfloat((_Fade < 0.3) ? 0xffffffffu : 0u);
                r0.y = asfloat((_Fade < inp.texcoord.y) ? 0xffffffffu : 0u);
                r0.x = asfloat(asuint(r0.y) & asuint(r0.x));
                r0.y = asfloat((inp.texcoord.y < _Fade) ? 0xffffffffu : 0u);
                r0.zw = _Fade + float2(0.7, -0.3);
                r0.zw = saturate(-r0.zw + inp.texcoord.y);
                r0.y = asfloat(asuint(r0.w) & asuint(r0.y));
                r0.x = (asuint(r0.x) != 0u) ? r0.z : r0.y;
                o.sv_target.w = r0.x * 3.3333333;
                o.sv_target.xyz = _Color.xyz;
                return o;
            }
            ENDCG
        }
    }
}
