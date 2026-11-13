// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "TextMeshPro/Mobile/Distance Field"
{
    Properties
    {
        [HDR] _FaceColor ("Face Color", Color) = (1,1,1,1)
        _FaceDilate ("Face Dilate", Range(-1,1)) = 0
        [HDR] _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
        _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
        [HDR] _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
        _UnderlayOffsetX ("Border OffsetX", Range(-1,1)) = 0
        _UnderlayOffsetY ("Border OffsetY", Range(-1,1)) = 0
        _UnderlayDilate ("Border Dilate", Range(-1,1)) = 0
        _UnderlaySoftness ("Border Softness", Range(0,1)) = 0
        _WeightNormal ("Weight Normal", Float) = 0
        _WeightBold ("Weight Bold", Float) = 0.5
        _ShaderFlags ("Flags", Float) = 0
        _ScaleRatioA ("Scale RatioA", Float) = 1
        _ScaleRatioB ("Scale RatioB", Float) = 1
        _ScaleRatioC ("Scale RatioC", Float) = 1
        _MainTex ("Font Atlas", 2D) = "white" {}
        _TextureWidth ("Texture Width", Float) = 512
        _TextureHeight ("Texture Height", Float) = 512
        _GradientScale ("Gradient Scale", Float) = 5
        _ScaleX ("Scale X", Float) = 1
        _ScaleY ("Scale Y", Float) = 1
        _PerspectiveFilter ("Perspective Correction", Range(0,1)) = 0.875
        _Sharpness ("Sharpness", Range(-1,1)) = 0
        _VertexOffsetX ("Vertex OffsetX", Float) = 0
        _VertexOffsetY ("Vertex OffsetY", Float) = 0
        _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
        _MaskSoftnessX ("Mask SoftnessX", Float) = 0
        _MaskSoftnessY ("Mask SoftnessY", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _CullMode ("Cull Mode", Float) = 0
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend One OneMinusSrcAlpha
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
            #pragma multi_compile_local _ UNDERLAY_ON
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 color : COLOR0;
                float3 normal : NORMAL0;
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct v2f
            {
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
                float2 texcoord4 : TEXCOORD4;
                float4 color : COLOR0;
                float4 color1 : COLOR1;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _FaceColor;
            float _FaceDilate;
            float _OutlineSoftness;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _WeightNormal;
            float _WeightBold;
            float _ScaleRatioA;
            float _VertexOffsetX;
            float _VertexOffsetY;
            float4 _ClipRect;
            float _MaskSoftnessX;
            float _MaskSoftnessY;
            float _GradientScale;
            float _ScaleX;
            float _ScaleY;
            float _PerspectiveFilter;
            float _Sharpness;
            float _UnderlayDilate;
            float _UnderlaySoftness;
            float _TextureHeight;
            float _TextureWidth;
            float _ScaleRatioC;
            float _UnderlayOffsetX;
            float _UnderlayOffsetY;
            float4 _UnderlayColor;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1, r2, r3, r4, r5, r6;
#if defined(UNDERLAY_ON)
                r0.xy = v.vertex.xy + float2(_VertexOffsetX, _VertexOffsetY);
                r1 = r0.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r1 = unity_ObjectToWorld._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r1;
                r2 = r1 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r1.xyz;
                r1.xyz = -r1.xyz + _WorldSpaceCameraPos.xyz;
                r3 = r2.y * unity_MatrixVP._m01_m11_m21_m31;
                r3 = unity_MatrixVP._m00_m10_m20_m30 * r2.x + r3;
                r3 = unity_MatrixVP._m02_m12_m22_m32 * r2.z + r3;
                r2 = unity_MatrixVP._m03_m13_m23_m33 * r2.w + r3;
                o.position = r2;
                o.color.w = _FaceColor.w;
                r3.xyz = v.color.xyz;
                r3.w = 1.0;
                r4 = r3 * _FaceColor;
                r2.xyz = r4.w * r4.xyz;
                o.color.xyz = r2.xyz;
                r5.xyz = -r2.xyz;
                r5.w = -r4.w;
                r6.xyz = _OutlineColor.w * _OutlineColor.xyz;
                r6.w = _OutlineColor.w;
                r5 = r5 + r6;
                r0.z = dot(r1.xyz, r1.xyz);
                r0.z = rsqrt(r0.z);
                r1.xyz = r0.z * r1.xyz;
                r2.x = dot(v.normal, unity_WorldToObject._m00_m10_m20);
                r2.y = dot(v.normal, unity_WorldToObject._m01_m11_m21);
                r2.z = dot(v.normal, unity_WorldToObject._m02_m12_m22);
                r0.z = dot(r2.xyz, r2.xyz);
                r0.z = rsqrt(r0.z);
                r2.xyz = r0.z * r2.xyz;
                r0.z = dot(r2.xyz, r1.xyz);
                r1.xy = _ScreenParams.y * UNITY_MATRIX_P._m01_m11;
                r1.xy = UNITY_MATRIX_P._m00_m10 * _ScreenParams.x + r1.xy;
                r1.xy = abs(r1.xy) * float2(_ScaleX, _ScaleY);
                r1.xy = r2.w / r1.xy;
                r0.w = dot(r1.xy, r1.xy);
                r1.xy = float2(_MaskSoftnessX, _MaskSoftnessY) * 0.25 + r1.xy;
                o.texcoord2.zw = 0.25 / r1.xy;
                r0.w = rsqrt(r0.w);
                r1.x = abs(v.texcoord1.y) * _GradientScale;
                r1.y = _Sharpness + 1.0;
                r1.x = r1.y * r1.x;
                r1.y = r0.w * r1.x;
                r1.z = -_PerspectiveFilter + 1.0;
                r1.z = r1.z * abs(r1.y);
                r0.w = r0.w * r1.x + -r1.z;
                r0.z = abs(r0.z) * r0.w + r1.z;
                r0.w = asfloat((UNITY_MATRIX_P._m33 == 0.0) ? 0xffffffffu : 0u);
                r0.z = (asuint(r0.w) != 0u) ? r0.z : r1.y;
                r0.w = _OutlineSoftness * _ScaleRatioA;
                r0.w = r0.w * r0.z + 1.0;
                r1.x = r0.z / r0.w;
                r0.w = _OutlineWidth * _ScaleRatioA;
                r0.w = r0.w * 0.5;
                r1.y = r1.x * r0.w;
                r1.y = r1.y + r1.y;
                r1.y = min(r1.y, 1.0);
                r1.y = sqrt(r1.y);
                r2 = r5 * r1.y;
                o.color1.xyz = r4.xyz * r4.w + r2.xyz;
                o.color1.w = r3.w * _FaceColor.w + r2.w;
                r2 = max(_ClipRect, float4(-2e+10, -2e+10, -2e+10, -2e+10));
                r2 = min(r2, float4(2e+10, 2e+10, 2e+10, 2e+10));
                r1.yz = r0.xy + -r2.xy;
                r0.xy = r0.xy * 2.0 + -r2.xy;
                o.texcoord2.xy = -r2.zw + r0.xy;
                r0.xy = -r2.xy + r2.zw;
                o.texcoord.zw = r1.yz / r0.xy;
                o.texcoord.xy = v.texcoord;
                r0.x = asfloat((0.0 >= v.texcoord1.y) ? 0xffffffffu : 0u);
                r0.x = asfloat(asuint(r0.x) & 0x3f800000u);
                r0.y = -_WeightNormal + _WeightBold;
                r0.x = r0.x * r0.y + _WeightNormal;
                r0.x = r0.x * 0.25 + _FaceDilate;
                r0.x = r0.x * _ScaleRatioA;
                r0.x = -r0.x * 0.5 + 0.5;
                r1.w = r0.x * r1.x + -0.5;
                o.texcoord1.y = -r0.w * r1.x + r1.w;
                o.texcoord1.z = r0.w * r1.x + r1.w;
                o.texcoord1.xw = r1.xw;
                o.texcoord3.z = v.color.w;
                o.texcoord3.w = 0.0;
                r1 = float4(_UnderlaySoftness, _UnderlayDilate, _UnderlayOffsetX, _UnderlayOffsetY) * _ScaleRatioC;
                r0.yw = -r1.zw * _GradientScale;
                r0.yw = r0.yw / float2(_TextureWidth, _TextureHeight);
                o.texcoord3.xy = r0.yw + v.texcoord;
                r0.y = r1.x * r0.z + 1.0;
                r0.y = r0.z / r0.y;
                r0.z = r1.y * 0.5;
                r0.x = r0.x * r0.y + -0.5;
                o.texcoord4.y = -r0.z * r0.y + r0.x;
                o.texcoord4.x = r0.y;
#else
                r0.xy = v.vertex.xy + float2(_VertexOffsetX, _VertexOffsetY);
                r1 = r0.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r1 = unity_ObjectToWorld._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r1;
                r2 = r1 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r1.xyz;
                r1.xyz = -r1.xyz + _WorldSpaceCameraPos.xyz;
                r3 = r2.y * unity_MatrixVP._m01_m11_m21_m31;
                r3 = unity_MatrixVP._m00_m10_m20_m30 * r2.x + r3;
                r3 = unity_MatrixVP._m02_m12_m22_m32 * r2.z + r3;
                r2 = unity_MatrixVP._m03_m13_m23_m33 * r2.w + r3;
                o.position = r2;
                r3 = v.color * _FaceColor;
                r3.xyz = r3.w * r3.xyz;
                o.color = r3;
                r4.w = v.color.w * _OutlineColor.w;
                r4.xyz = r4.w * _OutlineColor.xyz;
                r4 = -r3 + r4;
                r0.z = dot(r1.xyz, r1.xyz);
                r0.z = rsqrt(r0.z);
                r1.xyz = r0.z * r1.xyz;
                r2.x = dot(v.normal, unity_WorldToObject._m00_m10_m20);
                r2.y = dot(v.normal, unity_WorldToObject._m01_m11_m21);
                r2.z = dot(v.normal, unity_WorldToObject._m02_m12_m22);
                r0.z = dot(r2.xyz, r2.xyz);
                r0.z = rsqrt(r0.z);
                r2.xyz = r0.z * r2.xyz;
                r0.z = dot(r2.xyz, r1.xyz);
                r1.xy = _ScreenParams.y * UNITY_MATRIX_P._m01_m11;
                r1.xy = UNITY_MATRIX_P._m00_m10 * _ScreenParams.x + r1.xy;
                r1.xy = abs(r1.xy) * float2(_ScaleX, _ScaleY);
                r1.xy = r2.w / r1.xy;
                r0.w = dot(r1.xy, r1.xy);
                r1.xy = float2(_MaskSoftnessX, _MaskSoftnessY) * 0.25 + r1.xy;
                o.texcoord2.zw = 0.25 / r1.xy;
                r0.w = rsqrt(r0.w);
                r1.x = abs(v.texcoord1.y) * _GradientScale;
                r1.y = _Sharpness + 1.0;
                r1.x = r1.y * r1.x;
                r1.y = r0.w * r1.x;
                r1.z = -_PerspectiveFilter + 1.0;
                r1.z = r1.z * abs(r1.y);
                r0.w = r0.w * r1.x + -r1.z;
                r0.z = abs(r0.z) * r0.w + r1.z;
                r0.w = asfloat((UNITY_MATRIX_P._m33 == 0.0) ? 0xffffffffu : 0u);
                r0.z = (asuint(r0.w) != 0u) ? r0.z : r1.y;
                r0.w = _OutlineSoftness * _ScaleRatioA;
                r0.w = r0.w * r0.z + 1.0;
                r1.x = r0.z / r0.w;
                r0.z = _OutlineWidth * _ScaleRatioA;
                r0.z = r0.z * 0.5;
                r0.w = r1.x * r0.z;
                r0.w = r0.w + r0.w;
                r0.w = min(r0.w, 1.0);
                r0.w = sqrt(r0.w);
                o.color1 = r0.w * r4 + r3;
                r2 = max(_ClipRect, float4(-2e+10, -2e+10, -2e+10, -2e+10));
                r2 = min(r2, float4(2e+10, 2e+10, 2e+10, 2e+10));
                r1.yz = r0.xy + -r2.xy;
                r0.xy = r0.xy * 2.0 + -r2.xy;
                o.texcoord2.xy = -r2.zw + r0.xy;
                r0.xy = -r2.xy + r2.zw;
                o.texcoord.zw = r1.yz / r0.xy;
                o.texcoord.xy = v.texcoord;
                r0.x = asfloat((0.0 >= v.texcoord1.y) ? 0xffffffffu : 0u);
                r0.x = asfloat(asuint(r0.x) & 0x3f800000u);
                r0.y = -_WeightNormal + _WeightBold;
                r0.x = r0.x * r0.y + _WeightNormal;
                r0.x = r0.x * 0.25 + _FaceDilate;
                r0.x = r0.x * _ScaleRatioA;
                r0.x = -r0.x * 0.5 + 0.5;
                r1.w = r0.x * r1.x + -0.5;
                o.texcoord1.y = -r0.z * r1.x + r1.w;
                o.texcoord1.z = r0.z * r1.x + r1.w;
                o.texcoord1.xw = r1.xw;
#endif
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3;
#if defined(UNDERLAY_ON) && !defined(UNITY_UI_ALPHACLIP) && !defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord3.xy);
                r0.x = saturate(r0.w * inp.texcoord4.x + -inp.texcoord4.y);
                r1.xyz = _UnderlayColor.w * _UnderlayColor.xyz;
                r1.w = _UnderlayColor.w;
                r0 = r0.x * r1;
                r1 = inp.color + -inp.color1;
                r2 = tex2D(_MainTex, inp.texcoord.xy);
                r2.xy = saturate(r2.w * inp.texcoord1.x + -inp.texcoord1.zy);
                r1 = r2.x * r1 + inp.color1;
                r3 = r2.y * r1;
                r1.x = -r1.w * r2.y + 1.0;
                r0 = r0 * r1.x + r3;
                o.sv_target = r0 * inp.texcoord3.z;
#elif !defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && !defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord.xy);
                r0.x = saturate(r0.w * inp.texcoord1.x + -inp.texcoord1.w);
                r0.y = inp.color.w * r0.x + -0.001;
                r1 = r0.x * inp.color;
                o.sv_target = r1;
                r0.x = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#elif !defined(UNDERLAY_ON) && !defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0.xy = -_ClipRect.xy + _ClipRect.zw;
                r0.xy = r0.xy + -abs(inp.texcoord2.xy);
                r0.xy = saturate(r0.xy * inp.texcoord2.zw);
                r0.x = r0.y * r0.x;
                r1 = tex2D(_MainTex, inp.texcoord.xy);
                r0.y = saturate(r1.w * inp.texcoord1.x + -inp.texcoord1.w);
                r1 = r0.y * inp.color;
                o.sv_target = r0.x * r1;
#elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && !defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord3.xy);
                r0.x = saturate(r0.w * inp.texcoord4.x + -inp.texcoord4.y);
                r1.xyz = _UnderlayColor.w * _UnderlayColor.xyz;
                r1.w = _UnderlayColor.w;
                r0 = r0.x * r1;
                r1 = inp.color + -inp.color1;
                r2 = tex2D(_MainTex, inp.texcoord.xy);
                r2.xy = saturate(r2.w * inp.texcoord1.x + -inp.texcoord1.zy);
                r1 = r2.x * r1 + inp.color1;
                r3 = r2.y * r1;
                r1.x = -r1.w * r2.y + 1.0;
                r0 = r0 * r1.x + r3;
                r1.x = r0.w * inp.texcoord3.z + -0.001;
                r0 = r0 * inp.texcoord3.z;
                o.sv_target = r0;
                r0.x = asfloat((r1.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#elif defined(UNDERLAY_ON) && !defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord3.xy);
                r0.x = saturate(r0.w * inp.texcoord4.x + -inp.texcoord4.y);
                r1.xyz = _UnderlayColor.w * _UnderlayColor.xyz;
                r1.w = _UnderlayColor.w;
                r0 = r0.x * r1;
                r1 = inp.color + -inp.color1;
                r2 = tex2D(_MainTex, inp.texcoord.xy);
                r2.xy = saturate(r2.w * inp.texcoord1.x + -inp.texcoord1.zy);
                r1 = r2.x * r1 + inp.color1;
                r3 = r2.y * r1;
                r1.x = -r1.w * r2.y + 1.0;
                r0 = r0 * r1.x + r3;
                r1.xy = -_ClipRect.xy + _ClipRect.zw;
                r1.xy = r1.xy + -abs(inp.texcoord2.xy);
                r1.xy = saturate(r1.xy * inp.texcoord2.zw);
                r1.x = r1.y * r1.x;
                r0 = r0 * r1.x;
                o.sv_target = r0 * inp.texcoord3.z;
#elif !defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0.xy = -_ClipRect.xy + _ClipRect.zw;
                r0.xy = r0.xy + -abs(inp.texcoord2.xy);
                r0.xy = saturate(r0.xy * inp.texcoord2.zw);
                r0.x = r0.y * r0.x;
                r1 = tex2D(_MainTex, inp.texcoord.xy);
                r0.y = saturate(r1.w * inp.texcoord1.x + -inp.texcoord1.w);
                r1 = r0.y * inp.color;
                r0.y = r1.w * r0.x + -0.001;
                r1 = r0.x * r1;
                o.sv_target = r1;
                r0.x = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord3.xy);
                r0.x = saturate(r0.w * inp.texcoord4.x + -inp.texcoord4.y);
                r1.xyz = _UnderlayColor.w * _UnderlayColor.xyz;
                r1.w = _UnderlayColor.w;
                r0 = r0.x * r1;
                r1 = inp.color + -inp.color1;
                r2 = tex2D(_MainTex, inp.texcoord.xy);
                r2.xy = saturate(r2.w * inp.texcoord1.x + -inp.texcoord1.zy);
                r1 = r2.x * r1 + inp.color1;
                r3 = r2.y * r1;
                r1.x = -r1.w * r2.y + 1.0;
                r0 = r0 * r1.x + r3;
                r1.xy = -_ClipRect.xy + _ClipRect.zw;
                r1.xy = r1.xy + -abs(inp.texcoord2.xy);
                r1.xy = saturate(r1.xy * inp.texcoord2.zw);
                r1.x = r1.y * r1.x;
                r0 = r0 * r1.x;
                r1.x = r0.w * inp.texcoord3.z + -0.001;
                r0 = r0 * inp.texcoord3.z;
                o.sv_target = r0;
                r0.x = asfloat((r1.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#else
                r0 = tex2D(_MainTex, inp.texcoord.xy);
                r0.x = saturate(r0.w * inp.texcoord1.x + -inp.texcoord1.w);
                o.sv_target = r0.x * inp.color;
#endif
                return o;
            }
            ENDCG
        }
    }
}
