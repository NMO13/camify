#version 330 core
struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};

struct DirLight {
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight {
	vec3 position;

	float constant;
	float linear;
	float quadratic;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

//#define MAX_POINT_LIGHTS 100

in GS_OUT {
    vec3 Normal;
	vec3 FragPos;
	vec3 Dist;
	vec3 WorldPos;
	vec3 WorldNormal;
} gs_in;

out vec4 color;

uniform int numPointLights;
uniform DirLight dirLight;
uniform PointLight pointLights[2];
uniform Material material;
uniform sampler2D bayerTex;

// Function prototypes
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
	// Properties
	vec3 norm = gs_in.Normal;
	vec3 viewDir = normalize(-gs_in.FragPos);
    
	// Phase 1: Directional lighting
	vec3 result = CalcDirLight(dirLight, norm, viewDir);
	// Phase 2: Point lights
	for (int i = 0; i < 2; i++) {
		PointLight pLight = pointLights[i];
		result += CalcPointLight(pLight, norm, gs_in.FragPos, viewDir);
	}

	//Dithering
	result += texture2D(bayerTex, gl_FragCoord.xy / 8.0).r / 64.0 - (1.0 / 128.0); 

	//Edges
	float nearD = min(min(gs_in.Dist.x,gs_in.Dist.y),gs_in.Dist.z); 
	float edgeIntensity = exp2(-1.0*nearD*nearD);
	color = (edgeIntensity * vec4(0.1,0.1,0.1,1.0)) + ((1.0-edgeIntensity) * vec4(result, 1.0));
}

// Calculates the color when using a directional light.
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);
	// Diffuse shading
	float diff = max(dot(normal, lightDir), 0.0);
	// Specular shading
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	// Combine results
	vec3 ambient = light.ambient * material.ambient;
	vec3 diffuse = light.diffuse * diff * material.diffuse;
	vec3 specular = light.specular * spec * material.specular;
	return (ambient + diffuse + specular);
}

// Calculates the color when using a point light.
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);
	// Diffuse shading
	float diff = max(dot(normal, lightDir), 0.0);
	// Specular shading
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	// Combine results
	vec3 ambient = light.ambient * material.ambient;
	vec3 diffuse = light.diffuse * diff* material.diffuse;
	vec3 specular = light.specular * spec * material.specular;
	return (ambient + diffuse + specular);
}

