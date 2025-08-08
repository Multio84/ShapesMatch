Shader "UI/TutorialCircleHole"
{
    Properties
    {
        _MainTex        ("Texture", 2D)                    = "white" {}
        _Color          ("Tint"  , Color)                  = (0,0,0,0.7)

        // hole world position
        _HoleCenterX    ("Hole Center X (world)", Float)   = 0
        _HoleCenterY    ("Hole Center Y (world)", Float)   = 0

        // hole radius and softness in pixels of canvas
        _HoleRadiusPx   ("Hole Radius  (px)",    Float)    = 200
        _HoleSoftnessPx ("Hole Softness(px)",    Float)    = 20

        // scale of canvas
        _CanvasScaleX   ("Canvas scale X",       Float)    = 1
        _CanvasScaleY   ("Canvas scale Y",       Float)    = 1
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

            float _HoleCenterX, _HoleCenterY;
            float _HoleRadiusPx, _HoleSoftnessPx;
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
                // 1) px -> world
                float px2world = _CanvasScaleX; // X = Y
                float radiusW  = _HoleRadiusPx   * px2world;
                float softW    = _HoleSoftnessPx * px2world;

                float2 holeCenter = float2(_HoleCenterX, _HoleCenterY);

                // 2) distance
                float dist = length(i.worldPos.xy - holeCenter);
                float cut  = smoothstep(radiusW - softW, radiusW + softW, dist);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= cut;
                return col;
            }
            ENDCG
        }
    }
}