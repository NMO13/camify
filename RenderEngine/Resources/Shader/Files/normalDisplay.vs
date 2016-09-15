#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in float isContourEdge;
layout (location = 2) in vec3 normal;

out VS_OUT {
    vec3 Normal;
	float Mag;
} vs_out;

out vec3 Normal;

uniform mat4 view;
uniform mat4 proj;
uniform float magnitude;

void main()
{
    gl_Position = proj * view *  vec4(position, 1.0f);
	vs_out.Normal = vec3(proj * view * vec4(normal, 0));
	vs_out.Mag = magnitude;
} 