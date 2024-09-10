namespace FastConsole.Engine.Core;

[Flags]
public enum TextStyle
{
	None = 0,
	
	Bold = 1,
	Italic = 1 << 1,
	Underline = 1 << 2,
	Inverted = 1 << 3,
	
	All = Bold | Italic | Underline | Inverted
}