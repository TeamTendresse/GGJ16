Shader "Dot/Dot"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		//_Color("Color", Color) = (0,0,0,0)
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Transparent+1"
		}

		Pass
		{
			//ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile DUMMY PIXELSNAP_ON

			sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float4 _Scale;

			struct Vertex
			{
				float4 vertex : POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct Fragment
			{
				float4 vertex : POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 scale : TEXCOORD2;
			};

			Fragment vert(Vertex v)
			{
				Fragment o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = v.uv_MainTex;
				o.uv2 = v.uv2;
				o.scale.x = _Scale.x;
				o.scale.y = _Scale.y;

				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap(o.vertex);
				#endif

				return o;
			}

			float getAlpha(float min, float max)
			{

			}

			float4 frag(Fragment IN) : COLOR
			{
				float4 o = _Color;
				o.a = 0;

				float2 sizeScaled = float2(_MainTex_TexelSize.x / IN.scale.x, _MainTex_TexelSize.y / IN.scale.y);
				sizeScaled.x *= _MainTex_ST.x;
				sizeScaled.y *= _MainTex_ST.y;

				/*if (sizeScaled.x > 1 || sizeScaled.y > 1 || sizeScaled.x < 0 || sizeScaled.y < 0)
				{
					
					return o;
				}*/

								
				
				float len = 2 * length(IN.uv_MainTex - float2(0.5, 0.5));
				
				float min = 1 - 2 * length(sizeScaled);

				if (len < min)
					o.a = 1;
				else if (len < 1)
					o.a =1-	(len - min) / (1 - min);

				//Fractal


				return o;
			}

			ENDCG
		}
	}
}