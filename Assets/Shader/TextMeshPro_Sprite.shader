// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "TextMeshPro/Sprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _StencilComp ("Stencil Comparison", Float) = 4
        _Stencil ("Stencil ID", Float) = 1
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _CullMode ("Cull Mode", Float) = 0
        _ColorMask ("Color Mask", Float) = 15
        _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask 0
            Cull Off
            ZTest Always
            ZWrite Off
            Stencil {
                Ref [_Stencil]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilOp]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
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
                float4 texcoord1 : TEXCOORD1;
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _Color;
            float4 _TextureSampleAdd;
            float4 _ClipRect;
            sampler2D _MainTex;
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
                o.color = v.color * _Color;
                o.texcoord = v.texcoord;
                o.texcoord1 = v.vertex;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1;
#if defined(UNITY_UI_ALPHACLIP) && !defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord);
                r0 = r0 + _TextureSampleAdd;
                r1.x = r0.w * inp.color.w + -0.001;
                r0 = r0 * inp.color;
                o.sv_target = r0;
                r0.x = asfloat((r1.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#elif !defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0.x = asfloat((inp.texcoord1.x >= _ClipRect.x) ? 0xffffffffu : 0u);
                r0.y = asfloat((inp.texcoord1.y >= _ClipRect.y) ? 0xffffffffu : 0u);
                r0.z = asfloat((_ClipRect.z >= inp.texcoord1.x) ? 0xffffffffu : 0u);
                r0.w = asfloat((_ClipRect.w >= inp.texcoord1.y) ? 0xffffffffu : 0u);
                r0 = asfloat(asuint(r0) & uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
                r0.xy = r0.zw * r0.xy;
                r0.x = r0.y * r0.x;
                r1 = tex2D(_MainTex, inp.texcoord);
                r1 = r1 + _TextureSampleAdd;
                r1 = r1 * inp.color;
                o.sv_target.w = r0.x * r1.w;
                o.sv_target.xyz = r1.xyz;
#elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0.x = asfloat((inp.texcoord1.x >= _ClipRect.x) ? 0xffffffffu : 0u);
                r0.y = asfloat((inp.texcoord1.y >= _ClipRect.y) ? 0xffffffffu : 0u);
                r0.z = asfloat((_ClipRect.z >= inp.texcoord1.x) ? 0xffffffffu : 0u);
                r0.w = asfloat((_ClipRect.w >= inp.texcoord1.y) ? 0xffffffffu : 0u);
                r0 = asfloat(asuint(r0) & uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
                r0.xy = r0.zw * r0.xy;
                r0.x = r0.y * r0.x;
                r1 = tex2D(_MainTex, inp.texcoord);
                r1 = r1 + _TextureSampleAdd;
                r1 = r1 * inp.color;
                r0.y = r1.w * r0.x + -0.001;
                r0.x = r0.x * r1.w;
                o.sv_target.xyz = r1.xyz;
                o.sv_target.w = r0.x;
                r0.x = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#else
                r0 = tex2D(_MainTex, inp.texcoord);
                r0 = r0 + _TextureSampleAdd;
                o.sv_target = r0 * inp.color;
#endif
                return o;
            }
            ENDCG
        }
    }
}
