#version 330 core
layout (location = 0) in vec3 position; //Vertices and normals are in world coordinate space
layout (location = 1) in float isContourEdge;
layout (location = 2) in vec3 normal;

out VS_OUT {
    vec3 Normal;
	vec3 FragPos;
	vec3 VertWorldNormal;
	vec3 VertWorldPos;
	float IncludeEdge;
} vs_out;

uniform mat4 view;
uniform mat4 proj;

void main()
{
    gl_Position = proj * view *  vec4(position, 1.0f);
    vs_out.FragPos = vec3(view * vec4(position, 1.0f));
	vs_out.Normal = vec3(view * vec4(normal, 0));
	vs_out.IncludeEdge = isContourEdge > 0 ? 1.0f : 0.0f;

	vs_out.VertWorldNormal = normal;
	vs_out.VertWorldPos = position;
} 