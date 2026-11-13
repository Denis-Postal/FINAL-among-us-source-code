// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Hidden/LightCutaway"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightRadius ("Light Radius", Float) = 0.2
        [Toggle(EDGE_BLUR)] _EdgeBlur ("Multi-sample shadow blurring", Float) = 0
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "Queue"="Transparent+11" "RenderType"="TransparentCutout" }
        Pass
        {
            Tags { "IgnoreProjector"="True" "Queue"="Transparent+11" "RenderType"="TransparentCutout" }
            Blend Zero SrcAlpha
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_local _ EDGE_BLUR
            #pragma multi_compile_local _ FLASHLIGHT
            #pragma multi_compile_local _ GPU_SHADOW
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float4 texcoord1 : TEXCOORD1;
                float3 texcoord2 : TEXCOORD2;
                float3 texcoord3 : TEXCOORD3;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _LightPosition;
            float _LightRadius;
            float _PlayerRadius;
            float4 _LightOffset;
            float _FlashlightSize;
            float _FlashlightAngle;
            float4 _TexelSize;
            float _DepthCompressionValue;
            float4 _ShmapTexture_TexelSize;
            sampler2D _ShmapTexture;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
#if defined(GPU_SHADOW)
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r1.xy = unity_ObjectToWorld._m03_m13 * v.vertex.w + r0.xy;
                o.texcoord1.zw = r1.xy + -_LightPosition.xy;
                o.texcoord1.xy = v.texcoord;
                r1 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3 = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r0.xyz;
                r0 = r1.y * unity_MatrixVP._m01_m11_m21_m31;
                r0 = unity_MatrixVP._m00_m10_m20_m30 * r1.x + r0;
                r0 = unity_MatrixVP._m02_m12_m22_m32 * r1.z + r0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r1.w + r0;
                o.texcoord2 = _LightPosition.xyz;
#else
                o.texcoord1.xy = v.texcoord;
                o.texcoord1.zw = v.vertex.xy;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r1 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3 = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r0.xyz;
                r0 = r1.y * unity_MatrixVP._m01_m11_m21_m31;
                r0 = unity_MatrixVP._m00_m10_m20_m30 * r1.x + r0;
                r0 = unity_MatrixVP._m02_m12_m22_m32 * r1.z + r0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r1.w + r0;
                o.texcoord2 = _LightPosition.xyz;
#endif
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1, r2, r3;
#if defined(EDGE_BLUR) && !defined(FLASHLIGHT) && !defined(GPU_SHADOW)
                r0.x = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.x = sqrt(r0.x);
                r0.x = r0.x / _LightRadius;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif !defined(EDGE_BLUR) && defined(FLASHLIGHT) && !defined(GPU_SHADOW)
                r0.x = 0.0;
                r0.y = _LightRadius;
                r0.z = _FlashlightAngle * 0.15915495 + 0.25;
                r0.z = r0.z * 6.283184;
                r1.x = sin(r0.z);
                r2.x = cos(r0.z);
                r0.zw = -inp.texcoord1.zw + inp.texcoord2.xy;
                r0.zw = r0.zw + -_LightPosition.xy;
                r0.zw = r0.zw + -_LightOffset.xy;
                r1.xy = r0.wz * r1.x;
                r1.x = r2.x * r0.z + r1.x;
                r2.y = r2.x * r0.w + -r1.y;
                r0.z = dot(r0.zw, r0.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z + -_PlayerRadius;
                r2.x = abs(r1.x);
                r0.xy = -r0.xy + r2.xy;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.x = r0.x + -_FlashlightSize;
                r0.y = _PlayerRadius * 0.85 + -_FlashlightSize;
                r1.z = r0.y / _LightRadius;
                r1.x = -r1.z;
                r0.y = -r1.z * r1.z + 1.0;
                r1.y = sqrt(r0.y);
                r0.y = dot(r2.xy, r1.xy);
                r0.w = r1.y * _LightRadius;
                r1.x = dot(r2.xy, r1.yz);
                r1.y = dot(r2.xy, r2.xy);
                r1.y = sqrt(r1.y);
                r1.y = -_PlayerRadius * 0.85 + r1.y;
                r1.x = -_PlayerRadius * 0.85 + r1.x;
                r0.w = asfloat((r0.w < r0.y) ? 0xffffffffu : 0u);
                r0.y = asfloat((r0.y >= 0.0) ? 0xffffffffu : 0u);
                r0.x = (asuint(r0.w) != 0u) ? r0.x : r1.x;
                r0.x = (asuint(r0.y) != 0u) ? r0.x : r1.y;
                r0.x = min(r0.z, r0.x);
                r0.x = saturate(r0.x * 1e+01);
                r0.y = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.y = sqrt(r0.y);
                r0.y = r0.y / _LightRadius;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif !defined(EDGE_BLUR) && !defined(FLASHLIGHT) && defined(GPU_SHADOW)
                r0.xyz = inp.texcoord2 + -inp.texcoord3;
                r0.z = dot(r0.xyz, r0.xyz);
                r0.z = rsqrt(r0.z);
                r0.zw = r0.z * r0.yx;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.y = max(abs(r0.w), abs(r0.z));
                r0.y = 1.0 / r0.y;
                r1.x = min(abs(r0.w), abs(r0.z));
                r0.y = r0.y * r1.x;
                r1.x = r0.y * r0.y;
                r1.y = r1.x * 0.0208351 + -0.085133;
                r1.y = r1.x * r1.y + 0.180141;
                r1.y = r1.x * r1.y + -0.3302995;
                r1.x = r1.x * r1.y + 0.999866;
                r1.y = r0.y * r1.x;
                r1.y = r1.y * -2.0 + 1.5707964;
                r1.z = asfloat((abs(r0.w) < abs(r0.z)) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.z) & asuint(r1.y));
                r0.y = r0.y * r1.x + r1.y;
                r1.x = asfloat((-r0.w < r0.w) ? 0xffffffffu : 0u);
                r1.x = asfloat(asuint(r1.x) & 0xc0490fdbu);
                r0.y = r0.y + r1.x;
                r1.x = min(-r0.w, -r0.z);
                r0.z = max(-r0.w, -r0.z);
                r0.z = asfloat((r0.z >= -r0.z) ? 0xffffffffu : 0u);
                r0.w = asfloat((r1.x < -r1.x) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & asuint(r0.w));
                r0.y = (asuint(r0.z) != 0u) ? -r0.y : r0.y;
                r0.z = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                r0.w = r0.y + 6.283185;
                r0.y = (asuint(r0.z) != 0u) ? r0.w : r0.y;
                r1.y = r0.y * 0.15915495;
                r1.x = _TexelSize.x * 1.5;
                r1 = tex2D(_ShmapTexture, r1.xy);
                r0.x = -r1.x * _DepthCompressionValue + r0.x;
                r0.x = max(r0.x, 0.0);
                r0.x = r0.x * 1e+01;
                r0.x = min(r0.x, 1.0);
                r0.y = -r0.x + 0.99;
                r0.y = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) { discard; }
                r0.y = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.y = sqrt(r0.y);
                r0.y = r0.y / _LightRadius;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif defined(EDGE_BLUR) && defined(FLASHLIGHT) && !defined(GPU_SHADOW)
                r0.x = 0.0;
                r0.y = _LightRadius;
                r0.z = _FlashlightAngle * 0.15915495 + 0.25;
                r0.z = r0.z * 6.283184;
                r1.x = sin(r0.z);
                r2.x = cos(r0.z);
                r0.zw = -inp.texcoord1.zw + inp.texcoord2.xy;
                r0.zw = r0.zw + -_LightPosition.xy;
                r0.zw = r0.zw + -_LightOffset.xy;
                r1.xy = r0.wz * r1.x;
                r1.x = r2.x * r0.z + r1.x;
                r2.y = r2.x * r0.w + -r1.y;
                r0.z = dot(r0.zw, r0.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z + -_PlayerRadius;
                r2.x = abs(r1.x);
                r0.xy = -r0.xy + r2.xy;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.x = r0.x + -_FlashlightSize;
                r0.y = _PlayerRadius * 0.85 + -_FlashlightSize;
                r1.z = r0.y / _LightRadius;
                r1.x = -r1.z;
                r0.y = -r1.z * r1.z + 1.0;
                r1.y = sqrt(r0.y);
                r0.y = dot(r2.xy, r1.xy);
                r0.w = r1.y * _LightRadius;
                r1.x = dot(r2.xy, r1.yz);
                r1.y = dot(r2.xy, r2.xy);
                r1.y = sqrt(r1.y);
                r1.y = -_PlayerRadius * 0.85 + r1.y;
                r1.x = -_PlayerRadius * 0.85 + r1.x;
                r0.w = asfloat((r0.w < r0.y) ? 0xffffffffu : 0u);
                r0.y = asfloat((r0.y >= 0.0) ? 0xffffffffu : 0u);
                r0.x = (asuint(r0.w) != 0u) ? r0.x : r1.x;
                r0.x = (asuint(r0.y) != 0u) ? r0.x : r1.y;
                r0.x = min(r0.z, r0.x);
                r0.x = saturate(r0.x * 1e+01);
                r0.y = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.y = sqrt(r0.y);
                r0.y = r0.y / _LightRadius;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif defined(EDGE_BLUR) && !defined(FLASHLIGHT) && defined(GPU_SHADOW)
                r0.xyz = inp.texcoord2 + -inp.texcoord3;
                r0.z = dot(r0.xyz, r0.xyz);
                r0.z = rsqrt(r0.z);
                r0.zw = r0.z * r0.yx;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.y = max(abs(r0.w), abs(r0.z));
                r0.y = 1.0 / r0.y;
                r1.x = min(abs(r0.w), abs(r0.z));
                r0.y = r0.y * r1.x;
                r1.x = r0.y * r0.y;
                r1.y = r1.x * 0.0208351 + -0.085133;
                r1.y = r1.x * r1.y + 0.180141;
                r1.y = r1.x * r1.y + -0.3302995;
                r1.x = r1.x * r1.y + 0.999866;
                r1.y = r0.y * r1.x;
                r1.y = r1.y * -2.0 + 1.5707964;
                r1.z = asfloat((abs(r0.w) < abs(r0.z)) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.z) & asuint(r1.y));
                r0.y = r0.y * r1.x + r1.y;
                r1.x = asfloat((-r0.w < r0.w) ? 0xffffffffu : 0u);
                r1.x = asfloat(asuint(r1.x) & 0xc0490fdbu);
                r0.y = r0.y + r1.x;
                r1.x = min(-r0.w, -r0.z);
                r0.z = max(-r0.w, -r0.z);
                r0.z = asfloat((r0.z >= -r0.z) ? 0xffffffffu : 0u);
                r0.w = asfloat((r1.x < -r1.x) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & asuint(r0.w));
                r0.y = (asuint(r0.z) != 0u) ? -r0.y : r0.y;
                r0.z = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                r0.w = r0.y + 6.283185;
                r0.y = (asuint(r0.z) != 0u) ? r0.w : r0.y;
                r1.y = r0.y * 0.15915495;
                r1.x = _TexelSize.x * 1.5;
                r2 = tex2D(_ShmapTexture, r1.xy);
                r0.y = -r2.x * _DepthCompressionValue + r0.x;
                r0.yw = max(r0.yx, float2(0.0, 1.0));
                r0.z = _ShmapTexture_TexelSize.y * 5.0;
                r2.x = r0.z / r0.w;
                r2.y = 0.0;
                r0.zw = r1.xy + r2.yx;
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.yz = r0.yz * 1e+01;
                r0.yz = min(r0.yz, float2(1.0, 1.0));
                r0.y = r0.z + r0.y;
                r0.zw = r1.xy + -r2.yx;
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.z = r0.z * 1e+01;
                r0.z = min(r0.z, 1.0);
                r0.y = r0.z + r0.y;
                r2.zw = r2.x * 2.0;
                r0.zw = r1.xy + r2.yz;
                r1.zw = r1.xy + -r2.yw;
                r3 = tex2D(_ShmapTexture, r1.zw);
                r1.z = -r3.x * _DepthCompressionValue + r0.x;
                r1.z = max(r1.z, 0.0);
                r1.z = r1.z * 1e+01;
                r1.z = min(r1.z, 1.0);
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.z = r0.z * 1e+01;
                r0.z = min(r0.z, 1.0);
                r0.y = r0.z + r0.y;
                r0.y = r1.z + r0.y;
                r0.zw = r2.yx * float2(1.0, 3.0) + r1.xy;
                r1.xy = -r2.yx * float2(1.0, 3.0) + r1.xy;
                r1 = tex2D(_ShmapTexture, r1.xy);
                r1.x = -r1.x * _DepthCompressionValue + r0.x;
                r1.x = max(r1.x, 0.0);
                r1.x = r1.x * 1e+01;
                r1.x = min(r1.x, 1.0);
                r2 = tex2D(_ShmapTexture, r0.zw);
                r0.x = -r2.x * _DepthCompressionValue + r0.x;
                r0.x = max(r0.x, 0.0);
                r0.x = r0.x * 1e+01;
                r0.x = min(r0.x, 1.0);
                r0.x = r0.x + r0.y;
                r0.x = r1.x + r0.x;
                r0.y = -r0.x * 0.14285715 + 0.99;
                r0.x = r0.x * 0.14285715;
                r0.y = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) { discard; }
                r0.y = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.y = sqrt(r0.y);
                r0.y = r0.y / _LightRadius;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif !defined(EDGE_BLUR) && defined(FLASHLIGHT) && defined(GPU_SHADOW)
                r0.xyz = inp.texcoord2 + -inp.texcoord3;
                r0.z = dot(r0.xyz, r0.xyz);
                r0.z = rsqrt(r0.z);
                r0.zw = r0.z * r0.yx;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.y = max(abs(r0.w), abs(r0.z));
                r0.y = 1.0 / r0.y;
                r1.x = min(abs(r0.w), abs(r0.z));
                r0.y = r0.y * r1.x;
                r1.x = r0.y * r0.y;
                r1.y = r1.x * 0.0208351 + -0.085133;
                r1.y = r1.x * r1.y + 0.180141;
                r1.y = r1.x * r1.y + -0.3302995;
                r1.x = r1.x * r1.y + 0.999866;
                r1.y = r0.y * r1.x;
                r1.y = r1.y * -2.0 + 1.5707964;
                r1.z = asfloat((abs(r0.w) < abs(r0.z)) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.z) & asuint(r1.y));
                r0.y = r0.y * r1.x + r1.y;
                r1.x = asfloat((-r0.w < r0.w) ? 0xffffffffu : 0u);
                r1.x = asfloat(asuint(r1.x) & 0xc0490fdbu);
                r0.y = r0.y + r1.x;
                r1.x = min(-r0.w, -r0.z);
                r0.z = max(-r0.w, -r0.z);
                r0.z = asfloat((r0.z >= -r0.z) ? 0xffffffffu : 0u);
                r0.w = asfloat((r1.x < -r1.x) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & asuint(r0.w));
                r0.y = (asuint(r0.z) != 0u) ? -r0.y : r0.y;
                r0.z = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                r0.w = r0.y + 6.283185;
                r0.y = (asuint(r0.z) != 0u) ? r0.w : r0.y;
                r1.y = r0.y * 0.15915495;
                r1.x = _TexelSize.x * 1.5;
                r1 = tex2D(_ShmapTexture, r1.xy);
                r0.x = -r1.x * _DepthCompressionValue + r0.x;
                r0.x = max(r0.x, 0.0);
                r0.x = r0.x * 1e+01;
                r0.x = min(r0.x, 1.0);
                r0.y = -r0.x + 0.99;
                r0.y = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) { discard; }
                r1.x = 0.0;
                r1.y = _LightRadius;
                r0.y = _FlashlightAngle * 0.15915495 + 0.25;
                r0.y = r0.y * 6.283184;
                r2.x = sin(r0.y);
                r3.x = cos(r0.y);
                r0.yz = -inp.texcoord1.zw + inp.texcoord2.xy;
                r0.yz = r0.yz + -_LightPosition.xy;
                r0.yz = r0.yz + -_LightOffset.xy;
                r1.zw = r0.zy * r2.x;
                r0.w = r3.x * r0.y + r1.z;
                r2.y = r3.x * r0.z + -r1.w;
                r0.y = dot(r0.yz, r0.yz);
                r0.y = sqrt(r0.y);
                r0.y = r0.y + -_PlayerRadius;
                r2.x = abs(r0.w);
                r0.zw = -r1.xy + r2.xy;
                r0.z = dot(r0.zw, r0.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z + -_FlashlightSize;
                r0.w = _PlayerRadius * 0.85 + -_FlashlightSize;
                r1.z = r0.w / _LightRadius;
                r1.x = -r1.z;
                r0.w = -r1.z * r1.z + 1.0;
                r1.y = sqrt(r0.w);
                r0.w = dot(r2.xy, r1.xy);
                r1.x = r1.y * _LightRadius;
                r1.y = dot(r2.xy, r1.yz);
                r1.z = dot(r2.xy, r2.xy);
                r1.z = sqrt(r1.z);
                r1.z = -_PlayerRadius * 0.85 + r1.z;
                r1.y = -_PlayerRadius * 0.85 + r1.y;
                r1.x = asfloat((r1.x < r0.w) ? 0xffffffffu : 0u);
                r0.w = asfloat((r0.w >= 0.0) ? 0xffffffffu : 0u);
                r0.z = (asuint(r1.x) != 0u) ? r0.z : r1.y;
                r0.z = (asuint(r0.w) != 0u) ? r0.z : r1.z;
                r0.y = min(r0.y, r0.z);
                r0.y = saturate(r0.y * 1e+01);
                r0.z = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z / _LightRadius;
                r0.w = -r0.z + 1.0;
                r0.y = r0.y * r0.w + r0.z;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#elif defined(EDGE_BLUR) && defined(FLASHLIGHT) && defined(GPU_SHADOW)
                r0.xyz = inp.texcoord2 + -inp.texcoord3;
                r0.z = dot(r0.xyz, r0.xyz);
                r0.z = rsqrt(r0.z);
                r0.zw = r0.z * r0.yx;
                r0.x = dot(r0.xy, r0.xy);
                r0.x = sqrt(r0.x);
                r0.y = max(abs(r0.w), abs(r0.z));
                r0.y = 1.0 / r0.y;
                r1.x = min(abs(r0.w), abs(r0.z));
                r0.y = r0.y * r1.x;
                r1.x = r0.y * r0.y;
                r1.y = r1.x * 0.0208351 + -0.085133;
                r1.y = r1.x * r1.y + 0.180141;
                r1.y = r1.x * r1.y + -0.3302995;
                r1.x = r1.x * r1.y + 0.999866;
                r1.y = r0.y * r1.x;
                r1.y = r1.y * -2.0 + 1.5707964;
                r1.z = asfloat((abs(r0.w) < abs(r0.z)) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.z) & asuint(r1.y));
                r0.y = r0.y * r1.x + r1.y;
                r1.x = asfloat((-r0.w < r0.w) ? 0xffffffffu : 0u);
                r1.x = asfloat(asuint(r1.x) & 0xc0490fdbu);
                r0.y = r0.y + r1.x;
                r1.x = min(-r0.w, -r0.z);
                r0.z = max(-r0.w, -r0.z);
                r0.z = asfloat((r0.z >= -r0.z) ? 0xffffffffu : 0u);
                r0.w = asfloat((r1.x < -r1.x) ? 0xffffffffu : 0u);
                r0.z = asfloat(asuint(r0.z) & asuint(r0.w));
                r0.y = (asuint(r0.z) != 0u) ? -r0.y : r0.y;
                r0.z = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                r0.w = r0.y + 6.283185;
                r0.y = (asuint(r0.z) != 0u) ? r0.w : r0.y;
                r1.y = r0.y * 0.15915495;
                r1.x = _TexelSize.x * 1.5;
                r2 = tex2D(_ShmapTexture, r1.xy);
                r0.y = -r2.x * _DepthCompressionValue + r0.x;
                r0.yw = max(r0.yx, float2(0.0, 1.0));
                r0.z = _ShmapTexture_TexelSize.y * 5.0;
                r2.x = r0.z / r0.w;
                r2.y = 0.0;
                r0.zw = r1.xy + r2.yx;
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.yz = r0.yz * 1e+01;
                r0.yz = min(r0.yz, float2(1.0, 1.0));
                r0.y = r0.z + r0.y;
                r0.zw = r1.xy + -r2.yx;
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.z = r0.z * 1e+01;
                r0.z = min(r0.z, 1.0);
                r0.y = r0.z + r0.y;
                r2.w = r2.x * 2.0;
                r0.zw = r1.xy + r2.yw;
                r3 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r3.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.z = r0.z * 1e+01;
                r0.z = min(r0.z, 1.0);
                r0.y = r0.z + r0.y;
                r3 = -r2.yxyx * float4(1.0, 2.0, 1.0, 3.0) + r1.xyxy;
                r0.zw = r2.yx * float2(1.0, 3.0) + r1.xy;
                r1 = tex2D(_ShmapTexture, r0.zw);
                r0.z = -r1.x * _DepthCompressionValue + r0.x;
                r0.z = max(r0.z, 0.0);
                r0.z = r0.z * 1e+01;
                r1 = tex2D(_ShmapTexture, r3.xy);
                r3 = tex2D(_ShmapTexture, r3.zw);
                r0.w = -r3.x * _DepthCompressionValue + r0.x;
                r0.x = -r1.x * _DepthCompressionValue + r0.x;
                r0.x = max(r0.x, 0.0);
                r0.x = r0.x * 1e+01;
                r0.xz = min(r0.xz, float2(1.0, 1.0));
                r0.x = r0.x + r0.y;
                r0.x = r0.z + r0.x;
                r0.y = max(r0.w, 0.0);
                r0.y = r0.y * 1e+01;
                r0.y = min(r0.y, 1.0);
                r0.x = r0.y + r0.x;
                r0.y = -r0.x * 0.14285715 + 0.99;
                r0.y = asfloat((r0.y < 0.0) ? 0xffffffffu : 0u);
                if (asuint(r0.y) != 0u) { discard; }
                r2.z = _LightRadius;
                r0.y = _FlashlightAngle * 0.15915495 + 0.25;
                r0.xy = r0.xy * float2(0.14285715, 6.283184);
                r1.x = sin(r0.y);
                r2.x = cos(r0.y);
                r0.yz = -inp.texcoord1.zw + inp.texcoord2.xy;
                r0.yz = r0.yz + -_LightPosition.xy;
                r0.yz = r0.yz + -_LightOffset.xy;
                r1.xy = r0.zy * r1.x;
                r0.w = r2.x * r0.y + r1.x;
                r1.y = r2.x * r0.z + -r1.y;
                r0.y = dot(r0.yz, r0.yz);
                r0.y = sqrt(r0.y);
                r0.y = r0.y + -_PlayerRadius;
                r1.x = abs(r0.w);
                r0.zw = -r2.yz + r1.xy;
                r0.z = dot(r0.zw, r0.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z + -_FlashlightSize;
                r0.w = _PlayerRadius * 0.85 + -_FlashlightSize;
                r2.z = r0.w / _LightRadius;
                r2.x = -r2.z;
                r0.w = -r2.z * r2.z + 1.0;
                r2.y = sqrt(r0.w);
                r0.w = dot(r1.xy, r2.xy);
                r1.z = r2.y * _LightRadius;
                r1.w = dot(r1.xy, r2.yz);
                r1.x = dot(r1.xy, r1.xy);
                r1.x = sqrt(r1.x);
                r1.x = -_PlayerRadius * 0.85 + r1.x;
                r1.y = -_PlayerRadius * 0.85 + r1.w;
                r1.z = asfloat((r1.z < r0.w) ? 0xffffffffu : 0u);
                r0.w = asfloat((r0.w >= 0.0) ? 0xffffffffu : 0u);
                r0.z = (asuint(r1.z) != 0u) ? r0.z : r1.y;
                r0.z = (asuint(r0.w) != 0u) ? r0.z : r1.x;
                r0.y = min(r0.y, r0.z);
                r0.y = saturate(r0.y * 1e+01);
                r0.z = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.z = sqrt(r0.z);
                r0.z = r0.z / _LightRadius;
                r0.w = -r0.z + 1.0;
                r0.y = r0.y * r0.w + r0.z;
                r0.z = -r0.y + 1.0;
                r0.x = r0.x * r0.z + r0.y;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#else
                r0.x = dot(inp.texcoord1.zw, inp.texcoord1.zw);
                r0.x = sqrt(r0.x);
                r0.x = r0.x / _LightRadius;
                r0.x = r0.x * r0.x;
                o.sv_target.w = min(r0.x, 1.0);
                o.sv_target.xyz = 0.0;
#endif
                return o;
            }
            ENDCG
        }
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend One OneMinusSrcAlpha
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
                float3 texcoord1 : TEXCOORD1;
                float3 texcoord2 : TEXCOORD2;
                float4 color : COLOR0;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            float4 _LightPosition;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
                o.texcoord = v.texcoord;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r1 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord2 = unity_ObjectToWorld._m03_m13_m23 * v.vertex.w + r0.xyz;
                r0 = r1.y * unity_MatrixVP._m01_m11_m21_m31;
                r0 = unity_MatrixVP._m00_m10_m20_m30 * r1.x + r0;
                r0 = unity_MatrixVP._m02_m12_m22_m32 * r1.z + r0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * r1.w + r0;
                o.texcoord1.xy = _LightPosition.xy;
                o.texcoord1.z = 0.0;
                o.color = v.color;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0;
                r0 = tex2D(_MainTex, inp.texcoord);
                r0 = r0 * inp.color;
                o.sv_target.xyz = r0.w * r0.xyz;
                o.sv_target.w = r0.w;
                return o;
            }
            ENDCG
        }
    }
    SubShader
    {
        Tags { "Queue"="Transparent+10" "RenderType"="AlphaTest" }
        Pass
        {
            Tags { "Queue"="Transparent+10" "RenderType"="AlphaTest" }
            Blend One One
            BlendOp RevSub
            ColorMask A
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
                o.texcoord = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = v.color;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0;
                r0 = tex2D(_MainTex, inp.texcoord);
                o.sv_target = r0 * inp.color;
                return o;
            }
            ENDCG
        }
    }
}
