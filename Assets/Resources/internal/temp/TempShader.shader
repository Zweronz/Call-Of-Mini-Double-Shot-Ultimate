Shader "ShaderEditor/EditorShaderCache" {
Properties {
 _Color ("_Color", Color) = (1,0,0,1)
 _RimColor ("_RimColor", Color) = (0.0597015,0.250391,1,1)
 _RimPower ("_RimPower", Range(0.1,3)) = 1.8
 _SpecPower ("_SpecPower", Range(0.1,1)) = 0.9
 _MySpecColor ("_MySpecColor", Color) = (1,1,1,1)
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}