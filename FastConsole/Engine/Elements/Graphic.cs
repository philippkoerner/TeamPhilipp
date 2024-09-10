using System.Drawing;

namespace FastConsole.Engine.Elements;

public abstract class Graphic : Element
{
	public Color? Foreground { get; set; }
	public Color? Background { get; set; }
}