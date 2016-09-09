#version 330 core
layout (triangles) in;
layout (line_strip, max_vertices = 6) out;

in VS_OUT {
    vec3 Normal;
	float Mag;
} gs_in[];

// Function prototypes
void GenerateLine(int index);

void main() {    
	GenerateLine(0); //First normal
	GenerateLine(1); //Second normal
	GenerateLine(2); //Third normal
}  

void GenerateLine(int index){
	gl_Position = gl_in[index].gl_Position;
	EmitVertex();
	gl_Position = gl_in[index].gl_Position + vec4(gs_in[index].Normal, 0.0f) * gs_in[index].Mag;
	EmitVertex();

	EndPrimitive();
}