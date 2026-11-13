// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Sprites/Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Outline ("Outline", Float) = 0
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _AddColor ("AddColor", Color) = (0,0,0,0)
        [Toggle(OUTLINE_FLIP)] _OutlineFlip ("Interior Outline", Float) = 0
    }
    SubShader
    {
        Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
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
            float _Outline;
            float4 _OutlineColor;
            float4 _AddColor;
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
                float4 r0, r1, r2, r3, r4, r5, r6;
                r0 = tex2D(_MainTex, inp.texcoord);
                r1.z = 0.0;
                r1.w = r0.w;
                r2.x = 1e-45;
                for (;;) {
                r2.y = asfloat((asint(r2.x) >= 4) ? 0xffffffffu : 0u);
                if (asuint(r2.y) != 0u) { break; }
                r2.y = (float)(asint(r2.x));
                r1.xy = r2.y * _MainTex_TexelSize.yx;
                r3 = r1.zxyz + inp.texcoord.xyxy;
                r4 = tex2D(_MainTex, r3.xy);
                r5 = -r1.zxyz + inp.texcoord.xyxy;
                r6 = tex2D(_MainTex, r5.xy);
                r3 = tex2D(_MainTex, r3.zw);
                r5 = tex2D(_MainTex, r5.zw);
                r1.x = r4.w + r6.w;
                r1.x = r3.w + r1.x;
                r1.x = r5.w + r1.x;
                r1.w = r1.x + r1.w;
                r2.x = asfloat(asint(r2.x) + 1);
                }
                r1.x = saturate(_Outline);
                r1.x = r1.x * inp.color.w;
                r1.w = r1.x * r1.w;
                r2.x = r0.w + 0.1;
                r2.x = round(r2.x);
                r1.xyz = _OutlineColor.xyz;
                r0 = r0 * inp.color + -r1;
                r0 = r2.x * r0 + r1;
                o.sv_target.xyz = _AddColor.xyz * 0.3 + r0.xyz;
                o.sv_target.w = r0.w;
                return o;
            }
            ENDCG
        }
    }
}
