fixed _Thickness;

//Shuriken particles are camera-space. This matrix allows transform them to world space
float4x4 _Camera2World;

//Distance Fade variables
float _FadeStart;
float _FadeEnd;