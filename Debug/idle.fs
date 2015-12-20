#version 110

uniform sampler2D       u_Texture;
uniform sampler2D       u_TransparentMap;
uniform sampler2D       u_BrushMap;
uniform vec4            u_Color;
uniform vec3            u_UseTexture;

varying  vec3 v_VertexToLight;
varying  vec3 v_Normal;

void main(void) 
{
    vec3 L = v_VertexToLight;
    float intensity = abs(dot(L, v_Normal));

    vec4 color = u_Color * gl_Color; 
    if (u_UseTexture.x > 0.5)
        color *= texture2D( u_Texture, gl_TexCoord[0].st );
    if(u_UseTexture.y > 0.5)
        color.w *= texture2D( u_TransparentMap, gl_TexCoord[0].st ).x;
    color = vec4(color.xyz * intensity, color.w);
	if(u_UseTexture.z > 0.5)
	{
		vec4 blendColor = texture2D( u_BrushMap, gl_TexCoord[0].st );
		float k = 1.0 - blendColor.w;
		color = vec4((color.xyz * k) + (blendColor.xyz * blendColor.w), color.w);
	}
	gl_FragColor = color;
}
