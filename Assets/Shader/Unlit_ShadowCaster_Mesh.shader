// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/ShadowCaster_Mesh"
{
    Properties
    {
        _DepthCompressionValue ("Depth Compression Value", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            Tags { "RenderType"="Opaque" }
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
            #pragma geometry geom
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 normal : NORMAL0;
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float3 texcoord3 : TEXCOORD3;
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
                float sv_depth : SV_Depth;
            };
            float4 _LightPosition;
            float4 _TexelSize;
            float _Column;
            float _Radius;
            float _DepthCompressionValue;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1, r2;
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r1.x = asfloat((0.0 < v.vertex.z) ? 0xffffffffu : 0u);
                r1.y = asfloat((v.vertex.z < 0.0) ? 0xffffffffu : 0u);
                r1.x = asfloat(-asint(r1.x) + asint(r1.y));
                r1.x = (float)(asint(r1.x));
                r1.y = r1.x * 0.5;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * r1.y + r0;
                o.texcoord1.z = r1.y;
                r0 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.w + r0;
                r0 = r0 + -_LightPosition;
                r1.y = dot(r0.xy, r0.xy);
                r1.y = sqrt(r1.y);
                r1.yz = r0.yx / r1.y;
                r1.w = max(abs(r1.z), abs(r1.y));
                r1.w = 1.0 / r1.w;
                r2.x = min(abs(r1.z), abs(r1.y));
                r1.w = r1.w * r2.x;
                r2.x = r1.w * r1.w;
                r2.y = r2.x * 0.0208351 + -0.085133;
                r2.y = r2.x * r2.y + 0.180141;
                r2.y = r2.x * r2.y + -0.3302995;
                r2.x = r2.x * r2.y + 0.999866;
                r2.y = r1.w * r2.x;
                r2.y = r2.y * -2.0 + 1.5707964;
                r2.z = asfloat((abs(r1.z) < abs(r1.y)) ? 0xffffffffu : 0u);
                r2.y = asfloat(asuint(r2.z) & asuint(r2.y));
                r1.w = r1.w * r2.x + r2.y;
                r2.x = asfloat((r1.z < -r1.z) ? 0xffffffffu : 0u);
                r2.x = asfloat(asuint(r2.x) & 0xc0490fdbu);
                r1.w = r1.w + r2.x;
                r2.x = min(r1.z, r1.y);
                r1.y = max(r1.z, r1.y);
                r1.y = asfloat((r1.y >= -r1.y) ? 0xffffffffu : 0u);
                r1.z = asfloat((r2.x < -r2.x) ? 0xffffffffu : 0u);
                r1.y = asfloat(asuint(r1.y) & asuint(r1.z));
                r2.x = (asuint(r1.y) != 0u) ? -r1.w : r1.w;
                r1.y = asfloat((r2.x < 0.0) ? 0xffffffffu : 0u);
                r1.z = r2.x + 6.283185;
                r2.y = (asuint(r1.y) != 0u) ? r1.z : r2.x;
                o.texcoord2 = r2.xy;
                o.position.y = -r2.y * 0.31830996 + 1.0;
                r1.y = _Column * 3.0 + 1.5;
                r1.x = r1.x * 1.5 + r1.y;
                r1.x = dot(_TexelSize.xx, r1.xx);
                o.position.x = r1.x + -1.0;
                o.position.zw = float2(0.0, 1.0);
                o.texcoord = r0;
                o.texcoord1.xyw = v.vertex.xyw;
                r0.zw = v.normal.y * unity_ObjectToWorld._m01_m11;
                r0.zw = unity_ObjectToWorld._m00_m10 * v.normal.x + r0.zw;
                r0.zw = unity_ObjectToWorld._m02_m12 * v.normal.z + r0.zw;
                r1.x = dot(r0.zw, r0.zw);
                r1.x = rsqrt(r1.x);
                r0.zw = r0.zw * r1.x;
                o.texcoord3.z = dot(r0.zw, r0.xy);
                o.texcoord3.xy = r0.zw;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1;
                r0.x = sin(inp.texcoord2.y);
                r1.x = cos(inp.texcoord2.y);
                r1.y = r0.x;
                r0.x = dot(r1.xy, inp.texcoord3.xy);
                r0.x = inp.texcoord3.z / r0.x;
                o.sv_target.xw = r0.x / _DepthCompressionValue;
                r0.x = r0.x / _Radius;
                o.sv_depth = -r0.x + 1.0;
                o.sv_target.yz = 0.0;
                return o;
            }
            [maxvertexcount(6)]
            void geom(triangle v2f input[3], inout TriangleStream<v2f> stream)
            {
                v2f o = (v2f)0;
                float4 r0, r1, r2, r3;
                r0.xy = -input[1].texcoord2 + input[0].texcoord2;
                r0.z = -input[2].texcoord2.y + input[1].texcoord2.y;
                r0.x = asfloat((1e-06 >= abs(r0.x)) ? 0xffffffffu : 0u);
                r0.x = (asuint(r0.x) != 0u) ? abs(r0.z) : abs(r0.y);
                r0.x = asfloat((3.141592 < r0.x) ? 0xffffffffu : 0u);
                if (asuint(r0.x) != 0u) {
                r0.x = _Column * 3.0 + 1.5;
                r0.y = input[0].texcoord1.z * 3.0 + r0.x;
                r0.y = dot(_TexelSize.xx, r0.yy);
                r1.x = r0.y + -1.0;
                r0.y = -input[0].texcoord2.x * 0.31830996 + 1.0;
                r0.z = input[1].texcoord1.z * 3.0 + r0.x;
                r0.z = dot(_TexelSize.xx, r0.zz);
                r2.x = r0.z + -1.0;
                r0.z = -input[1].texcoord2.x * 0.31830996 + 1.0;
                r0.x = input[2].texcoord1.z * 3.0 + r0.x;
                r0.x = dot(_TexelSize.xx, r0.xx);
                r3.x = r0.x + -1.0;
                r0.x = -input[2].texcoord2.x * 0.31830996 + 1.0;
                o.position.x = r1.x;
                o.position.y = r0.y;
                o.position.zw = float2(0.0, 1.0);
                o.texcoord = input[0].texcoord;
                o.texcoord1 = input[0].texcoord1;
                o.texcoord2 = input[0].texcoord2.x;
                o.texcoord3 = input[0].texcoord3;
                stream.Append(o);
                o.position.x = r2.x;
                o.position.y = r0.z;
                o.position.zw = float2(0.0, 1.0);
                o.texcoord = input[1].texcoord;
                o.texcoord1 = input[1].texcoord1;
                o.texcoord2 = input[1].texcoord2.x;
                o.texcoord3 = input[1].texcoord3;
                stream.Append(o);
                o.position.x = r3.x;
                o.position.y = r0.x;
                o.position.zw = float2(0.0, 1.0);
                o.texcoord = input[2].texcoord;
                o.texcoord1 = input[2].texcoord1;
                o.texcoord2 = input[2].texcoord2.x;
                o.texcoord3 = input[2].texcoord3;
                stream.Append(o);
                stream.RestartStrip();
                r0.x = 6.283185 + input[0].texcoord2.x;
                r0.y = 6.283185 + input[1].texcoord2.x;
                r0.z = 6.283185 + input[2].texcoord2.x;
                r1.y = input[0].texcoord2.x * -0.31830996 + -1.0000004;
                r1.zw = float2(0.0, 1.0);
                r2.y = input[1].texcoord2.x * -0.31830996 + -1.0000004;
                r2.zw = float2(0.0, 1.0);
                r3.y = input[2].texcoord2.x * -0.31830996 + -1.0000004;
                r3.zw = float2(0.0, 1.0);
                } else {
                r1 = input[0].position;
                r2 = input[1].position;
                r3 = input[2].position;
                r0.x = input[0].texcoord2.y;
                r0.y = input[1].texcoord2.y;
                r0.z = input[2].texcoord2.y;
                }
                o.position = r1;
                o.texcoord = input[0].texcoord;
                o.texcoord1 = input[0].texcoord1;
                o.texcoord2.x = input[0].texcoord2.x;
                o.texcoord2.y = r0.x;
                o.texcoord3 = input[0].texcoord3;
                stream.Append(o);
                o.position = r2;
                o.texcoord = input[1].texcoord;
                o.texcoord1 = input[1].texcoord1;
                o.texcoord2.x = input[1].texcoord2.x;
                o.texcoord2.y = r0.y;
                o.texcoord3 = input[1].texcoord3;
                stream.Append(o);
                o.position = r3;
                o.texcoord = input[2].texcoord;
                o.texcoord1 = input[2].texcoord1;
                o.texcoord2.x = input[2].texcoord2.x;
                o.texcoord2.y = r0.z;
                o.texcoord3 = input[2].texcoord3;
                stream.Append(o);
            }
            ENDCG
        }
    }
}
