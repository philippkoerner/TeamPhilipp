using System.Drawing;
using FastConsole.Engine.Core;

namespace FastConsole.Engine.Elements;

public class Box : Graphic
{
	public Element? Child { get; set; }
	public char TopAndBottom { get; set; }
	public char LeftAndRight { get; set; }
	public char Corners { get; set; }
	
	public Box(char topAndBottom, char corners, char leftAndRight)
	{
		TopAndBottom = topAndBottom;
		Corners = corners;
		LeftAndRight = leftAndRight;
	}

	public static Box DefaultBox(Point position, Size size, Element? child)
	{
		return new Box('-', '*', '|')
		{
			Position = position,
			Size = size,
			Child = child
		};
	}

	public override void Update()
	{
		if (Child != null)
		{
			Child.Position = new Point(Position.X + 1, Position.Y + 1);
			Child.Size = new Size(Size.Width - 2, Size.Height - 2);
			Child.Update();
		}
	}
	
	private void RenderHorizontalBorder()
	{
		string border = $"{Corners}{new string(TopAndBottom, Math.Max(Size.Width - 2, 0))}{Corners}";
		border = border[..Size.Width];
		
		Renderer.Write(border, Foreground, Background);
		MoveCursorToLineStart();
		MoveCursor(new Point(0, 1));
	}

	private void RenderVerticalBorder()
	{
		if (Child == null)
		{
			string border = $"{new string(LeftAndRight, Math.Max(Size.Width, 0))}";
			border = border[..Size.Width];
			Renderer.Write(border, Foreground, Background);
		}
		else
		{
			string border = LeftAndRight.ToString();

			for (int i = 0; i < Math.Clamp(Size.Width, 0, 2); i++)
			{
				Renderer.Write(border, Foreground, Background);
				if (i == 0)
				{
					MoveCursor(new Point(Size.Width - 2, 0));
				}
			}
		}
		
		MoveCursorToLineStart();
		MoveCursor(new Point(0, 1));
	}

	protected override void OnRender()
	{
		for (int i = 0; i < Size.Height; i++)
		{
			if (i == 0 || Size.Height - 1 == i)
			{
				RenderHorizontalBorder();
			}
			else
			{
				RenderVerticalBorder();
			}
		}

		Child?.Render();
	}
}