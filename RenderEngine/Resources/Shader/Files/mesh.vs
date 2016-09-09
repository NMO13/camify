#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

out VS_OUT {
    vec3 Normal;
} vs_out;

out vec3 Normal;
out vec3 FragPos;

//uniform mat4 model; Vertices and normals are alrady in world coordinate space
uniform mat4 view;
uniform mat4 proj;

void main()
{
    gl_Position = proj * view *  vec4(position, 1.0f);
    FragPos = vec3(view * vec4(position, 1.0f));
	vs_out.Normal = vec3(view * vec4(normal, 0));
} 