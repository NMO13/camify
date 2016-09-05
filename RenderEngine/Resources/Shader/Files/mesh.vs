#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

out vec3 Normal;
out vec3 FragPos;

//uniform mat4 model; Vertices are alrady in world coordinates
uniform mat4 view;
uniform mat4 proj;

void main()
{
    gl_Position = proj * view *  vec4(position, 1.0f);
    FragPos = vec3(view * vec4(position, 1.0f));
    Normal =  mat3(view) * normal;
} 