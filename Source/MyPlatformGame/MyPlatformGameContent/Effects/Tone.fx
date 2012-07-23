float4x4 World;
float4x4 View;
float4x4 Projection;

float time;
float2 pos;
// TODO: add effect parameters here.
sampler s;

struct VertexShaderInput
{
    float4 Position : POSITION0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // TODO: add your vertex shader code here.

    return output;
}

float Tone; // from -1 to 1, last is gray from 0 to 1
float2 Position;
float hwRatio;
float maxx(float a,float b,float c)
{
	return max(max(a,b),c);
}
float minn(float a,float b,float c)
{
	return min(min(a,b),c);
}
float4 TonePixelShaderFunction(float2 uv:TEXCOORD0,float4 color:COLOR) : COLOR0
{
	float4 o=tex2D(s,uv);
	float L=(maxx(o.r,o.g,o.b)+minn(o.r,o.g,o.b))/2.0;
	//float2 a=float2(Position.x*hwRatio,Position.v);
	float2 diff=uv-Position;
	float2 diffFix=float2(diff.x*hwRatio,diff.y);
	float distf=length(diffFix)/0.4;
	
	o.rgb=(L*Tone+o.rgb*(1-Tone))*(1-Tone*sin(max(min(distf,1),0.3)*1.57));
	o=o*color;
	return o;
}
technique ToneTech
{
    pass Pass1
    {
        // TODO: set renderstates here.

        PixelShader = compile ps_2_0 TonePixelShaderFunction();
    }
}
