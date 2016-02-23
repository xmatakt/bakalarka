#version 330

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;
uniform mat4 normalMatrix;
uniform vec3 eye;

varying vec3 diffuse,ambient,specular;
varying vec3 normal;

layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inColor;
layout (location = 2) in vec3 inNormal;

struct DirectionalLight
{
	vec3 ambient;	
	vec3 diffuse;
	vec3 specular;
	vec3 direction;
};

uniform DirectionalLight light;

struct Material
{
	float specCoef; 	//[0,1]
	float diffCoef;		//[0,1]
	float ambCoef;		//[0,1]
	int shininess;	//[0,128]
};

uniform Material material;

out vec3 TheColor;
out vec3 lightDirection;
out vec3 EyeVector;
out float shininess;

void main()
{
	vec4 normal4;

	//transformacia normaly do eye space a znormovanie vysledku
	normal4=normalMatrix * normalize(vec4(inNormal,1.0));
	normal=normalize(normal4.xyz);
	
	EyeVector = normalize(eye);//iba znormujem a posuniem dalej
	lightDirection = normalize(light.direction);//znormujem a posuniem dalej

	diffuse =  material.diffCoef * light.diffuse;// * NdotL pride az vo fragment shaderi
	ambient = light.ambient * material.ambCoef;
	specular = material.specCoef * light.specular;//zvysok pride vo fragment shaderi
	shininess = material.shininess;//teba tiez len poslem

	TheColor = inColor;//farbu iba posuniem fragment shaderu

	gl_Position = projectionMatrix*modelViewMatrix*vec4(inPosition, 1.0);	
}

// Vytvorene s pomocou:
// http://www.lighthouse3d.com/tutorials/glsl-12-tutorial/directional-light-per-pixel/

























