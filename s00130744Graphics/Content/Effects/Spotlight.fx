float4x4 World;
float4x4 View;
float4x4 Projection;

bool TextureEnabled = false;
texture Texture;

float3 AmbientLightColor = float3(.15, .15, .15);

float3 DiffuseColor = float3(.85, .85, .85);
float3 LightPosition = float3(0, 0, 0);
float3 LightColor = float3(1, 1, 1);
float3 LightDirection = float3(0, -1, 0);

float LightFalloff = 2;
float ConeAngle = 45;

//float3 CameraPosition = float3(1, 1, 1);
//float SpecularPower = 32;
//float3 SpecularColor = float3(1, 1, 1);

//sampler TextureSampler = sampler_state
//{
//	texture = <Texture>;
//};

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
	float3 WorldPosition : TEXCOORD2;
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
	//output.ViewDirection = worldPosition - CameraPosition;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = DiffuseColor;

	if (TextureEnabled)
		diffuseColor *= tex2D(TextureSampler, input.UV);

	float3 totalLight = float3(0, 0, 0);
	totalLight += AmbientLightColor;

	float3 lightDir = normalize(LightPosition - input.WorldPosition);
	float3 diffuse = saturate(dot(normalize(input.Normal), lightDir));

	float d = dot(-lightDir, normalize(LightDirection));
	float a = cos(ConeAngle);

	float att = 0;
	if (a < d)
		att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);

	totalLight += diffuse * att * LightColor;

	return float4(diffuseColor * totalLight, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
