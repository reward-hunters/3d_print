#version 110

uniform sampler2D       u_Texture;
uniform sampler2D       u_TransparentMap;
uniform vec4            u_Color;
uniform float           u_UseTexture;
uniform float           u_UseTransparent;

varying  vec3 v_VertexToLight;
varying  vec3 v_Normal;

void main(void) 
{
    vec3 L = v_VertexToLight;
    float intensity = abs(dot(L, v_Normal));

    vec4 color = u_Color * gl_Color; 
    if (u_UseTexture > 0.5)
        color *= texture2D( u_Texture, gl_TexCoord[0].st );
    if(u_UseTransparent > 0.5)
        color.w *= texture2D( u_TransparentMap, gl_TexCoord[0].st ).x;
   gl_FragColor = vec4(color.xyz * intensity, color.w);
}
