using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Scene currentScene = new MenuScene();  
        while (currentScene != null)
        {
            Console.Clear();
            currentScene = currentScene.Render();  
        }
    }
}

public abstract class Scene
{
    public abstract Scene Render();  
}

public class MenuScene : Scene
{
    private int boxSize = 32;
    private string[] menuButtons = new string[] { "Start", "About", "Settings", "Exit" };
    private int selectedButtonIndex = 0;

    public override Scene Render()
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
                        case ConsoleKey.W:
                            selectedButtonIndex--;
                            break;
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
        return null;
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

public class AboutScene : Scene
{
    private int boxSize = 32;

    public override Scene Render()
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

                Console.SetCursorPosition(0, 0);
                PrintMessageNTimes("-", boxSize);
                Console.WriteLine();
                PrintSurrondedMessage("|", "Tough Choice", "|", boxSize);
                Console.WriteLine();
                PrintSurrondedMessage("|", "Make your choice.", "|", boxSize);
                Console.WriteLine();
                PrintSurrondedMessage("|", "Version 0.0", "|", boxSize, 1);
                Console.WriteLine();
                PrintMessageNTimes("-", boxSize);
                Console.WriteLine();
                Console.WriteLine("Press Esc to return");

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            return new MenuScene(); 
                    }
                }
            }
        }

        return null;
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

public class GameScene : Scene
{
    public override Scene Render()
    {
        Console.WriteLine("Game has started!");
        Console.WriteLine("Press Esc to return to the menu.");
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    return new MenuScene();  
                }
            }
        }
    }
}