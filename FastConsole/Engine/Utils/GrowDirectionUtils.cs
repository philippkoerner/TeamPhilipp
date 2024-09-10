using System.Drawing;
using FastConsole.Engine.Core;

namespace FastConsole.Engine.Utils;

public static class GrowDirectionUtils
{
	public static int GetGrowAxis(this Point position, GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Horizontal => position.X,
			_ => position.Y
		};
	}

	public static int GetFixedAxis(this Point position, GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Horizontal => position.Y,
			_ => position.X
		};
	}

	public static int GetGrowAxis(this Size size, GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Horizontal => size.Width,
			_ => size.Height
		};
	}

	public static int GetFixedAxis(this Size size, GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Horizontal => size.Height,
			_ => size.Width
		};
	}

	public static Point GetPoint(this GrowDirection direction, int grow, int @fixed)
	{
		return direction switch
		{
			GrowDirection.Horizontal => new Point(grow, @fixed),
			_ => new Point(@fixed, grow)
		};
	}

	public static Size GetSize(this GrowDirection direction, int grow, int @fixed)
	{
		return direction switch
		{
			GrowDirection.Horizontal => new Size(grow, @fixed),
			_ => new Size(@fixed, grow)
		};
	}

	public static GrowDirection GetOpposite(this GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Horizontal => GrowDirection.Vertical,
			_ => GrowDirection.Horizontal
		};
	}

	public static int AlignGrow(this GrowDirection direction, int elementSize, int containerSize, Alignment alignment)
	{
		return direction switch
		{
			GrowDirection.Horizontal => alignment.AlignHorizontally(elementSize, containerSize),
			_ => alignment.AlignVertically(elementSize, containerSize)
		};
	}
	
	public static int AlignFixed(this GrowDirection direction, int elementSize, int containerSize, Alignment alignment)
	{
		return direction switch
		{
			GrowDirection.Horizontal => alignment.AlignVertically(elementSize, containerSize),
			_ => alignment.AlignHorizontally(elementSize, containerSize)
		};
	}
}