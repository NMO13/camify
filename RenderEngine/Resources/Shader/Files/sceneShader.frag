#version 330 core
in vec4 vertexColor; // The input variable from the vertex shader (same name and same type)
flat in int yRes;  
out vec4 color;

void main()
{
   vec3 h_color_top = vec3(0.68, 0.80, 0.89);
   vec3 h_color_bottom = vec3(0.596, 0.745, 0.909);
   color = vec4(h_color_top * (gl_FragCoord.y / yRes), 1) + vec4(h_color_bottom * (1.4 - (gl_FragCoord.y / yRes)), 1);
} 