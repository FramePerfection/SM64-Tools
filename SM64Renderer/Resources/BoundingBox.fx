float4x4 World, ViewProjection;

float4 Color = 1;

float4 PixelShaderFunc() : COLOR
{
	return Color;
}

float4 PickselShaderFunc(out float4 c0 : COLOR0) : COLOR1
{
	c0 = 0;
	return Color;
}

float4 AxisShaderFunc(float4 inColor : TEXCOORD0) : COLOR0
{
	return inColor;
}

void TransformVertex(float3 inPosition : POSITION0, out float4 outPosition : POSITION)
{
	outPosition = mul(float4(inPosition * 1.001f, 1), mul(World, ViewProjection));
}

void TransformAxis(float3 inPosition : POSITION0, float4 inColor : COLOR0, out float4 outPosition : POSITION, out float4 outColor : TEXCOORD0)
{
	outPosition = mul(float4(inPosition, 1), mul(World, ViewProjection));
	outColor = inColor;
}

TECHNIQUE T1
{
	PASS P1
	{
		VertexShader = compile vs_2_0 TransformVertex();
		PixelShader = compile ps_2_0 PixelShaderFunc();
	}
	PASS P2
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		VertexShader = compile vs_2_0 TransformVertex();
		PixelShader = compile ps_2_0 PickselShaderFunc();
	}
	PASS P3
	{
		VertexShader = compile vs_2_0 TransformAxis();
		PixelShader = compile ps_2_0 AxisShaderFunc();
	}
}