using System.Runtime.InteropServices;

namespace FastConsole.Engine.Core;

public static class Windows
{
	private const int STD_OUTPUT_HANDLE = -11;
	private const int STD_ERROR_HANDLE = -12;
	private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
	private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

	[DllImport("kernel32.dll")]
	private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

	[DllImport("kernel32.dll")]
	private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetStdHandle(int nStdHandle);

	[DllImport("kernel32.dll")]
	public static extern uint GetLastError();

	public static void ForceUpgradeToAnsi()
	{
		if (TryUpgradeToAnsi() == false)
			throw new Exception("Ansi support required");
	}
	
	public static bool TryUpgradeToAnsi()
	{
		try
		{
			var @out = GetStdHandle(STD_OUTPUT_HANDLE);
			if (!GetConsoleMode(@out, out var mode))
			{
				return false;
			}

			if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
			{
				// Try enable ANSI support.
				mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
				if (!SetConsoleMode(@out, mode))
				{
					// Enabling failed.
					return false;
				}
			}

			return true;
		}
		catch
		{
			// All we know here is that we don't support ANSI.
			return false;
		}
	}
}