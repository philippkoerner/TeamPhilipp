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

public class GameScene : Scene
{
    long lastRefreshTime = 0;
    double refreshRate = 1.0 / 10.0;
    private int _playerY = 1;
    private int _playerX = 1;
    private int _playerchunk = 00;
    int chunksize = 8;
    Dictionary<int, char[,]> Chunk = new Dictionary<int, char[,]>();
    List<int> keys = new List<int>();
    List<Enemy> enemies = new List<Enemy>();

    public GameScene()
    {
        GenerateInitialEnemies();
    }

    private void GenerateInitialEnemies()
    {
        enemies.Add(new Enemy(2, 2, 'E', Chunk.ContainsKey(_playerchunk) ? Chunk[_playerchunk] : new char[chunksize, chunksize]));
        enemies.Add(new Enemy(5, 5, 'E', Chunk.ContainsKey(_playerchunk) ? Chunk[_playerchunk] : new char[chunksize, chunksize]));
    }

    public void GenerateChunk(int chunksize, int key)
    {
        int height = chunksize;
        int width = chunksize;
        char[,] chunk = new char[height, width];
        Random rand = new Random();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int randValue = rand.Next(0, 100);

                if (randValue < 10)
                {
                    chunk[x, y] = '■';
                }
                else
                {
                    chunk[x, y] = ' ';
                }
            }
        }

        if (!Chunk.ContainsKey(key))
        {
            Chunk.Add(key, chunk);
            keys.Add(key);
        }
    }

    private void PrintChunk(int height, int width, int key)
    {
        if (!Chunk.ContainsKey(key))
        {
            Console.WriteLine("Chunk not found!");
            return;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == _playerX && y == _playerY)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write("*");
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

    public override Scene Render()
    {
        bool isRunning = true;
        Console.CursorVisible = false;
        GenerateChunk(chunksize, _playerchunk);

        foreach (var enemy in enemies)
        {
            enemy.UpdateChunk(Chunk[_playerchunk]);
        }

        while (isRunning)
        {
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(lastRefreshTime);
            if (elapsedTime.TotalSeconds > refreshRate)
            {
                lastRefreshTime = Stopwatch.GetTimestamp();
                Console.SetCursorPosition(0, 0);
                PrintChunk(8, 8, _playerchunk);

                foreach (var enemy in enemies)
                {
                    enemy.Render();
                }

                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Use WASD to move, press Esc to return.");

                HandleInput(ref isRunning);

                foreach (var enemy in enemies)
                {
                    enemy.Move();
                    enemy.UpdateChunk(Chunk[_playerchunk]);
                }

                foreach (var enemy in enemies)
                {
                    if (_playerX == enemy.X && _playerY == enemy.Y)
                    {
                        Console.Clear();
                        Console.WriteLine("Game Over! You were caught by an enemy.");
                        return new MenuScene();
                    }
                }
            }
        }
        return new MenuScene();
    }

    private void HandleInput(ref bool isRunning)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    MovePlayer(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
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
        int oldchunk = _playerchunk;

        if (newY == 0)
        {
            GenerateChunk(chunksize, _playerchunk + 1);
        }
        if (newY == 7)
        {
            GenerateChunk(chunksize, _playerchunk - 1);
        }
        if (newX == 0)
        {
            GenerateChunk(chunksize, _playerchunk - 10);
        }
        if (newX == 7)
        {
            GenerateChunk(chunksize, _playerchunk + 10);
        }
        if (newY > 7)
        {
            _playerchunk -= 1;
            newY = 0;
        }
        if (newY < 0)
        {
            _playerchunk += 1;
            newY = 7;
        }
        if (newX > 7)
        {
            _playerchunk += 10;
            newX = 0;
        }
        if (newX < 0)
        {
            _playerchunk -= 10;
            newX = 7;
        }
        if (Chunk[_playerchunk][newY, newX] != '■')
        {
            _playerX = newX;
            _playerY = newY;
        }
        else
        {
            _playerchunk = oldchunk;
        }
    }
}

public class Enemy
{
    public int X { get; set; }
    public int Y { get; set; }
    private char representation;
    private long lastMoveTime;
    private double moveInterval = 1.0 / 2.0;
    private char[,] chunk;

    public Enemy(int x, int y, char representation, char[,] chunk)
    {
        X = x;
        Y = y;
        this.representation = representation;
        lastMoveTime = Stopwatch.GetTimestamp();
        this.chunk = chunk;
    }

    public void Render()
    {
        Console.SetCursorPosition(X * 2, Y);
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write(representation);
        Console.ResetColor();
    }

    public void Move()
    {
        long currentTime = Stopwatch.GetTimestamp();
        double elapsedSeconds = (currentTime - lastMoveTime) / (double)Stopwatch.Frequency;

        if (elapsedSeconds > moveInterval)
        {
            lastMoveTime = currentTime;

            Random rand = new Random();
            int direction = rand.Next(4);

            int newX = X;
            int newY = Y;

            switch (direction)
            {
                case 0: newY--; break;
                case 1: newY++; break;
                case 2: newX--; break;
                case 3: newX++; break;
            }

            newX = Math.Clamp(newX, 0, 7);
            newY = Math.Clamp(newY, 0, 7);

            if (chunk[newY, newX] != '■')
            {
                X = newX;
                Y = newY;
            }
        }
    }

    public void UpdateChunk(char[,] newChunk)
    {
        chunk = newChunk;
    }
}
