float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture;
float3 Color;

sampler TextureSample = sampler_state
{
	texture = <Texture>;
};

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//Color * TextureColor at vertex UV  coordinate
	return float4(Color*tex2D(TextureSample, input.UV), 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
