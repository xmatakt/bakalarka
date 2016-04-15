#version 330 core

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D gSampler;

void main()
{
	
	color = texture( gSampler, UV );//bud iba toto

	//alebo toto ak chcem inverznu
	//vec3 inverseCol = vec3(1.0f,1.0f,1.0f) - texture( gSampler, UV ).xyz;
	//color = vec4(inverseCol,1.0f);
}