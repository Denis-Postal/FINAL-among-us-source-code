// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Sprites/ColorFlash"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader
    {
        Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "CanUseSpriteAtlas"="True" "IgnoreProjector"="True" "PreviewType"="Plane" "Queue"="Transparent" "RenderType"="Transparent" }
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile _ PIXELSNAP_ON
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
            float4 _Color;
            float4 _FlashColor;
            float _FlashAmount;
            sampler2D _MainTex;
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                float4 r0, r1;
#if defined(PIXELSNAP_ON)
                r0 = v.vertex.y * unity_ObjectToWorld._m01_m11_m21_m31;
                r0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.x + r0;
                r0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.z + r0;
                r0 = r0 + unity_ObjectToWorld._m03_m13_m23_m33;
                r1 = r0.y * unity_MatrixVP._m01_m11_m21_m31;
                r1 = unity_MatrixVP._m00_m10_m20_m30 * r0.x + r1;
                r1 = unity_MatrixVP._m02_m12_m22_m32 * r0.z + r1;
                r0 = unity_MatrixVP._m03_m13_m23_m33 * r0.w + r1;
                r0.xy = r0.xy / r0.w;
                r1.xy = _ScreenParams.xy * 0.5;
                r0.xy = r0.xy * r1.xy;
                r0.xy = round(r0.xy);
                r0.xy = r0.xy / r1.xy;
                o.position.xy = r0.w * r0.xy;
                o.position.zw = r0.zw;
                o.color = v.color * _Color;
                o.texcoord = v.texcoord;
#else
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
#endif
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                float4 r0, r1;
                r0 = tex2D(_MainTex, inp.texcoord);
                r1.xyz = -r0.xyz * inp.color.xyz + _FlashColor.xyz;
                r0 = r0 * inp.color;
                r0.xyz = _FlashAmount * r1.xyz + r0.xyz;
                o.sv_target.xyz = r0.w * r0.xyz;
                o.sv_target.w = r0.w;
                return o;
            }
            ENDCG
        }
    }
}
