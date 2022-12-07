Shader "iPhone/LightMap_ColorEffect" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader { 
 Pass {
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord", TexCoord1
  }
  SetTexture [_texLightmap] { combine texture }
  SetTexture [_texBase] { combine previous + texture }
 }
}
}