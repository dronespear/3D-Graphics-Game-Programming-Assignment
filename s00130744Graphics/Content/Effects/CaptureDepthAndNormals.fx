float4x4 World;
float4x4 View;
float4x4 Projection;

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
	float3 Normal: NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
	float3 Normal : TEXCOORD0;
	float2 Depth : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	output.Depth.xy = output.Position.zw;

    return output;
}

struct PixelShaderOutput
{
	float4 Normal : COLOR0;
	float4 Depth: COLOR1;
};

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;

	//Normals
	output.Normal.xyz = normalize(input.Normal / 2) + .5;
	output.Normal.a = 1;

	//Depth
	output.Depth = input.Depth.x / input.Depth.y;
	output.Depth.a = 1;

	return output;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
