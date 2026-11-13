// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "TextMeshPro/Distance Field"
{
    Properties
    {
        _FaceTex ("Face Texture", 2D) = "white" {}
        _FaceUVSpeedX ("Face UV Speed X", Range(-5,5)) = 0
        _FaceUVSpeedY ("Face UV Speed Y", Range(-5,5)) = 0
        [HDR] _FaceColor ("Face Color", Color) = (1,1,1,1)
        _FaceDilate ("Face Dilate", Range(-1,1)) = 0
        [HDR] _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineTex ("Outline Texture", 2D) = "white" {}
        _OutlineUVSpeedX ("Outline UV Speed X", Range(-5,5)) = 0
        _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5,5)) = 0
        _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
        _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
        _Bevel ("Bevel", Range(0,1)) = 0.5
        _BevelOffset ("Bevel Offset", Range(-0.5,0.5)) = 0
        _BevelWidth ("Bevel Width", Range(-0.5,0.5)) = 0
        _BevelClamp ("Bevel Clamp", Range(0,1)) = 0
        _BevelRoundness ("Bevel Roundness", Range(0,1)) = 0
        _LightAngle ("Light Angle", Range(0,6.2831855)) = 3.1416
        [HDR] _SpecularColor ("Specular", Color) = (1,1,1,1)
        _SpecularPower ("Specular", Range(0,4)) = 2
        _Reflectivity ("Reflectivity", Range(5,15)) = 10
        _Diffuse ("Diffuse", Range(0,1)) = 0.5
        _Ambient ("Ambient", Range(1,0)) = 0.5
        _BumpMap ("Normal map", 2D) = "bump" {}
        _BumpOutline ("Bump Outline", Range(0,1)) = 0
        _BumpFace ("Bump Face", Range(0,1)) = 0
        _ReflectFaceColor ("Reflection Color", Color) = (0,0,0,1)
        _ReflectOutlineColor ("Reflection Color", Color) = (0,0,0,1)
        _Cube ("Reflection Cubemap", Cube) = "black" {}
        _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
        [HDR] _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
        _UnderlayOffsetX ("Border OffsetX", Range(-1,1)) = 0
        _UnderlayOffsetY ("Border OffsetY", Range(-1,1)) = 0
        _UnderlayDilate ("Border Dilate", Range(-1,1)) = 0
        _UnderlaySoftness ("Border Softness", Range(0,1)) = 0
        [HDR] _GlowColor ("Color", Color) = (0,1,0,0.5)
        _GlowOffset ("Offset", Range(-1,1)) = 0
        _GlowInner ("Inner", Range(0,1)) = 0.05
        _GlowOuter ("Outer", Range(0,1)) = 0.05
        _GlowPower ("Falloff", Range(1,0)) = 0.75
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
        _MaskCoord ("Mask Coordinates", Vector) = (0,0,32767,32767)
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
                float2 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float3 texcoord3 : TEXCOORD3;
                float4 texcoord5 : TEXCOORD5;
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float _FaceDilate;
            float _OutlineSoftness;
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
            float4 _FaceTex_ST;
            float4 _OutlineTex_ST;
            float4x4 _EnvMatrix;
            float _FaceUVSpeedX;
            float _FaceUVSpeedY;
            float4 _FaceColor;
            float _OutlineUVSpeedX;
            float _OutlineUVSpeedY;
            float4 _OutlineColor;
            sampler2D _MainTex;
            sampler2D _FaceTex;
            sampler2D _OutlineTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1, r2, r3;
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
                o.color = v.color;
                o.texcoord = v.texcoord;
                r0.zw = _ScreenParams.y * UNITY_MATRIX_P._m01_m11;
                r0.zw = UNITY_MATRIX_P._m00_m10 * _ScreenParams.x + r0.zw;
                r0.zw = abs(r0.zw) * float2(_ScaleX, _ScaleY);
                r0.zw = r2.w / r0.zw;
                r1.w = dot(r0.zw, r0.zw);
                r0.zw = float2(_MaskSoftnessX, _MaskSoftnessY) * 0.25 + r0.zw;
                o.texcoord2.zw = 0.25 / r0.zw;
                r0.z = rsqrt(r1.w);
                r0.w = abs(v.texcoord1.y) * _GradientScale;
                r1.w = _Sharpness + 1.0;
                r0.w = r0.w * r1.w;
                r1.w = r0.w * r0.z;
                r2.x = -_PerspectiveFilter + 1.0;
                r2.x = abs(r1.w) * r2.x;
                r0.z = r0.z * r0.w + -r2.x;
                r3.x = dot(v.normal, unity_WorldToObject._m00_m10_m20);
                r3.y = dot(v.normal, unity_WorldToObject._m01_m11_m21);
                r3.z = dot(v.normal, unity_WorldToObject._m02_m12_m22);
                r0.w = dot(r3.xyz, r3.xyz);
                r0.w = rsqrt(r0.w);
                r2.yzw = r0.w * r3.xyz;
                r0.w = dot(r1.xyz, r1.xyz);
                r0.w = rsqrt(r0.w);
                r3.xyz = r0.w * r1.xyz;
                r0.w = dot(r2.yzw, r3.xyz);
                r0.z = abs(r0.w) * r0.z + r2.x;
                r0.w = asfloat((UNITY_MATRIX_P._m33 == 0.0) ? 0xffffffffu : 0u);
                r2.y = (asuint(r0.w) != 0u) ? r0.z : r1.w;
                r0.z = asfloat((0.0 >= v.texcoord1.y) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & 0x3f800000u);
                r0.w = -_WeightNormal + _WeightBold;
                r0.z = r0.z * r0.w + _WeightNormal;
                r0.z = r0.z * 0.25 + _FaceDilate;
                r0.z = r0.z * _ScaleRatioA;
                r2.w = r0.z * 0.5;
                o.texcoord1.yw = r2.yw;
                r0.w = 0.5 / r2.y;
                r1.w = -_OutlineWidth * _ScaleRatioA + 1.0;
                r1.w = -_OutlineSoftness * _ScaleRatioA + r1.w;
                r1.w = r1.w * 0.5 + -r0.w;
                o.texcoord1.x = -r0.z * 0.5 + r1.w;
                r0.z = -r0.z * 0.5 + 0.5;
                o.texcoord1.z = r0.w + r0.z;
                r2 = max(_ClipRect, float4(-2e+10, -2e+10, -2e+10, -2e+10));
                r2 = min(r2, float4(2e+10, 2e+10, 2e+10, 2e+10));
                r0.xy = r0.xy * 2.0 + -r2.xy;
                o.texcoord2.xy = -r2.zw + r0.xy;
                r0.xyz = r1.y * _EnvMatrix._m01_m11_m21;
                r0.xyz = _EnvMatrix._m00_m10_m20 * r1.x + r0.xyz;
                o.texcoord3 = _EnvMatrix._m02_m12_m22 * r1.z + r0.xyz;
                r0.x = v.texcoord1.x * 0.00024414062;
                r0.z = floor(r0.x);
                r0.w = -r0.z * 4096.0 + v.texcoord1.x;
                r0.xy = r0.zw * 0.001953125;
                o.texcoord5.xy = r0.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
                o.texcoord5.zw = r0.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3;
#if defined(UNITY_UI_ALPHACLIP) && !defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord);
                r0.x = r0.w + -inp.texcoord1.x;
                r0.y = -r0.w + inp.texcoord1.z;
                r0.x = asfloat((r0.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
                r0.x = _OutlineWidth * _ScaleRatioA;
                r0.x = r0.x * inp.texcoord1.y;
                r0.z = min(r0.x, 1.0);
                r0.x = r0.x * 0.5;
                r0.z = sqrt(r0.z);
                r0.w = saturate(r0.y * inp.texcoord1.y + r0.x);
                r0.x = r0.y * inp.texcoord1.y + -r0.x;
                r0.y = r0.z * r0.w;
                r0.zw = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y + inp.texcoord5.zw;
                r1 = tex2D(_OutlineTex, r0.zw);
                r1 = r1 * _OutlineColor;
                r1.xyz = r1.w * r1.xyz;
                r2.xyz = inp.color.xyz * _FaceColor.xyz;
                r0.zw = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y + inp.texcoord5.xy;
                r3 = tex2D(_FaceTex, r0.zw);
                r2.xyz = r2.xyz * r3.xyz;
                r3.w = r3.w * _FaceColor.w;
                r3.xyz = r2.xyz * r3.w;
                r1 = r1 + -r3;
                r1 = r0.y * r1 + r3;
                r0.y = _OutlineSoftness * _ScaleRatioA;
                r0.z = r0.y * inp.texcoord1.y;
                r0.y = r0.y * inp.texcoord1.y + 1.0;
                r0.x = r0.z * 0.5 + r0.x;
                r0.x = saturate(r0.x / r0.y);
                r0.x = -r0.x + 1.0;
                r0.y = r1.w * r0.x + -0.001;
                r1 = r0.x * r1;
                o.sv_target = r1 * inp.color.w;
                r0.x = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#elif !defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord);
                r0.x = r0.w + -inp.texcoord1.x;
                r0.y = -r0.w + inp.texcoord1.z;
                r0.x = asfloat((r0.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
                r0.x = _OutlineWidth * _ScaleRatioA;
                r0.x = r0.x * inp.texcoord1.y;
                r0.z = min(r0.x, 1.0);
                r0.x = r0.x * 0.5;
                r0.z = sqrt(r0.z);
                r0.w = saturate(r0.y * inp.texcoord1.y + r0.x);
                r0.x = r0.y * inp.texcoord1.y + -r0.x;
                r0.y = r0.z * r0.w;
                r0.zw = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y + inp.texcoord5.zw;
                r1 = tex2D(_OutlineTex, r0.zw);
                r1 = r1 * _OutlineColor;
                r1.xyz = r1.w * r1.xyz;
                r2.xyz = inp.color.xyz * _FaceColor.xyz;
                r0.zw = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y + inp.texcoord5.xy;
                r3 = tex2D(_FaceTex, r0.zw);
                r2.xyz = r2.xyz * r3.xyz;
                r3.w = r3.w * _FaceColor.w;
                r3.xyz = r2.xyz * r3.w;
                r1 = r1 + -r3;
                r1 = r0.y * r1 + r3;
                r0.y = _OutlineSoftness * _ScaleRatioA;
                r0.z = r0.y * inp.texcoord1.y;
                r0.y = r0.y * inp.texcoord1.y + 1.0;
                r0.x = r0.z * 0.5 + r0.x;
                r0.x = saturate(r0.x / r0.y);
                r0.x = -r0.x + 1.0;
                r0 = r0.x * r1;
                r1.xy = -_ClipRect.xy + _ClipRect.zw;
                r1.xy = r1.xy + -abs(inp.texcoord2.xy);
                r1.xy = saturate(r1.xy * inp.texcoord2.zw);
                r1.x = r1.y * r1.x;
                r0 = r0 * r1.x;
                o.sv_target = r0 * inp.color.w;
#elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
                r0 = tex2D(_MainTex, inp.texcoord);
                r0.x = r0.w + -inp.texcoord1.x;
                r0.y = -r0.w + inp.texcoord1.z;
                r0.x = asfloat((r0.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
                r0.x = _OutlineWidth * _ScaleRatioA;
                r0.x = r0.x * inp.texcoord1.y;
                r0.z = min(r0.x, 1.0);
                r0.x = r0.x * 0.5;
                r0.z = sqrt(r0.z);
                r0.w = saturate(r0.y * inp.texcoord1.y + r0.x);
                r0.x = r0.y * inp.texcoord1.y + -r0.x;
                r0.y = r0.z * r0.w;
                r0.zw = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y + inp.texcoord5.zw;
                r1 = tex2D(_OutlineTex, r0.zw);
                r1 = r1 * _OutlineColor;
                r1.xyz = r1.w * r1.xyz;
                r2.xyz = inp.color.xyz * _FaceColor.xyz;
                r0.zw = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y + inp.texcoord5.xy;
                r3 = tex2D(_FaceTex, r0.zw);
                r2.xyz = r2.xyz * r3.xyz;
                r3.w = r3.w * _FaceColor.w;
                r3.xyz = r2.xyz * r3.w;
                r1 = r1 + -r3;
                r1 = r0.y * r1 + r3;
                r0.y = _OutlineSoftness * _ScaleRatioA;
                r0.z = r0.y * inp.texcoord1.y;
                r0.y = r0.y * inp.texcoord1.y + 1.0;
                r0.x = r0.z * 0.5 + r0.x;
                r0.x = saturate(r0.x / r0.y);
                r0.x = -r0.x + 1.0;
                r0 = r0.x * r1;
                r1.xy = -_ClipRect.xy + _ClipRect.zw;
                r1.xy = r1.xy + -abs(inp.texcoord2.xy);
                r1.xy = saturate(r1.xy * inp.texcoord2.zw);
                r1.x = r1.y * r1.x;
                r1.y = r0.w * r1.x + -0.001;
                r0 = r0 * r1.x;
                o.sv_target = r0 * inp.color.w;
                r0.x = asfloat((r1.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
#else
                r0 = tex2D(_MainTex, inp.texcoord);
                r0.x = r0.w + -inp.texcoord1.x;
                r0.y = -r0.w + inp.texcoord1.z;
                r0.x = asfloat((r0.x < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) { discard; }
                r0.x = _OutlineWidth * _ScaleRatioA;
                r0.x = r0.x * inp.texcoord1.y;
                r0.z = min(r0.x, 1.0);
                r0.x = r0.x * 0.5;
                r0.z = sqrt(r0.z);
                r0.w = saturate(r0.y * inp.texcoord1.y + r0.x);
                r0.x = r0.y * inp.texcoord1.y + -r0.x;
                r0.y = r0.z * r0.w;
                r0.zw = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y + inp.texcoord5.zw;
                r1 = tex2D(_OutlineTex, r0.zw);
                r1 = r1 * _OutlineColor;
                r1.xyz = r1.w * r1.xyz;
                r2.xyz = inp.color.xyz * _FaceColor.xyz;
                r0.zw = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y + inp.texcoord5.xy;
                r3 = tex2D(_FaceTex, r0.zw);
                r2.xyz = r2.xyz * r3.xyz;
                r3.w = r3.w * _FaceColor.w;
                r3.xyz = r2.xyz * r3.w;
                r1 = r1 + -r3;
                r1 = r0.y * r1 + r3;
                r0.y = _OutlineSoftness * _ScaleRatioA;
                r0.z = r0.y * inp.texcoord1.y;
                r0.y = r0.y * inp.texcoord1.y + 1.0;
                r0.x = r0.z * 0.5 + r0.x;
                r0.x = saturate(r0.x / r0.y);
                r0.x = -r0.x + 1.0;
                r0 = r0.x * r1;
                o.sv_target = r0 * inp.color.w;
#endif
                return o;
            }
            ENDCG
        }
    }
}
