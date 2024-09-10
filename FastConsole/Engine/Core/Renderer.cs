using System.Diagnostics;
using System.Drawing;

namespace FastConsole.Engine.Core;

public class Renderer
{
	private static Node[,] _screenBuffer = new Node[300, 40];
	private static Node[,] _renderBuffer = new Node[300, 40];
	private static int _windowLastWidth;
	private static int _windowLastHeight;

	public static bool DebugMode { get; set; } = false;
	public static int Width => _renderBuffer.GetLength(0);
	public static int Height => _renderBuffer.GetLength(1);
	public static Point CursorPosition { get; set; }
	
	private struct Node : IEquatable<Node>
	{
		public char Value;
		public Color? Foreground;
		public Color? Background;
		public TextStyle Style;
		public bool Equals(Node other)
		{
			return Value == other.Value && Nullable.Equals(Foreground, other.Foreground) && Nullable.Equals(Background, other
				.Background) && Style == other.Style;
		}

		public override bool Equals(object? obj)
		{
			return obj is Node other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Foreground, Background, Style);
		}

		public static bool operator ==(Node left, Node right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Node left, Node right)
		{
			return !left.Equals(right);
		}
	}
	
	private struct StyleRange
	{
		public Color? Foreground;
		public Color? Background;
		public TextStyle Style;
		public int Range;

		public int Size
		{
			get
			{
				if (Foreground == null && Background == null && Style == TextStyle.None)
					return Range;

				int size = 7 + Range - 1;
				if (Foreground != null)
				{
					size += 16;
					size += 1;
				}

				if (Background != null)
				{
					size += 16;
					size += 1;
				}

				if (Style.HasFlag(TextStyle.Bold))
				{
					size += 2;
				}

				if (Style.HasFlag(TextStyle.Inverted))
				{
					size += 2;
				}

				if (Style.HasFlag(TextStyle.Italic))
				{
					size += 2;
				}

				if (Style.HasFlag(TextStyle.Underline))
				{
					size += 2;
				}

				return size;
			}
		}

		public void WriteStart(Span<char> span, ref int i)
		{
			if (Foreground == null && Background == null && Style == TextStyle.None)
			{
				return;
			}
			
			WriteCSI(span, ref i);
			int elementsAmount = 0;
			if (Style != TextStyle.None)
			{
				WriteTextStyles(span, ref elementsAmount, ref i);
			}
			
			if (Foreground != null)
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				WriteForeground(span, Foreground.Value, ref i);
			}

			if (Background != null)
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				WriteBackground(span, Background.Value, ref i);
			}

			span[i++] = 'm';
		}

		public void WriteSeparator(Span<char> span, ref int elementsAmount, ref int i)
		{
			if (elementsAmount > 0)
			{
				span[i++] = ';';
			}

			elementsAmount++;
		}

		public void WriteTextStyles(Span<char> span, ref int elementsAmount, ref int i)
		{
			if (Style.HasFlag(TextStyle.Bold))
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				span[i++] = '1';
			}
			
			if (Style.HasFlag(TextStyle.Italic))
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				span[i++] = '3';
			}
			
			if (Style.HasFlag(TextStyle.Inverted))
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				span[i++] = '7';
			}
			
			if (Style.HasFlag(TextStyle.Underline))
			{
				WriteSeparator(span, ref elementsAmount, ref i);
				span[i++] = '4';
			}
		}

		public void WriteForeground(Span<char> span, Color color, ref int i)
		{
			span[i++] = '3';
			span[i++] = '8';
			span[i++] = ';';
			span[i++] = '2';
			span[i++] = ';';
			WriteColor(span, color, ref i);
		}

		public void WriteBackground(Span<char> span, Color color, ref int i)
		{
			span[i++] = '4';
			span[i++] = '8';
			span[i++] = ';';
			span[i++] = '2';
			span[i++] = ';';
			WriteColor(span, color, ref i);
		}
		
		public void WriteColor(Span<char> span, Color color, ref int i)
		{
			span[i++] = (char)('0' + color.R / 100);
			span[i++] = (char)('0' + color.R % 100 / 10);
			span[i++] = (char)('0' + color.R % 10);
			span[i++] = ';';
			span[i++] = (char)('0' + color.G / 100);
			span[i++] = (char)('0' + color.G % 100 / 10);
			span[i++] = (char)('0' + color.G % 10);
			span[i++] = ';';
			span[i++] = (char)('0' + color.B / 100);
			span[i++] = (char)('0' + color.B % 100 / 10);
			span[i++] = (char)('0' + color.B % 10);	
		}
		
		public void WriteCSI(Span<char> span, ref int i)
		{
			span[i++] = '\u001b';
			span[i++] = '[';
		}

		public void WriteEnd(Span<char> span, ref int i)
		{
			if (Foreground == null && Background == null && Style == TextStyle.None)
				return;
			
			WriteCSI(span, ref i);
			span[i++] = '0';
			span[i++] = 'm';
		}
	}

	public static void Occupy(int width, int height)
	{
		_renderBuffer = new Node[width, height];
		_screenBuffer = new Node[width, height];
	}

	public static void Clear()
	{
		for (int y = 0; y < _windowLastHeight && y < _renderBuffer.GetLength(1); y++)
		{
			for (int x = 0; x < _windowLastWidth && x < _renderBuffer.GetLength(0); x++)
			{
				_renderBuffer[x, y] = new Node()
				{
					Value = ' '
				};
			}
		}

		if (DebugMode)
		{
			OutputToConsole();
		}
	}

	public static bool IsOutsideBuffer(Point position)
	{
		if (position.X < 0 || position.Y < 0)
		{
			return true;
		}

		if (position.X >= _windowLastWidth || position.Y >= _windowLastHeight)
		{
			return true;
		}

		if (position.X >= _renderBuffer.GetLength(0) || position.Y >= _renderBuffer.GetLength(1))
		{
			return true;
		}

		return false;
	}

	public static void WriteCell(char value, Color? foreground = null, Color? background = null, TextStyle style = TextStyle.None)
	{
		int x = CursorPosition.X;
		int y = CursorPosition.Y;

		if (IsOutsideBuffer(CursorPosition))
		{
			CursorPosition = new Point(x + 1, y);
			return;
		}

		_renderBuffer[x, y] = new Node()
		{
			Value = value,
			Background = background,
			Foreground = foreground,
			Style = style
		};

		CursorPosition = new Point(x + 1, y);

		if (DebugMode)
		{
			OutputToConsole();
		}
	}

	public static void Write(string str, Color? foreground = null, Color? background = null, TextStyle style = TextStyle.None)
	{
		int x = CursorPosition.X;
		int y = CursorPosition.Y;

		foreach (char c in str)
		{
			if (c == '\n')
			{
				y++;
				x = 0;
				continue;
			}

			if (IsOutsideBuffer(new Point(x, y)))
			{
				x++;
				continue;
			}
			
			_renderBuffer[x, y] = new Node()
			{
				Value = c,
				Background = background,
				Foreground = foreground,
				Style = style
			};
			x++;
		}

		CursorPosition = new Point(x, y);

		if (DebugMode)
		{
			OutputToConsole();
		}
	}

	public static void SetCursorPosition(int x, int y)
	{
		CursorPosition = new Point(x, y);
	}

	public static void OutputToConsole()
	{
		Console.CursorVisible = DebugMode ^ true;
		Console.SetCursorPosition(0, 0);
		int consoleWidth = Console.WindowWidth;
		int consoleHeight = Console.WindowHeight - 1;
		/*if (consoleWidth <= Width || consoleHeight <= Height)
	{
		Clear();
		CursorPosition = new Point(0, 0);
		Write($"Not enough space. Current: {consoleWidth}x{consoleHeight}. Minimum required: {Width}x{Height}");
	}*/

		bool forceDraw = false;
		if (_windowLastWidth != consoleWidth || _windowLastHeight != consoleHeight)
		{
			Console.Clear();
			forceDraw = true;
		}
		
		_windowLastWidth = consoleWidth;
		_windowLastHeight = consoleHeight;

		for (int y = 0; y < consoleHeight && y < _screenBuffer.GetLength(1); y++)
		{
			bool hasChanges = forceDraw;
			for (int x = 0; x < consoleWidth && x < _screenBuffer.GetLength(0); x++)
			{
				Node renderNode = _renderBuffer[x, y];
				Node screenNode = _screenBuffer[x, y];

				hasChanges |= renderNode != screenNode;
				_screenBuffer[x, y] = renderNode;
			}

			if (hasChanges)
			{
				OutputLine(y);
			}
			else if (y + 1 < consoleHeight)
			{
				Console.Write($"\u001b[{y + 1 + 1};0f");
			}
		}

		if (DebugMode)
		{
			Thread.Sleep(600);
		}
	}
	
	private static void OutputLine(int y)
	{
		Queue<StyleRange> styles = new Queue<StyleRange>(16);
		StyleRange lastPair = new StyleRange()
		{
			Background = _renderBuffer[0, y].Background,
			Foreground = _renderBuffer[0, y].Foreground,
			Style = _renderBuffer[0, y].Style
		};
		int size = 0;

		for (int x = 0; x < _windowLastWidth && x < _renderBuffer.GetLength(0); x++)
		{
			Node node = _renderBuffer[x, y];
			if (node.Background != lastPair.Background || node.Foreground != lastPair.Foreground || node.Style != lastPair.Style)
			{
				styles.Enqueue(lastPair);
				size += lastPair.Size;
				lastPair = new StyleRange()
				{
					Background = node.Background,
					Foreground = node.Foreground,
					Style = node.Style,
					Range = 1
				};
			}
			else
			{
				lastPair.Range++;
			}
		}

		size += lastPair.Size;
		styles.Enqueue(lastPair);
		
		string line = string.Create(size + 1, styles, (span, ranges) =>
		{
			int i = 0;
			int x = 0;
			foreach (StyleRange range in ranges)
			{
				range.WriteStart(span, ref i);
				for (int a = 0; a < range.Range; a++, x++)
				{
					span[i++] = _renderBuffer[x, y].Value;
				}
				range.WriteEnd(span, ref i);
			}

			span[i++] = '\0';
		});
		
		Console.WriteLine(line);
	}
}