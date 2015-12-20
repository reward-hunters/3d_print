#version 110

uniform vec3		u_BrushColor;
uniform vec3		u_SphereCenter;
uniform float		u_SphereRadius;

varying	vec3		v_Position;

void main(void) 
{
	vec3 v = v_Position - u_SphereCenter;
	float l = length(v);
	if(l >  u_SphereRadius)
		gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);
	else
	{
		float k = 1.0 - (l / u_SphereRadius);
		gl_FragColor = vec4(u_BrushColor, 0.75 + (k * 0.25));
	}
}
