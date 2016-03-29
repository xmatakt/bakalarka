// for raycasting
#version 400

layout(location = 0) in vec3 VerPos;
layout(location = 1) in vec3 VerClr;

out vec3 Color;

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

void main()
{
 	Color = VerClr;
	//posunutie do stredu obrazovky
	vec3 tmp = vec3(-0.5f,-0.5f,-0.5f);
	tmp=VerPos+tmp; 
	gl_Position = projectionMatrix * modelViewMatrix * vec4(tmp, 1.0);
}
