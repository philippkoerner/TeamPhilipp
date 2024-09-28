using System.Diagnostics;

namespace AdventureGame;

public class MenuScene : Scene
{
    private int boxSize = 32;
    private string[] menuButtons = new string[] { "Start", "About", "Settings", "Exit" };
    private int selectedButtonIndex = 0;

    public override Scene Update()
    {
        bool isRunning = true;
        long lastRefreshTime = 0;
        double refreshRate = 1.0 / 25.0;

        Console.CursorVisible = false;
        while (isRunning)
        {
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(lastRefreshTime);
            if (elapsedTime.TotalSeconds > refreshRate)
            {
                lastRefreshTime = Stopwatch.GetTimestamp();
                selectedButtonIndex = Math.Clamp(selectedButtonIndex, 0, menuButtons.Length - 1);

                Console.SetCursorPosition(0, 0);
                PrintMessageNTimes("-", boxSize);
                Console.WriteLine();
                PrintSurrondedMessage("|", "Tough Choice", "|", boxSize);
                Console.WriteLine();
                PrintSurrondedMessage("|", "Made by: Team Phillip", "|", boxSize);
                Console.WriteLine();
                PrintMessageNTimes("-", boxSize);
                Console.WriteLine();

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    if (i == selectedButtonIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        PrintSurrondedMessage("*", menuButtons[i], "*", boxSize);
                        Console.ResetColor();
                    }
                    else
                    {
                        PrintSurrondedMessage("", menuButtons[i], "", boxSize);
                    }
                    Console.WriteLine();
                }

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.W:
                            selectedButtonIndex--;
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.S:
                            selectedButtonIndex++;
                            break;
                        case ConsoleKey.Enter:
                            switch (selectedButtonIndex)
                            {
                                case 0:
                                    return new GameScene(); 
                                case 1:
                                    return new AboutScene();
                                case 3:
                                    isRunning = false;
                                    Console.WriteLine("Exiting...");
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        Console.WriteLine("Good bye");
        return null!;
    }

    private void PrintMessageNTimes(string message, int n)
    {
        for (int i = 0; i < n; i++)
        {
            Console.Write(message);
        }
    }

    private void PrintSurrondedMessage(string before, string message, string after, int boxSize, double alignment = 0.5)
    {
        Console.Write(before);
        boxSize = boxSize - (before.Length + after.Length);
        int start = (int)((boxSize - message.Length) * alignment);
        PrintMessageNTimes(" ", start);
        Console.Write(message);
        PrintMessageNTimes(" ", boxSize - start - message.Length);
        Console.Write(after);
    }
}
