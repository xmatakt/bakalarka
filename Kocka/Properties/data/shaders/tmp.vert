#version 330

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;
uniform mat4 modelMatrix;

layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inColor;

smooth out vec3 theColor;

void main()
{
	gl_Position = projectionMatrix*modelViewMatrix*modelMatrix*vec4(inPosition, 1.0);
	theColor = inColor;
}