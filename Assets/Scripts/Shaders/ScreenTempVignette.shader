
Shader "Custom/ScreenTempVignette" {
  Properties {
    _MainTex("Texture", 2D) = "white" {}
    _VRadius("Vignette Radius", Range(0.0, 1.0)) = 0.5
    _VSoft("Vignette Softness", Range(0.0, 1.0)) = 0.5
    _VColor("Vignette Color", Color) = (1, 1, 1, 1)
  }

  SubShader {
    Pass {
      CGPROGRAM
      #pragma vertex vert_img
      #pragma fragment frag
      #include "UnityCG.cginc"

      sampler2D _MainTex;
      float _VRadius;
      float _VSoft;
      float4  _VColor;

      float4 frag(v2f_img input) : COLOR {
        float4 base = tex2D(_MainTex, input.uv);
        float distFromCenter = distance(input.uv.xy, float2(0.5, 0.5));
        float vingette = smoothstep(_VRadius, _VRadius -_VSoft, distFromCenter);
        base = base * vingette + _VColor * (1.0 - vingette);
        return base;
      }
      ENDCG
}}}