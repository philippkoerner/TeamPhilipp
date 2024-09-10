using System.Diagnostics;

namespace FastConsole.Engine.Core;

public static class Time
{
	private static long _timestamp;

	public static double NowSeconds => new TimeSpan(_timestamp).TotalSeconds;
	public static double DeltaTime { get; private set; }
	public static double RefreshRate { get; set; } = 1 / 60.0;
	public static double TargetFPS
	{
		get => 1 / RefreshRate;
		set => RefreshRate = 1 / value;
	}

	public static bool TryUpdate()
	{
		TimeSpan delta = Stopwatch.GetElapsedTime(_timestamp);

		double deltaTime = delta.TotalSeconds;
		if (deltaTime > RefreshRate)
		{
			_timestamp = Stopwatch.GetTimestamp();
			DeltaTime = deltaTime;
			return true;
		}

		return false;
	}
}