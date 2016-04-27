#version 330

in vec3 EntryPoint;
in vec4 ExitPointCoord;

uniform sampler2D ExitPoints;
uniform sampler3D VolumeTex;
uniform sampler1D TransferFunc;  
uniform float     StepSize;
uniform float     AlphaReduce;
uniform vec2      ScreenSize;
layout (location = 0) out vec4 FragColor;

void main()
{
	//gl_FragCoord --> http://www.txutxi.com/?p=182
	vec3 exitPoint = texture(ExitPoints, gl_FragCoord.st/ScreenSize).xyz;

	if (EntryPoint == exitPoint)
		//background need no raycasting
		discard;
	
	vec3 dir = normalize(exitPoint - EntryPoint);
	vec4 pos = vec4(EntryPoint, 0.0f);
	vec4 dst = vec4(.0f,.0f,.0f,0.0f);
	vec4 src = vec4(0.0f,0.0f,0.0f,0.0f);
	float value = 0.0f;
	vec3 Step = dir * StepSize;
	float stepLength= length(Step);
	float LengthSum = 0.0f;
	float Length = length(exitPoint - EntryPoint);
	
	for(int i=0; i < 1600; i++)
	{
		//pos.w = 0.0f;
		value = texture(VolumeTex, pos.xyz).a;
		//src = vec4(value);
		src = texture(TransferFunc,value);
		//reduce the alpha to have a more transparent result
		src.a *= AlphaReduce;
		
		//Front to back blending
		src.rgb *= src.a;
		dst = (1.0f - dst.a) * src + dst;
		
		//accumulate length
		LengthSum += stepLength;

		//break from the loop when alpha gets high enough
		if(dst.a >= .95f)
			break;
		
		//advance the current position
		pos.xyz += Step;

		//break if the ray is outside of the bounding box
		if(LengthSum >= Length)
			break;
	}
	FragColor = dst;
}
