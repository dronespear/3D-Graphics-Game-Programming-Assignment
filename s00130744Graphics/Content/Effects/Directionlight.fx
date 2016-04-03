float4x4 World;
float4x4 View;
float4x4 Projection;

bool TextureEnabled = false;
texture Texture;

float3 DiffuseColor = float3(1, 1, 1);
float3 AmbientColor = float3(.15, .15, .15);
float3 LightColor = float3(1, 1, 1);
float3 LightDirection = float3(0, 1, 0);

//float3 CameraPosition = float3(1, 1, 1);
//float SpecularPower = 32;
//float3 SpecularColor = float3(1, 1, 1);

sampler TextureSampler = sampler_state
{
	texture = <Texture>;
};

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	//float3 ViewDirection : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);
	//output.ViewDirection = worldPosition - CameraPosition;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = DiffuseColor;

	if (TextureEnabled)
		color *= tex2D(TextureSampler, input.UV);

	float3 lighting = AmbientColor;
	float3 lightDir = normalize(LightDirection);
	float3 normal = normalize(input.Normal);

	//lambertian reflectance dot product(Light Direction, Surface Normal)
	lighting += saturate(dot(lightDir, normal)) * LightColor;


	//
	//float3 refi = reflect(lightDir, normal);
	//float3 view = normalize(input.ViewDirection);

	//Specular lighting
	//lighting += pow(saturate(dot(refl, view))), SpecularPower)*SpecularColor;

	float3 output = saturate(lighting * color);

	return float4(output, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
