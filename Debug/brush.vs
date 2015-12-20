#version 110

uniform	mat4	u_World;

varying	vec3	v_Position;

void main(void) 
{
	gl_Position = vec4( gl_Vertex.x * 2.0 - 1.0, 1.0 - gl_Vertex.y * 2.0, 0.0, 1.0);
	vec4 position = u_World * vec4( gl_Normal.xyz, 1.0 );
	v_Position = position.xyz;

	gl_TexCoord[0] = gl_MultiTexCoord0;
}