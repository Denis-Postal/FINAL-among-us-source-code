// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Hidden/ProBuilder/EdgePicker"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "DisableBatching"="True" "IgnoreProjector"="True" "LightMode"="ALWAYS" "ProBuilderPicker"="EdgePass" }
        Pass
        {
            Tags { "DisableBatching"="True" "IgnoreProjector"="True" "LightMode"="ALWAYS" "ProBuilderPicker"="EdgePass" }
            Cull Off
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
                r1.xyz = r0.y * unity_MatrixV._m01_m11_m21;
                r1.xyz = unity_MatrixV._m00_m10_m20 * r0.x + r1.xyz;
                r0.xyz = unity_MatrixV._m02_m12_m22 * r0.z + r1.xyz;
                r0.xyz = unity_MatrixV._m03_m13_m23 * r0.w + r0.xyz;
                r1.xy = unity_OrthoParams.w * float2(0.00999999, -0.050000012) + float2(0.99, 1.0);
                r0.xyz = r0.xyz * r1.x;
                r0.z = r1.y * r0.z;
                r1 = r0.y * UNITY_MATRIX_P._m01_m11_m21_m31;
                r1 = UNITY_MATRIX_P._m00_m10_m20_m30 * r0.x + r1;
                r0 = UNITY_MATRIX_P._m02_m12_m22_m32 * r0.z + r1;
                o.position = r0 + UNITY_MATRIX_P._m03_m13_m23_m33;
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
    }
}
