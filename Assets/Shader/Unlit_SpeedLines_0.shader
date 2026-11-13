// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/SpeedLines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackgroungColor ("Background Color", Color) = (0,0,0,1)
        _BaseColor ("Base Color", Color) = (1,0,0,1)
        _LineColor ("Line Color", Color) = (0,0,1,1)
        _LineColor2 ("Line Color 2", Color) = (0,0,1,1)
        _AdditiveGradient ("AdditiveGradient", Color) = (0,0,0,0)
        _Speed ("Speed", Float) = -10
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
            float4 _BackgroundColor;
            float4 _BaseColor;
            float4 _LineColor;
            float4 _LineColor2;
            float4 _AdditiveGradient;
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
                float4 r0, r1;
                r0.x = inp.texcoord.y * 4.0 + -inp.texcoord.x;
                r0.y = _Speed * _Time.x;
                r1.x = r0.x * 0.125 + r0.y;
                r1.y = inp.texcoord.y * 8.0 + r0.y;
                r0 = tex2D(_MainTex, r1.xy);
                r0.y = r0.x * r0.x;
                r0.x = r0.x + -0.5;
                r0.x = max(r0.x, 0.0);
                r0.x = r0.x + r0.x;
                r0.xzw = r0.x * _LineColor2.xyz;
                r0.y = r0.y * 1.5;
                r1.xyz = -_BaseColor.xyz + _LineColor.xyz;
                r1.xyz = r0.y * r1.xyz + _BaseColor.xyz;
                r0.y = inp.texcoord.y * 3.1415;
                r0.y = sin(r0.y);
                r1.w = r0.y * r0.y;
                r0.y = r0.y * r1.w;
                r0.xzw = saturate(r0.y * r1.xyz + r0.xzw);
                r0.xzw = r0.xzw + -_BackgroundColor.xyz;
                r0.xyz = r0.y * r0.xzw + _BackgroundColor.xyz;
                o.sv_target.xyz = _AdditiveGradient.xyz * inp.texcoord.y + r0.xyz;
                o.sv_target.w = 1.0;
                return o;
            }
            ENDCG
        }
    }
}
