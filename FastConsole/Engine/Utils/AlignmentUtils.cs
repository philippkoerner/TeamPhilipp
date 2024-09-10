using FastConsole.Engine.Core;

namespace FastConsole.Engine.Utils;

public static class AlignmentUtils
{
	public static int AlignHorizontally(this Alignment alignment, int elementSize, int containerSize)
	{
		double alignmentSize = alignment switch
		{
			Alignment.Center or Alignment.BottomMiddle or Alignment.TopMiddle => 0.5,
			Alignment.MiddleLeft or Alignment.BottomLeft or Alignment.TopLeft => 0,
			Alignment.MiddleRight or Alignment.BottomRight or Alignment.TopRight => 1,
			_ => 0
		};	
		
		return (int)((containerSize - elementSize) * alignmentSize);	
	}
	
	public static int AlignVertically(this Alignment alignment, int elementSize, int containerSize)
	{
		double alignmentSize = alignment switch
		{
			Alignment.Center or Alignment.MiddleLeft or Alignment.MiddleRight => 0.5,
			Alignment.TopMiddle or Alignment.TopLeft or Alignment.TopRight => 0,
			Alignment.BottomMiddle or Alignment.BottomLeft or Alignment.BottomRight => 1,
			_ => 0
		};
		
		return (int)((containerSize - elementSize) * alignmentSize);	
	}	
}