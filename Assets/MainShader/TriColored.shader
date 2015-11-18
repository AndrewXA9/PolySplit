Shader "Polygon Colored"{
	Properties{
		_Color1("Color 1", Color) = (1,0,0,1)
		_Color2("Color 2", Color) = (0,0,1,1)
		_Seed("Seed", float) = 0
	}
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		//prepare the variables
		float4 _Color1;
		float4 _Color2;
		float _Seed;
		
		struct Input{
			//vertex color
			float4 color: Color; 
		};
		
		void surf (Input IN, inout SurfaceOutput o){
			
			//use CG lerp function, with the lerp value being a 
			//linear congruential generator based on user seed and vertex color
			o.Albedo = lerp(_Color1,_Color2,fmod(((IN.color.x)*263) + _Seed,1));
			
		}
		
		ENDCG
	}
	
	FallBack "Diffuse"
}