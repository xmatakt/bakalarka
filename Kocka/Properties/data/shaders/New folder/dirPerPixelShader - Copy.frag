#version 330

varying vec3 diffuse,ambient,specular;
varying vec3 normal;

//toto by som mal dostat z vertex shaderu
in vec3 TheColor;
in vec3 lightDirection;
in vec3 EyeVector;
in float shininess;

//posielam dalej
//out vec4 outputColor;

void main()
{
	float NdotL,RdotEye;
	vec3 n,R,color,ld;
	
	color = ambient;//nezavisle na pozicii vsetkeho
	ld=lightDirection;//tohoto sa zbavit

	//fragment shader nemoze zapisovat varying premenne, preto n=...
	n = normalize(normal);

	NdotL = max(dot(n,-ld),0.0);

	//vypocet odrazeneho luca R
	R = 2.0 * NdotL * n - ld;
	R = normalize(R);
	RdotEye = dot(R,normalize(EyeVector));

	if(NdotL > 0.0)
	{
		color += diffuse * NdotL;
		//druhy if tu?,alebo bez ifu?
	}

	if(RdotEye > 0.0)
		color += specular * pow(RdotEye, 3);

	//color = color * TheColor;
	
	gl_FragColor = vec4(TheColor, 1.0);
	//outputColor=vec4(TheColor, 1.0);
}