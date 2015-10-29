Shader "Skybox/CubemapsBlended" {
	Properties{
		_Tint("Tint Color", Color) = (.5, .5, .5, .5)
		_Blend("Blend", Range(0.0,1.0)) = 0.5
		_Tex1("Tex1", Cube) = "white" {}
		_Tex2("Tex2", Cube) = "white" {}
	}

	SubShader{
		Tags{ "Queue" = "Background" }
		Cull Off
		Fog{ Mode Off }
		Lighting Off
		Color[_Tint]
		Pass {
			SetTexture[_Tex1]{ combine texture }
			SetTexture[_Tex2]{ constantColor(0,0,0,[_Blend]) combine texture lerp(constant) previous }
		}
	}

	Fallback "Skybox/6 Sided", 1
}