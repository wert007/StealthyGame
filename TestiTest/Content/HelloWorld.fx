float4x4 matWorldViewProj;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(input.Position, matWorldViewProj);
	output.Position.z = input.Position.z;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if(input.Position.z > 0.2)
		return float4(0, 1, 0, 0.5);
	return float4(0, 0, 1, 0.7);
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}