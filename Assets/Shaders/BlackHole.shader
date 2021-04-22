Shader "Unlit/BlackHole"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 pos : SV_POSITION;  
            };

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;
            sampler2D _MainTex;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            sampler2D _BackgroundTexture;

            float remap(float value, float startMin, float startMax, float endMin, float endMax) {
                float t = (value - startMin) / (startMax - startMin);
                return lerp(endMin, endMax, t);
            }

            half4 frag(v2f i) : SV_Target
            {
                float dist = pow(pow((i.uv.x - 0.5), 2) + pow((i.uv.y - 0.5), 2), 0.5);
                float mask = max(0, 1 - remap(dist, 0.3, 0.5, 0, 1));
                float ring = remap(dist, 0.25, 1, 0, 1);

                float deformation = 1/pow(dist*pow(0.3,0.5),2)*0.01*2;
                half4 bgcolor = tex2Dproj(_BackgroundTexture, (i.grabPos + deformation * mask));
                half4 l = (1,1,1,1) * ring * mask * 0.7;
                //return mask;
                return bgcolor + l; // (sin(_Time)) * dist;
            }
            ENDCG
        }

    }
}