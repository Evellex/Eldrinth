#include "EMParticleFunctions.cginc"

struct SurfaceOutputSmoke
{
    fixed3 Albedo;  
    fixed3 Normal;  
    fixed3 Emission;
    half Specular;
    fixed Gloss; 
    fixed Alpha; 
};

inline fixed4 LightingSmoke (SurfaceOutputSmoke s, fixed3 lightDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	
	//Light scattering is achieved with half-lambert technique
	// https://developer.valvesoftware.com/wiki/Half_Lambert
	diff = pow(diff * (1 - _Thickness) + _Thickness, 2);
	
	fixed4 c;
	
	c.rgb = s.Albedo * _LightColor0.rgb * diff * atten;
	
	c.a = saturate(s.Alpha);
	return c;
}