#version 110

uniform vec4		u_BrushColor;
uniform vec3		u_SphereCenter;
uniform float		u_SphereRadius;
uniform sampler2D	u_Texture;

varying	vec3		v_Position;
varying	vec2		v_TexCoords;

void main(void) 
{
	vec3 v = v_Position - u_SphereCenter;
	vec4 baseColor = texture2D( u_Texture, v_TexCoords );
	float l = length(v);
	if(l >  u_SphereRadius)
		gl_FragColor = baseColor;
	else
	{
		float k = (1.0 - (l / u_SphereRadius)) * u_BrushColor.w;		
		float summ = k + baseColor.w;
		float k1 = baseColor.w / summ;
		gl_FragColor = vec4((u_BrushColor.xyz * k / summ) + (baseColor.xyz * k1), k + baseColor.w); //, 0.0, 0.6
	}
}
