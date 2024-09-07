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
    long lastRefreshTime = 0;
    double refreshRate = 1.0 / 25.0;
    private int _playerY = 1;
    private int _playerX = 1;
    private int _playerchunk = 00;
    Dictionary<int,char[,]> Chunk = new Dictionary<int, char[,]>();
    
    public void GenerateChunk(int height,int width,int key)
    {
        
        char[,] chunk = new char[height,width];
        Random rand = new Random();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int randValue = rand.Next(0, 100);

                if (randValue < 20)
                {
                    chunk[x, y] = '■';
                }
                else
                {
                    chunk[x, y] = ' ';
                }
            }
        }
        Chunk.Add(key,chunk);
    }
    public void PrintChunk(int height,int width,int key)
    {
        
        for (int y = 0;y < height;y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (x == _playerX && y == _playerY)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write("*");
                    Console.ResetColor();
                    Console.Write(" ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(Chunk[_playerchunk][y, x] + " ");
                   
                }
                
            }
            Console.WriteLine();
        }
    }
    private void HandleInput(ref bool isRunning)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.S:
                    MovePlayer(0, 1);
                    break;
                case ConsoleKey.A:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.D:
                    MovePlayer(1, 0);
                    break;
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
            }
        }
    }

    private void MovePlayer(int dx, int dy)
    {
        int newX = _playerX + dx;
        int newY = _playerY + dy;
        if (Chunk[_playerchunk][newY,newX] != '■')
        {
            _playerX = newX;
            _playerY = newY;
        }
    }

    public override Scene Render()
    {
        bool isRunning = true;
        Console.CursorVisible = false;
        GenerateChunk(8, 8, _playerchunk);
        while (isRunning)
        {
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(lastRefreshTime);
            if (elapsedTime.TotalSeconds > refreshRate)
            {
                Console.SetCursorPosition(0, 0);
                PrintChunk(8, 8, _playerchunk);
                Console.WriteLine("Use WASD to move, press Esc to return to menu.");

                HandleInput(ref isRunning);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            return new MenuScene();


                    }
                }
            };
        }
        return new MenuScene();
    }

}