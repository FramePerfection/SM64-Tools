float4x4 World, ViewProjection;

TEXTURE Texture0, Texture1;
float4 Color = 1;

float4 CameraDirection;
float4 DarkShading = 1, BrightShading = 1;

float4 ShiftMul;
float4 PickColor;

sampler DefaultSampler = sampler_state
{Texture = <Texture0>; MagFilter = Linear; AddressU = Wrap; AddressV = Wrap;};

float4 PixelShaderFunc(float2 inTex : TEXCOORD0, float3 inNormal : TEXCOORD1, out float4 outPickColor : COLOR1) : COLOR0
{
	float4 col = tex2D(DefaultSampler, inTex);
	col *= DarkShading + (BrightShading - DarkShading) * max(0, -dot(CameraDirection.xyz, inNormal));
	col *= Color;
	outPickColor = PickColor ;
	return col;
}

float4 PixelShaderPickOnly(out float4 outPickColor : COLOR1) : COLOR
{
	outPickColor = PickColor;
	return 0;
}

void TransformVertex(float3 inPosition : POSITION0, float3 inNormal : TEXCOORD1, float2 inTex : TEXCOORD0, out float4 outPosition : POSITION, out float2 outTex : TEXCOORD0, out float3 outNormal : TEXCOORD1)
{
	outPosition = mul(float4(inPosition, 1), mul(World, ViewProjection));
	outNormal = normalize(mul(inNormal, World));
	outTex = inTex * ShiftMul.xy;
}

TECHNIQUE T1
{
	PASS P1
	{
		CullMode = None;
		VertexShader = compile vs_2_0 TransformVertex();
		PixelShader = compile ps_2_0 PixelShaderFunc();
	}
	PASS P2
	{
		CullMode = None;
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		VertexShader = compile vs_2_0 TransformVertex();
		PixelShader = compile ps_2_0 PixelShaderFunc();
	}
}