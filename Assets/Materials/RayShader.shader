Shader "Custom/RayShader"
{
    Properties
    {
      _MainTex ("Texture", 2D) = "white" {}
      _Color ("Color", Color) = (1, 1, 1, 1)
      _DisplacementTex("Displacement Texture", 2D) = "white" {}
      _Saturation("Saturation", float) = 0.0
      _Magnitude("Magnitude", Range(0, 1)) =  0.5
      _Speed("Speed", float) =  1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        //Cull Off
        ZWrite Off //Don't render the depth buffer
        Blend SrcAlpha OneMinusSrcAlpha //Color - Alpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 duv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;
            sampler2D _DisplacementTex;
            float4 _DisplacementTex_ST;
            float _Saturation;
            float _Magnitude;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.duv = TRANSFORM_TEX(v.uv, _DisplacementTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 disp = tex2D(_DisplacementTex, i.duv+ sin(_Time.y * _Speed * 0.05)).xy ;
                disp = ((disp * 2) - 1) * _Magnitude;
	            float4 col = tex2D(_MainTex, i.uv + disp) * _Color;
                col = saturate(col) * _Saturation;
                return col;
            }
            ENDCG
        }
    }
}
