// Reconstructed from AssetRipper compiled-program dump (D3D11 DXBC disassembly) for Unity 6 Built-in RP.
Shader "Hidden/ProBuilder/HideVertices"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "RenderType"="Geometry" }
        Pass
        {
            Tags { "IgnoreProjector"="True" "RenderType"="Geometry" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            struct appdata
            {
            };
            struct v2f
            {
                float4 position : SV_POSITION;
            };
            struct fout
            {
                float4 sv_target : SV_Target0;
            };
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;
                o.position = 0.0;
                return o;
            }
            fout frag(v2f inp)
            {
                fout o = (fout)0;
                o.sv_target = 0.0;
                return o;
            }
            ENDCG
        }
    }
}
