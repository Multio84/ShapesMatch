Shader "UI/TutorialRectangularHole"
{
    Properties
    {
        _MainTex        ("Texture", 2D)                      = "white" {}
        _Color          ("Tint",    Color)                   = (0,0,0,0.7)

        // hole world position
        _HoleCenterX    ("Hole Center X (world)", Float)     = 0
        _HoleCenterY    ("Hole Center Y (world)", Float)     = 0

        // hole height and width in pixels of canvas
        _HoleWidthPx    ("Hole Width  (px)",      Float)     = 500
        _HoleHeightPx   ("Hole Height (px)",      Float)     = 300

        // hole edge softness
        _HoleSoftnessPx ("Hole Softness(px)",     Float)     = 20

        // scale of canvas
        _CanvasScaleX   ("Canvas scale X",        Float)     = 1
        _CanvasScaleY   ("Canvas scale Y",        Float)     = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f     { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION;
                             float3 worldPos : TEXCOORD1; };

            sampler2D _MainTex;      float4 _MainTex_ST;     float4 _Color;

            float _HoleCenterX,  _HoleCenterY;
            float _HoleWidthPx,  _HoleHeightPx;
            float _HoleSoftnessPx;
            float _CanvasScaleX, _CanvasScaleY;

            /* ---------- VERTEX ---------- */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv       = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            /* ---------- FRAGMENT ---------- */
            fixed4 frag (v2f i) : SV_Target
            {
                // px -> world
                float2 px2world  = float2(_CanvasScaleX, _CanvasScaleY);
                float2 halfSizeW = float2(_HoleWidthPx, _HoleHeightPx) * px2world * 0.5;
                float  softW     = _HoleSoftnessPx * _CanvasScaleX;

                float2 holeCenter = float2(_HoleCenterX, _HoleCenterY);

                
                float2 d = abs(i.worldPos.xy - holeCenter) - halfSizeW;
                float dist = max(d.x, d.y);
                float cut  = smoothstep(0.0, softW, dist);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= cut;
                return col;
            }
            ENDCG
        }
    }
}


/*
{
    Properties
    {
        _MainTex        ("Texture", 2D)                = "white" {}
        _Color          ("Tint",    Color)             = (1,1,1,1)

        // 1) ÷ентр отверсти€ (мировые координаты)
        _HoleCenter     ("Hole Center (world)", Vector)= (0,0,0,0)

        // 2) –азмеры окна Ч ¬ ѕ» —≈Ћя’ CanvasТа
        _HoleSizePx     ("Hole Size (px WxH)", Vector) = (500,300,0,0)

        // 3) ѕлавность границы Ч в тех же px
        _HoleSoftnessPx ("Hole Softness (px)", Float)  = 20

        // 4) ћасштаб корневого CanvasТа
        _CanvasScale    ("Canvas lossyScale", Vector)  = (0.01,0.01,0,0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"
               "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend  SrcAlpha OneMinusSrcAlpha
            Cull   Off

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv        : TEXCOORD0;
                float4 vertex    : SV_POSITION;
                float3 worldPos  : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _Color;

            float3 _HoleCenter;       // world
            float2 _HoleSizePx;       // px (ширина, высота)
            float  _HoleSoftnessPx;   // px
            float2 _CanvasScale;      // lossyScale X,Y

            //------------------------- VERTEX -------------------------
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv       = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            //------------------------ FRAGMENT ------------------------
            fixed4 frag (v2f i) : SV_Target
            {
                // 1) ѕереводим размеры / м€гкость из px в world
                float  px2world  = _CanvasScale.x;            // X == Y
                float2 halfSizeW = (_HoleSizePx * 0.5) * px2world;
                float  softW     = _HoleSoftnessPx * px2world;

                // 2) Ќаходим Ђрассто€ниеї до границы пр€моугольника
                float2 delta = abs(i.worldPos.xy - _HoleCenter.xy) - halfSizeW;
                // max(delta.x, delta.y) < 0  -  внутри пр€моугольника
                float dist = max(delta.x, delta.y);

                // 3) ѕлавный переход:   0  Ц внутри,  1 Ц вне м€гкой зоны
                float cut = smoothstep(0.0, softW, dist);

                // 4) »тоговый цвет
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= cut;
                return col;
            }
            ENDCG
        }
    }
}
*/