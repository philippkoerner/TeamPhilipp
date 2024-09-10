using System.Drawing;
using FastConsole.Engine.Core;

namespace FastConsole.Engine.Elements;

public abstract class Element
{
	public Size Size { get; set; }
	public Point Position { get; set; }

	public void Render()
	{
		Renderer.CursorPosition = Position;
		OnRender();
	}

	protected void MoveCursor(Point delta)
	{
		Renderer.CursorPosition = new Point(Renderer.CursorPosition.X + delta.X, Renderer.CursorPosition.Y + delta.Y);
	}

	protected void MoveCursorToLineStart()
	{
		Renderer.CursorPosition = new Point(Position.X, Renderer.CursorPosition.Y);
	}

	public virtual void Update()
	{
		
	}
	protected abstract void OnRender();

	public static void UpdateAndRender(List<Element> elements)
	{
		Renderer.Clear();
		
		foreach (Element element in elements)
		{
			element.Update();
		}

		foreach (Element element in elements)
		{
			element.Render();	
		}
		
		Renderer.OutputToConsole();
	}
}