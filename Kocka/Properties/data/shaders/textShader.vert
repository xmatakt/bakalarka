#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec2 vertexUV;
uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

void main()
{
	gl_Position =  projectionMatrix*modelViewMatrix*vec4(vertexPosition,1);
	// UV of the vertex. No special space for this one.
	UV = vertexUV;
}

