Shader "Sprites/CustomOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,0,0)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        // Настройки обводки
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(1, 5)) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            fixed4 _OutlineColor;
            float _OutlineWidth;
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f IN) : SV_Target
            {
                // Читаем базовый пиксель спрайта
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                c *= IN.color;

                // Если пиксель полностью прозрачный, ищем соседей для обводки
                if (c.a < 0.05)
                {
                    float maxAlpha = 0;
                    int maxDist = (int)_OutlineWidth;

                    [unroll]
                    for (int x = -5; x <= 5; x++)
                    {
                        [unroll]
                        for (int y = -5; y <= 5; y++)
                        {
                            if (abs(x) <= maxDist && abs(y) <= maxDist)
                            {
                                float2 uv = IN.texcoord + float2(x, y) * _MainTex_TexelSize.xy;
                                float neighborAlpha = tex2D(_MainTex, uv).a;
                                
                                if (neighborAlpha > maxAlpha)
                                {
                                    maxAlpha = neighborAlpha;
                                }
                            }
                        }
                    }

                    // Рисуем обводку только если рядом есть непрозрачная часть
                    if (maxAlpha > 0.05)
                    {
                        return _OutlineColor;
                    }
                }

                return c;
            }
        ENDCG
        }
    }
}