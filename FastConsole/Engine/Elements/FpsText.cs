using FastConsole.Engine.Core;

namespace FastConsole.Engine.Elements;

public class FpsText : Text
{
	public string Template { get; set; } = "FPS: {0} ({1}ms)";
	
	public override void Update()
	{
		Value = string.Format(Template, (1 / Time.DeltaTime).ToString("F2"), (Time.DeltaTime * 1000).ToString("F2"));
		
		base.Update();
	}
}