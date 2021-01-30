using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings {
	public BoolParameter depthEdge = new BoolParameter();
	public BoolParameter normalEdge = new BoolParameter();
	public BoolParameter useColor = new BoolParameter();
	public IntParameter scale = new IntParameter { value = 1 };
	public FloatParameter edgeDepthFactor = new FloatParameter { value = 100 };
	[Range(0, 1)]
	public FloatParameter normalThreshold = new FloatParameter { value = 0.4f };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        
		sheet.properties.SetInt("_DeptEdge", settings.depthEdge ? 1 : 0);
		sheet.properties.SetInt("_NormalEdge", settings.normalEdge ? 1 : 0);
		sheet.properties.SetInt("_UseColor", settings.useColor ? 1 : 0);
		sheet.properties.SetFloat("_Scale", settings.scale);
		sheet.properties.SetFloat("_EdgeDepthFactor", settings.edgeDepthFactor);
		sheet.properties.SetFloat("_NormalThreshold", settings.normalThreshold);
        
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}