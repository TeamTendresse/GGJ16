Shader "Dot/Dot"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
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
			uniform float2 _DirTrace;
			uniform int _Fadeout;
			uniform int _Mode;
			uniform int _Invert;

			uniform float p1;
			uniform float p2;

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

			float getAlpha(float2 center, float2 pos, float rayon, float pixelSize, int prof)
			{
				float alpha = 0;
				float rayonMax = rayon;
				float2 dirTmp = normalize(_DirTrace);
				float2 dir = float2(dirTmp.y,-dirTmp.x);
				float marge = 2 * pixelSize;

				for (int i = 0; i < 10; i++)
				{
					if (_Mode == 0)
						rayon *= 1-p1;

					float len = length(pos - center);
					float zonePleine = rayon - marge;
					
					if(_Mode == 0)
					{
						if(len < rayon)
						{
							if (len < zonePleine)
								alpha = 1;
							else
								alpha = 1 - (len - zonePleine) / (rayon - zonePleine);
						}
					}

					if (_Mode == 2 || _Mode == 1)
					{
						
						if (_Mode == 1)
						{
							if (_Invert == 0)
							{
								if (p1 > 0)
								{
									if (len < rayonMax)
										len = (rayonMax - marge) - len;
								}
							}
						}

						if (_Mode == 2)
						{
							if (_Invert == 0)
							{
								if (p1 > 0)
								{
									if (len < rayonMax)
										len = (rayonMax - marge) - abs(dot(pos - center, dir));
								}
							}
							else
							{ 
								if (p1 > 0)
								{
									if (len < rayonMax)
										len = abs(dot(pos - center, dir));
								}
							}
								
						}
						
						
						if (i % 2 == 0)
						{
							if (len < rayon)
							{
								if (len < zonePleine)
									alpha = 1;
								else
									alpha = 1 - (len - zonePleine) / (rayon - zonePleine);
							}
						}
						else
						{
							if (len < rayon)
							{
								if (len < zonePleine)
									alpha = 0;
								else
									alpha = (len - zonePleine) / (rayon - zonePleine);
							}
						}
					}

					if (_Mode == 0)
						break;
						
					//On passe au suivant
					rayon *= p1;
				}
				
				//if (_Fadeout > 0)
					alpha *= 1 - p1;

				return alpha ;	
			}

			float4 frag(Fragment IN) : COLOR
			{
				float4 o = _Color;
				o.a = 0;

				float2 sizeScaled = float2(_MainTex_TexelSize.x / IN.scale.x, _MainTex_TexelSize.y / IN.scale.y);
				sizeScaled.x *= _MainTex_ST.x;
				sizeScaled.y *= _MainTex_ST.y;						
		
				o.a = getAlpha(float2(0.5,0.5), IN.uv_MainTex, 0.5	, length(sizeScaled),0);

				return o;
			}

			ENDCG
		}
	}
}