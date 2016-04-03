float4x4 World;
float4x4 View;
float4x4 Projection;

texture2D Texture;
texture2D LightTexture;
bool TextureEnabled = false;

float3 AmbientColor = float3(0, 0, 0);
float3 DiffuseColor = float3(1, 1, 1);

float ViewportWidth;
float ViewportHeight;

sampler TextureSampler = sampler_state
{
	texture = <Texture>;
};

sampler LightSampler = sampler_state
{
	texture = <LightTexture>;
};

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
	float2 UV: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
	float2 UV: TEXCOORD0;
	float4 PositionCopy : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
	output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;
	output.PositionCopy = output.Position;

    return output;
}

float2 PostProjToScreen(float4 position)
{
	float2 screenPos = position.xy / position.w;
	return 0.5f * (float2(screenPos.x, -screenPos.y) + 1);
};

//PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//PixelShaderOutput output;

	float3 basicTexture = float4(1, 1, 1, 1);

	if (TextureEnabled)
		basicTexture *= tex2D(TextureSampler, input.UV);

	float2 texCoord = PostProjToScreen(input.PositionCopy);
	float3 light = tex2D(LightSampler, texCoord);

	light += AmbientColor;

	return float4(basicTexture * DiffuseColor * light, 1);

	//return output;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
