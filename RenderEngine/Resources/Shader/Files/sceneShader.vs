#version 330 core
layout (location = 0) in vec3 position; // The position variable has attribute position 0

uniform int resolutionY;  
flat out int yRes;  

void main()
{
	yRes = resolutionY;
    gl_Position = vec4(position, 1.0f);
}