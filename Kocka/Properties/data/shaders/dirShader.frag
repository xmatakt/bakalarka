#version 330

smooth in vec3 TheColor;
out vec4 outputColor;

void main()
{
	outputColor=vec4(TheColor, 1.0);
}