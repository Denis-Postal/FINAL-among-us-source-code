// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/GradientShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (0,0,0,1)
        _Color2 ("Color2", Color) = (0,0,0,1)
        _Center ("Center", Float) = 0.25
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
            float4 _Color1;
            float4 _Color2;
            float _Center;
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
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1;
                r0.x = inp.texcoord.x * 2.0 + -1.0;
                r0.y = asfloat((abs(r0.x) < _Center) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) {
                o.sv_target = _Color1;
                }
                r0.x = abs(r0.x) + -_Center;
                r0.y = -_Center + 1.0;
                r0.x = r0.x / r0.y;
                r0.x = sqrt(r0.x);
                r1 = -_Color1 + _Color2;
                o.sv_target = r0.x * r1 + _Color1;
                return o;
            }
            ENDCG
        }
    }
}
