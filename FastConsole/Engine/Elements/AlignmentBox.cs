using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Utils;

namespace FastConsole.Engine.Elements;

public class AlignmentBox : Element
{
	public Element Child { get; set; }
	public Alignment Alignment { get; set; }

	public AlignmentBox(Element child)
	{
		Child = child;
	}

	public override void Update()
	{
		if (Child != null)
		{
			Child.Update();
			int startHorizontal = Alignment.AlignHorizontally(Child.Size.Width, Size.Width);
			int startVertical = Alignment.AlignVertically(Child.Size.Height, Size.Height);

			Child.Position = new Point(Position.X + startHorizontal, Position.Y + startVertical);
		}
	}

	protected override void OnRender()
	{
		Child?.Render();
	}
}