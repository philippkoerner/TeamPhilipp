using System.Drawing;
using FastConsole.Engine.Core;

namespace FastConsole.Engine.Elements;

public class Canvas : Element
{
	private Cell[,] _cells;
	
	public Size CanvasSize
	{
		get => new Size(_cells.GetLength(0), _cells.GetLength(1));
		set
		{
			_cells = new Cell[value.Width, value.Height];
			Clear();
		}
	}

	public char DefaultCellContent { get; set; } = ' ';
	public int CellWidth { get; set; } = 2;
	public Color? BrushForegroundColor { get; set; }
	public Color? BrushBackgroundColor { get; set; }
	
	public class Image
	{
		public Cell[,] Data { get; }

		public int Width => Data.GetLength(0);
		public int Height => Data.GetLength(1);
		public Size Size => new Size(Width, Height);
		
		public Image(Cell[,] data)
		{
			Data = data;
		}
	}
	
	public struct Cell : IEquatable<Cell>
	{
		public char Value;
		public Color? Foreground;
		public Color? Background;
		public bool Equals(Cell other)
		{
			return Value == other.Value && Nullable.Equals(Foreground, other.Foreground) && Nullable.Equals(Background, other.Background);
		}

		public override bool Equals(object? obj)
		{
			return obj is Cell other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Foreground, Background);
		}

		public static bool operator ==(Cell left, Cell right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Cell left, Cell right)
		{
			return !left.Equals(right);
		}
	}

	public Canvas(Size canvasSize)
	{
		CanvasSize = canvasSize;
		Clear();
	}

	public void Clear()
	{
		for (int y = 0; y < _cells.GetLength(1); y++)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				_cells[x, y] = new Cell()
				{
					Value = DefaultCellContent
				};
			}
		}
	}

	public void LoadImage(Image image)
	{
		if (CanvasSize != image.Size)
		{
			CanvasSize = image.Size;
		}

		for (int y = 0; y < image.Height; y++)
		{
			for (int x = 0; x < image.Width; x++)
			{
				_cells[x, y] = image.Data[x, y];
			}
		}
	}

	public Image GetImage()
	{
		Cell[,] data = new Cell[CanvasSize.Width, CanvasSize.Height];
		for (int y = 0; y < _cells.GetLength(1); y++)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				Cell cell = _cells[x, y];

				data[x, y] = cell;
			}
		}	

		return new Image(data);
	}

	public bool IsCellOutside(int x, int y)
	{
		if (x < 0 || y < 0)
		{
			return true;
		}

		if (x >= _cells.GetLength(0) || y >= _cells.GetLength(1))
		{
			return true;
		}

		return false;
	}

	public void Fill(Color? background = null, Color? foreground = null, char? content = null)
	{
		FillRect(0, 0, CanvasSize.Width, CanvasSize.Height, background, foreground, content);
	}

	public void FillRect(int x, int y, int width, int height, Color? background = null, Color? foreground = null,
		char? content = null)
	{
		FillRect(new Point(x, y), new Size(width, height), background, foreground, content);
	}

	public void FillRect(Point position, Size size, Color? background = null, Color? foreground = null, char? content = null)
	{
		for (int y = 0; y < size.Height; y++)
		{
			for (int x = 0; x < size.Width; x++)
			{
				FillCell(position.X + x, position.Y + y, background, foreground, content);
			}
		}
	}

	public void FillCell(Point position, Color? background = null, Color? foreground = null, char? content = null)
	{
		FillCell(position.X, position.Y, background, foreground, content);
	}

	public void FillCell(int x, int y, Color? background = null, Color? foreground = null, char? content = null)
	{
		if (IsCellOutside(x, y))
			return;
		
		background ??= BrushBackgroundColor;
		foreground ??= BrushForegroundColor;
		content ??= DefaultCellContent;

		_cells[x, y] = new Cell()
		{
			Value = content.Value,
			Background = background,
			Foreground = foreground
		};
	}

	public override void Update()
	{
		Size = new Size(CanvasSize.Width * CellWidth, CanvasSize.Height);
	}

	protected override void OnRender()
	{
		for (int y = 0; y < _cells.GetLength(1); y++)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				Cell cell = _cells[x, y];
				for (int s = 0; s < CellWidth; s++)
				{
					Renderer.WriteCell(cell.Value, cell.Foreground, cell.Background);
				}
			}
			MoveCursorToLineStart();
			MoveCursor(new Point(0, 1));
		}	
	}
}