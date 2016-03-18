#version 330

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;
uniform mat4 normalMatrix;

layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inColor;
layout (location = 2) in vec3 inNormal;

out vec3 TheColor;

smooth out vec3 vNormal;

void main()
{
	gl_Position = projectionMatrix*modelViewMatrix*vec4(inPosition, 1.0);
	TheColor= inColor;
	vec4 vRes = normalMatrix*vec4(inNormal, 0.0);
	vNormal = vRes.xyz;
}