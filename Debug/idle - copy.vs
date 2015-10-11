#version 110

uniform mat4  u_WorldView;
uniform mat4  u_ViewProjection;
uniform mat4  u_World;
uniform vec3  u_LightDirection; 

varying  vec3 v_VertexToLight;
varying  vec3 v_Normal;

void main(void) 
{
	vec4 position = u_World * vec4( gl_Vertex.xyz, 1.0 );
	gl_Position = u_ViewProjection * position;

	vec3 viewLightDirection = ( u_WorldView * vec4(u_LightDirection, 0.0) ).xyz;
	v_VertexToLight = normalize( viewLightDirection );

	v_Normal = ( u_WorldView * vec4(gl_Normal.xyz, 0.0) ).xyz;
	v_Normal = normalize( v_Normal );

	gl_FrontColor = gl_Color;

	gl_TexCoord[0] = gl_MultiTexCoord0;
}
