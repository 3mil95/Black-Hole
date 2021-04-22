Shader "Unlit/Nebula"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex1 ("Texture2", 2D) = "white" {}
        _NoiseTex2 ("Texture2", 2D) = "white" {}
        _color1 ("color", color) = (1,1,1,1)
        _color2 ("color", color) = (1,1,1,1)
        _color3 ("color", color) = (1,1,1,1)
        _colorStart("Color Start", range(0,1)) = 0
        _colorEnd("Color End", range(0,1)) = 1
        _colorMax("Colot Max", range(0,1)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex1;
            sampler2D _NoiseTex2;
            fixed4 _color1;
            fixed4 _color2;
            fixed4 _color3;
            
            float _colorStart;
            float _colorEnd;
            float _colorMax;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float remap(float value, float startMin, float startMax, float endMin, float endMax) {
                float t = (value - startMin) / (startMax - startMin);
                return lerp(endMin, endMax, t);
            }

            float getNoiseValue(float2 uv) {
                float2 t =  (0,_Time/20);
                float value = tex2D(_NoiseTex2, ((uv + t)) % 1); // 1
                value -= tex2D(_NoiseTex2, ((uv + t)/ 8) % 1); 
                value += tex2D(_NoiseTex1, ((uv + t) * 1.2) % 1) / 2;
                value += tex2D(_NoiseTex1, ((uv + t) * 2) % 1) / 8; 
                value -= tex2D(_NoiseTex1, ((uv + t) * 4) % 1) / 10; 
                value += tex2D(_NoiseTex1, ((uv + t) * 8) % 1) / 12;   

                value = max(value, 0);
                return remap(value / 1.5, _colorStart, _colorEnd, 0, _colorMax);
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                float2 t =  (0,_Time/20);
                // sample the texture
                float value = getNoiseValue(i.uv);


                fixed4 value1 = tex2D(_NoiseTex2, ((i.uv + t * 2) + 30) % 1) * _color1; 
                fixed4 value2 = (1-tex2D(_NoiseTex2, ((i.uv + t * 3) ) % 1)) * _color2; 
                fixed4 value3 = tex2D(_NoiseTex2, ((i.uv + t * 1.5) / 2) % 1) /2 * _color3; 
                fixed4 col = value * (value1 + value2 + value3);

                col += tex2D(_MainTex, (i.uv + t) * (4, 3)  % 1) * (tex2D(_NoiseTex2, ((i.uv + t) + 10) % 1)).x;


                return col;
            }
            ENDCG
        }
    }
}
