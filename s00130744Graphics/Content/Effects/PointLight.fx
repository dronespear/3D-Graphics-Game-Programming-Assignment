float4x4 World;
float4x4 View;
float4x4 Projection;

bool TextureEnabled = false;
texture Texture;

//same as directional
float3 DiffuseColor = float3(1, 1, 1);
float3 AmbientColor = float3(.15, .15, .15);
float3 LightColor = float3(1, 1, 1);

//added to point
float3 LightPosition = float3(0, 0, 0);
float LightAttenuation = 20;
float LightFallOff = 2;

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
	float4 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = DiffuseColor;

	if (TextureEnabled)
		color *= tex2D(TextureSampler, input.UV);

	float3 lighting = AmbientColor;
	float3 lightDir = normalize(LightPosition - input.WorldPosition);
	float3 normal = normalize(input.Normal);
	float3 diffuse = saturate(dot(normal, lightDir));
	float dist = distance(LightPosition, input.WorldPosition);
	float atten = 1 - pow(clamp(dist / LightAttenuation, 0, 1), LightFallOff);

	lighting += diffuse * atten * LightColor;

	return float4(lighting * color, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
