using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Utils;

namespace FastConsole.Engine.Elements;

public class FlexBox : Element
{
	private bool _needRecalculation;
	
	public List<Element> Children { get; set; } = new List<Element>();
	public int Spacing { get; set; }
	public GrowDirection GrowDirection { get; set; }
	public Alignment Alignment { get; set; }
	public bool AlwaysRecalculate { get; set; } = false;

	public void RequestRecalculation()
	{
		_needRecalculation = true;
	}

	private int GetGrowFitSize()
	{
		int size = 0;
		
		foreach (Element child in Children)
		{
			size += child.Size.GetGrowAxis(GrowDirection);
		}

		size += Math.Max(Children.Count - 1, 0) * Spacing;

		return size;
	}

	public override void Update()
	{
		foreach (Element child in Children)
		{
			child.Update();
		}

		if (AlwaysRecalculate == false && _needRecalculation == false)
			return;
		
		RecalculateLayout();
	}

	private void RecalculateLayout()
	{
		int containerGrow = Size.GetGrowAxis(GrowDirection);
		int containerFixed = Size.GetFixedAxis(GrowDirection);

		int growFitSize = GetGrowFitSize();

		int growStart = GrowDirection.AlignGrow(growFitSize, containerGrow, Alignment);
		
		foreach (Element child in Children)
		{
			int childGrowSize = child.Size.GetGrowAxis(GrowDirection);
			int childFixedSize = child.Size.GetFixedAxis(GrowDirection);

			child.Position =
				GrowDirection.GetPoint(growStart, GrowDirection.AlignFixed(childFixedSize, containerFixed, Alignment));
			child.Position = new Point(child.Position.X + Position.X, child.Position.Y + Position.Y);

			growStart += childGrowSize;
			growStart += Spacing;
		}
	}

	protected override void OnRender()
	{
		foreach (Element child in Children)
		{
			child.Render();
		}		
	}
}