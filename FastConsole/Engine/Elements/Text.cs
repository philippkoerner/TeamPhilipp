using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Utils;

namespace FastConsole.Engine.Elements;

public class Text : Graphic
{
	private string[] _lines;
	private string _value;
	private bool _isLinesDirty;

	public string Value
	{
		get => _value;
		set
		{
			if (_value == value)
				return;

			_value = value;
			_isLinesDirty = true;
		}
	}
	public Alignment Alignment { get; set; }
	public bool BackgroundOnlyWhereTextIs { get; set; }
	public TextStyle Style { get; set; }
	public Size FitSize
	{
		get
		{
			EnsureLinesIsUpToDate();

			int fitHeight = _lines.Length;
			int fitWidth = _lines.Max(line => line.Length);

			return new Size(fitWidth, fitHeight);
		}
	}

	private void EnsureLinesIsUpToDate()
	{
		if (_isLinesDirty)
		{
			_lines = _value.Split("\n", StringSplitOptions.RemoveEmptyEntries);
			_isLinesDirty = false;
		}		
	}

	private void RenderLine(string line)
	{
		if (line.Length > Size.Width)
		{
			line = line[..Size.Width];
		}

		int start = Alignment.AlignHorizontally(line.Length, Size.Width);
		if (BackgroundOnlyWhereTextIs)
		{
			MoveCursor(new Point(start, 0));
		}
		else
		{
			line = $"{new string(' ', start)}{line}{new string(' ', Size.Width - line.Length - start)}";
		}
		
		Renderer.Write(line, Foreground, Background, Style);
		MoveCursorToLineStart();
		MoveCursor(new Point(0, 1));
	}
	
	protected override void OnRender()
	{
		EnsureLinesIsUpToDate();	

		int lines = Math.Min(_lines.Length, Size.Height);
		int start = Alignment.AlignVertically(lines, Size.Height);

		if (BackgroundOnlyWhereTextIs)
		{
			MoveCursor(new Point(0, start));
			foreach (string line in _lines)
			{
				RenderLine(line);
			}
		}
		else
		{
			for (var i = 0; i < Size.Height; i++)
			{
				var line = (i >= start && i < _lines.Length + start) ? _lines[i - start] : new string(' ', Size.Width);
				RenderLine(line);
			}
		}
	}
}