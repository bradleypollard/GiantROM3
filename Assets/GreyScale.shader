Shader "Unlit/GreyScale" {
  Properties{
    _MainTex("Base (RGB)", 2D) = "white" {}
  _AlphaReject("Alpha reject", Range(0.0, 1.0)) = 0.5
  }
    SubShader{
    Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
    LOD 200

    CGPROGRAM
#pragma surface surf Lambert

      sampler2D _MainTex;
    half _AlphaReject;

    struct Input {
      float2 uv_MainTex;
    };

    void surf(Input IN, inout SurfaceOutput o) {
      half4 c = tex2D(_MainTex, IN.uv_MainTex);
      clip(c.a - _AlphaReject);
      o.Albedo = dot(c.rgb, half3(0.3333, 0.3333, 0.3333));
      o.Alpha = c.a;
    }
    ENDCG
  }
    FallBack "Transparent/Cutout/Diffuse"
}