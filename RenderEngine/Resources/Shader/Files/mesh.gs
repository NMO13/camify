#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in VS_OUT {
    vec3 normal;
} gs_in[];

out FS_OUT {
	vec3 Normal;
}frag_out;

void main() {    
	gl_Position = gl_in[0].gl_Position; 
	frag_out.Normal = gs_in[0].normal;
	EmitVertex();

	gl_Position = gl_in[1].gl_Position; 
	frag_out.Normal = gs_in[1].normal;
	EmitVertex();

	gl_Position = gl_in[2].gl_Position; 
	frag_out.Normal = gs_in[2].normal;
	EmitVertex();

    EndPrimitive();
}  