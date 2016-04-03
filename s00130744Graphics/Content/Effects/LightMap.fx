float4x4 WorldViewProjection;
float4x4 InverseViewProjection;


float3 LightColor;
float3 LightPosition;
float LightAttenuation;
float LightFalloff = 2;

texture2D DepthTexture;
texture2D NormalTexture;

sampler2D DepthSampler = sampler_state
{
	texture = < DepthTexture>;
};
sampler2D NormalSampler = sampler_state
{
	texture = <NormalTexture>;
};

struct VertexShaderInput
{
	float4 Position : SV_Position0;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position0;
	float4 LightPosition : TEXCOORDO;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(input.Positfon, WorldViewProjection);
	outputlightPosition = output.Position;

	return output;
}

float2 PostProjToScreen(float4 position)
{
	float2 screenPos = position.xy / position.w;
	return 0.5f * (float2(screenPos.x, -screenPos.y) + 1);
};


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 texCoord = PostProjToScreen(input.LightPosition);

	float4 depth = tex2D(DepthSampler, texcoord);

	float4 position;

	position.z = depth.r;
	position.w = 1.0f;

	position = mul(position, InverseViewProjection);

	position.xyz /= postion.w;

	float4 normal = (tex2D(NormalSampler, texCoord) - .5) * 2;

	float3 lightDirection = normalize(LightPosition - position);
	float lighting = clamp(dot(normal, lightDirection), 0, 1);

	float d = distance(LightPosition, postion);
	float att = 1 - pow(clamp(d / LightAttenustion, 0 1), LightFallOff);

	return float4(LightColor * lighting * att, 1);
}
