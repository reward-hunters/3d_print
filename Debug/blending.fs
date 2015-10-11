#version 110

uniform sampler2D   u_Texture;
varying  float v_Blend;

void main(void) 
{
   vec4 color = texture2D( u_Texture, gl_TexCoord[0].st );
   gl_FragColor = vec4(color.xyz, v_Blend);
}
