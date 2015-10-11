#version 110

uniform float u_BlendDepth;
uniform float u_BlendStartDepth;
varying  float v_Blend;

void main(void) 
{
	gl_Position = vec4( gl_Vertex.x * 2.0 - 1.0, 1.0 - gl_Vertex.y * 2.0, 0.0, 1.0);

	v_Blend = gl_MultiTexCoord0.z * (gl_Normal.z - u_BlendStartDepth) / u_BlendDepth;

	gl_TexCoord[0] = gl_MultiTexCoord0;
}