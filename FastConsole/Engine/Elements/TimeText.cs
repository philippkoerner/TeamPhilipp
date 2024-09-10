namespace FastConsole.Engine.Elements;

public class TimeText : Text
{
	public string Template { get; set; }
	
	public override void Update()
	{
		Value = string.Format(Template, DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
		
		base.Update();
	}
}