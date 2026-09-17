Shader "Custom/TrueDepthBalloon"
{
    Properties
    {
        // ========================================================
        // BALLOON APPEARANCE
        // ========================================================

        [Header(Balloon Appearance)]

        _MainTex(
            "Balloon Texture",
            2D
        ) = "white" {}


        _Color(
            "Balloon Tint",
            Color
        ) = (1,1,1,1)


        _Brightness(
            "Brightness",
            Range(0.1,3.0)
        ) = 1.0


        _Metallic(
            "Metallic",
            Range(0,1)
        ) = 0.0


        _Glossiness(
            "Smoothness",
            Range(0,1)
        ) = 0.5


        [HDR]
        _EmissionColor(
            "Emission",
            Color
        ) = (0,0,0,1)


        // ========================================================
        // TEXTURE COLOR CONTROL
        // ========================================================

        [Header(Texture Color)]

        _TextureColorInfluence(
            "Texture RGB Influence",
            Range(0,1)
        ) = 1.0


        // ========================================================
        // TRUEDEPTH
        // ========================================================

        [Header(TrueDepth)]

        _MaskStrength(
            "Person Mask Strength",
            Range(0.5,2.0)
        ) = 1.0
    }


    SubShader
    {
        // ========================================================
        // IMPORTANT
        //
        // Balloon is now treated as a SOLID object.
        // No transparent blending between front/back faces.
        // ========================================================

        Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "IgnoreProjector" = "True"
        }


        LOD 300


        // Only render the outward-facing side.
        Cull Back


        // Solid balloons should write depth.
        ZWrite On


        CGPROGRAM


        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        #pragma target 3.0

        #pragma multi_compile_instancing


        #include "UnityCG.cginc"

        #include "CorrectedDepthCommon.cginc"


        // ========================================================
        // MATERIAL
        // ========================================================

        sampler2D _MainTex;


        fixed4 _Color;


        half _Brightness;

        half _Metallic;

        half _Glossiness;

        half4 _EmissionColor;


        float _TextureColorInfluence;

        float _MaskStrength;


        // ========================================================
        // INPUT
        // ========================================================

        struct Input
        {
            float2 uv_MainTex;

            float4 color : COLOR;

            float4 screenPos;

            float3 worldPos;
        };


        // ========================================================
        // VERTEX
        // ========================================================

        void vert(
            inout appdata_full v,
            out Input o
        )
        {
            UNITY_INITIALIZE_OUTPUT(
                Input,
                o
            );


            o.color =
                v.color;
        }


        // ========================================================
        // 4x4 DITHER
        //
        // Used ONLY at the TrueDepth transition.
        //
        // This lets us avoid making the entire balloon
        // semi-transparent.
        // ========================================================

        float Dither4x4(
            float2 screenPosition
        )
        {
            int x =
                (int)fmod(
                    screenPosition.x,
                    4.0
                );


            int y =
                (int)fmod(
                    screenPosition.y,
                    4.0
                );


            int index =
                x +
                y * 4;


            float threshold;


            if (index == 0)       threshold = 1.0 / 17.0;
            else if (index == 1)  threshold = 9.0 / 17.0;
            else if (index == 2)  threshold = 3.0 / 17.0;
            else if (index == 3)  threshold = 11.0 / 17.0;

            else if (index == 4)  threshold = 13.0 / 17.0;
            else if (index == 5)  threshold = 5.0 / 17.0;
            else if (index == 6)  threshold = 15.0 / 17.0;
            else if (index == 7)  threshold = 7.0 / 17.0;

            else if (index == 8)  threshold = 4.0 / 17.0;
            else if (index == 9)  threshold = 12.0 / 17.0;
            else if (index == 10) threshold = 2.0 / 17.0;
            else if (index == 11) threshold = 10.0 / 17.0;

            else if (index == 12) threshold = 16.0 / 17.0;
            else if (index == 13) threshold = 8.0 / 17.0;
            else if (index == 14) threshold = 14.0 / 17.0;
            else                  threshold = 6.0 / 17.0;


            return threshold;
        }


        // ========================================================
        // SURFACE
        // ========================================================

        void surf(
            Input IN,
            inout SurfaceOutputStandard o
        )
        {
            // ----------------------------------------------------
            // Texture
            // ----------------------------------------------------

            fixed4 textureColor =
                tex2D(
                    _MainTex,
                    IN.uv_MainTex
                );


            // ----------------------------------------------------
            // OPTIONAL TEXTURE RGB
            //
            // 1 = use texture's original colors
            // 0 = ignore texture RGB
            //
            // Alpha still comes from texture.
            // ----------------------------------------------------

            fixed3 textureRGB =
                lerp(
                    fixed3(
                        1.0,
                        1.0,
                        1.0
                    ),

                    textureColor.rgb,

                    _TextureColorInfluence
                );


            fixed3 finalRGB =
                textureRGB
                *
                _Color.rgb
                *
                IN.color.rgb;


            float sourceAlpha =
                textureColor.a
                *
                _Color.a
                *
                IN.color.a;


            // Don't render transparent parts of the source texture.
            clip(
                sourceAlpha -
                0.01
            );


            // ====================================================
            // TRUEDEPTH
            // ====================================================

            float personMask =
                CorrectedDepthPersonMask(
                    IN.screenPos,
                    IN.worldPos
                );


            personMask =
                saturate(
                    personMask *
                    _MaskStrength
                );


            float visibility =
                1.0 -
                personMask;


            // ====================================================
            // DITHERED TRUEDEPTH CUT
            //
            // Instead of making the balloon semi-transparent,
            // pixels are either fully present or fully absent.
            // ====================================================

            float2 pixelPosition =
                (
                    IN.screenPos.xy /
                    IN.screenPos.w
                )
                *
                _ScreenParams.xy;


            float dither =
                Dither4x4(
                    pixelPosition
                );


            clip(
                visibility -
                dither
            );


            // ====================================================
            // SOLID BALLOON APPEARANCE
            // ====================================================

            o.Albedo =
                finalRGB *
                _Brightness;


            o.Metallic =
                _Metallic;


            o.Smoothness =
                _Glossiness;


            o.Occlusion =
                1.0;


            o.Emission =
                finalRGB *
                _EmissionColor.rgb;


            // Balloon itself is fully opaque.
            o.Alpha =
                1.0;
        }


        ENDCG
    }


    FallBack "Diffuse"
}