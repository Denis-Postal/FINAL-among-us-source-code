// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/BucketShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmptyColor ("EmptyColor", Color) = (0,0.5,0,1)
        _FullColor ("FullColor", Color) = (0,1,0,1)
        _Buckets ("Buckets", Float) = 5
        _FullBuckets ("FullBuckets", Float) = 3
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
            float _Buckets;
            float _FullBuckets;
            float4 _EmptyColor;
            float4 _FullColor;
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
                float4 r0;
                r0.x = inp.texcoord.x * _Buckets;
                r0.y = frac(r0.x);
                r0.y = r0.y + -0.49999;
                r0.y = abs(r0.y) * 2e+02;
                r0.y = floor(r0.y);
                r0.y = r0.y * 0.01;
                r0.z = -_Buckets * 0.010000001 + 1.0;
                r0.y = asfloat((r0.z < r0.y) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) {
                    o.sv_target = float4(0.0, 0.0, 0.0, 1.0);
                } else if (_FullBuckets >= r0.x) {
                    o.sv_target = _FullColor;
                } else {
                    o.sv_target = _EmptyColor;
                }
                return o;
            }
            ENDCG
        }
    }
}
