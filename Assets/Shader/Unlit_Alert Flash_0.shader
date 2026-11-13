// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Unlit/Alert Flash"
{
    Properties
    {
        _ColorInner ("Inner Color", Color) = (1,1,1,1)
        _ColorOuter ("Outer Color", Color) = (0,0,0,1)
        _Opacity ("Opacity", Float) = 1
        _Rad ("Radius", Float) = 1
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Mask ("Mask", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
			Cull Off
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Stencil {
                Ref 1
                Comp [_Mask]
            }
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
            float4 _ColorInner;
            float4 _ColorOuter;
            float _Opacity;
            float _Rad;
            float4 _Offset;
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
                r0.xy = inp.texcoord * 2.0 + -1.0;
                r0.xy = r0.xy + -_Offset.xy;
                r0.xy = r0.xy * r0.xy;
                r0.x = r0.y + r0.x;
                r0.x = sqrt(r0.x);
                r0.x = r0.x * _Rad;
                r1 = -_ColorInner + _ColorOuter;
                r0 = r0.x * r1 + _ColorInner;
                o.sv_target.w = r0.w * _Opacity;
                o.sv_target.xyz = r0.xyz;
                return o;
            }
            ENDCG
        }
    }
}
