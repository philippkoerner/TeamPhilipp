using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame;
public class GameScene : Scene
{


    public int _playerY = 1;
    public int _playerX = 1;
    public int _playerchunk = 00;
    private int _chunksize = 16;

    Dictionary<int, char[,]> Chunk = new Dictionary<int, char[,]>();
    List<int> keys = new List<int>();
    List<Enemy> enemies = new List<Enemy>();

    public GameScene()
    {
        GenerateInitialEnemies();
    }

    private void GenerateInitialEnemies()
    {
        enemies.Add(new Enemy(2, 2, 'E', Chunk.ContainsKey(_playerchunk) ? Chunk[_playerchunk] : new char[_chunksize, _chunksize], _playerchunk));
        enemies.Add(new Enemy(5, 5, 'E', Chunk.ContainsKey(_playerchunk) ? Chunk[_playerchunk] : new char[_chunksize, _chunksize], _playerchunk));
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

    private void PrintChunk(int chunksize, int key)
    {
        int height = chunksize;
        int width = chunksize;
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
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(Chunk[_playerchunk][y, x] + " ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine(_playerchunk + "               ");
    }

    public override Scene Update()
    {
        double refreshRate = 1.0 / 25.0;
        long lastRefreshTime = 20;
        bool isRunning = true;
        Console.CursorVisible = false;
        GenerateChunk(_chunksize, _playerchunk);

        foreach (var enemy in enemies)
        {
            enemy.UpdateChunk(Chunk[_playerchunk], _playerchunk);
        }

        while (isRunning)
        {
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(lastRefreshTime);
            if (elapsedTime.TotalSeconds > refreshRate)
            {

                HandleInput(ref isRunning);
                lastRefreshTime = Stopwatch.GetTimestamp();
                Console.SetCursorPosition(0, 0);
                PrintChunk(_chunksize, _playerchunk);

                foreach (var enemy in enemies)
                {
                    enemy.Render();
                }

                Console.SetCursorPosition(0, _chunksize + 1);
                Console.WriteLine("Use WASD to move, press Esc to return.");



                foreach (var enemy in enemies)
                {
                    enemy.Move();
                    enemy.UpdateChunk(Chunk[_playerchunk], _playerchunk);
                }

                foreach (var enemy in enemies)
                {
                    if (_playerchunk == enemy.chunkkey)
                    {
                        if (_playerX == enemy.X && _playerY == enemy.Y)
                        {
                            return new GameOverScene();
                        }
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
                    MovePlayer(0, -1, _chunksize);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    MovePlayer(0, 1, _chunksize);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    MovePlayer(-1, 0, _chunksize);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    MovePlayer(1, 0, _chunksize);
                    break;
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
            }
        }
    }

    private void MovePlayer(int dx, int dy, int chunksize)
    {
        int newX = _playerX + dx;
        int newY = _playerY + dy;
        int oldchunk = _playerchunk;
        GenerateChunk(_chunksize, _playerchunk + 1);
        GenerateChunk(_chunksize, _playerchunk - 1);
        GenerateChunk(_chunksize, _playerchunk - 10);
        GenerateChunk(_chunksize, _playerchunk + 10);
        int asdf = chunksize - 1;
        if (newY > asdf)
        {
            _playerchunk -= 1;
            newY = 0;
        }
        if (newY < 0)
        {
            _playerchunk += 1;
            newY = asdf;
        }
        if (newX > asdf)
        {
            _playerchunk += 10;
            newX = 0;
        }
        if (newX < 0)
        {
            _playerchunk -= 10;
            newX = asdf;
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