#version 330

in vec3 TheColor;
smooth in vec3 vNormal;
out vec4 outputColor;

//uniform sampler2D gSampler;
//uniform vec4 vColor;

struct SimpleDirectionalLight
{
	vec3 vColor;
	vec3 vDirection;
	float fAmbientIntensity;
};

uniform SimpleDirectionalLight sunLight;

void main()
{
	vec4 v4Color = vec4(TheColor,1.0);
	float fDiffuseIntensity = max(0.0, dot(normalize(vNormal), -sunLight.vDirection));
	gl_FragColor = v4Color*vec4(sunLight.vColor*(sunLight.fAmbientIntensity+fDiffuseIntensity), 1.0);
	//outputColor = v4Color*vec4(sunLight.vColor*(sunLight.fAmbientIntensity+fDiffuseIntensity), 1.0);
	//outputColor = v4Color*vColor*vec4(sunLight.vColor*(sunLight.fAmbientIntensity+fDiffuseIntensity), 1.0);
}