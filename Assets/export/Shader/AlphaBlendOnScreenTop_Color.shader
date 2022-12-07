Shader "iPhone/AlphaBlendOnScreenTop_Color" {
Properties {
 R ("R", Range(0,1)) = 1
 G ("G", Range(0,1)) = 1
 B ("B", Range(0,1)) = 1
 _Alpha ("Alpha", Range(0,1)) = 0.5
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor ([R],[G],[B],[_Alpha]) combine texture * constant double, texture alpha * constant alpha }
 }
}
}