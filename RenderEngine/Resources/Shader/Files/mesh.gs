#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in VS_OUT {
    vec3 Normal;
	vec3 FragPos;
	vec3 VertWorldNormal;
	vec3 VertWorldPos;
} gs_in[];

out GS_OUT {
	vec3 Normal;
	vec3 FragPos;
	vec3 Dist;
	vec3 WorldPos;
	vec3 WorldNormal;
} gs_out;

uniform vec2 win_scale;

void main() {    
	vec2 p0 = win_scale * gl_in[0].gl_Position.xy / gl_in[0].gl_Position.w;
	vec2 p1 = win_scale * gl_in[1].gl_Position.xy / gl_in[1].gl_Position.w;
	vec2 p2 = win_scale * gl_in[2].gl_Position.xy / gl_in[2].gl_Position.w;

	vec2 v0 = p2 - p1;
	vec2 v1 = p2 - p0;
	vec2 v2 = p1 - p0;
	float area = abs(v1.x * v2.y - v1.y * v2.x);

	gl_Position = gl_in[0].gl_Position; 
	gs_out.Dist = vec3(area/length(v0), 0, 0);
	gs_out.Normal = gs_in[0].Normal;
	gs_out.FragPos = gs_in[0].FragPos;
	gs_out.WorldPos = gs_in[0].VertWorldPos;
	gs_out.WorldNormal = gs_in[0].VertWorldPos;
	EmitVertex();

	gl_Position = gl_in[1].gl_Position; 
	gs_out.Dist = vec3(0, area/length(v1), 0);
	gs_out.Normal = gs_in[1].Normal;
	gs_out.FragPos = gs_in[1].FragPos;
	gs_out.WorldPos = gs_in[1].VertWorldPos;
	gs_out.WorldNormal = gs_in[1].VertWorldPos;
	EmitVertex();

	gl_Position = gl_in[2].gl_Position; 
	gs_out.Dist = vec3(0, 0, area/length(v2));
	gs_out.Normal = gs_in[2].Normal;
	gs_out.FragPos = gs_in[2].FragPos;
	gs_out.WorldPos = gs_in[2].VertWorldPos;
	gs_out.WorldNormal = gs_in[2].VertWorldPos;
	EmitVertex();

    EndPrimitive();
}  