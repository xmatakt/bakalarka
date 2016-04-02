#version 400

layout (location = 0) in vec3 VerPos;
// have to use this variable!!!, or it will be very hard to debug for AMD video card
layout (location = 1) in vec3 VerClr;  


out vec3 EntryPoint;
out vec4 ExitPointCoord;

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

void main()
{
	//posunutie do stredu obrazovky
	vec3 tmp = vec3(-0.5f,-0.5f,-0.5f);
	tmp=VerPos+tmp; 
	EntryPoint = VerClr;
	gl_Position = projectionMatrix*modelViewMatrix * vec4(tmp,1.0);
	ExitPointCoord = gl_Position;  
}
