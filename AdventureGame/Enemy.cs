using System.Diagnostics;

namespace AdventureGame;

public class Enemy
{
    public int X { get; set; }
    public int Y { get; set; }
    private char representation;
    private long lastMoveTime;
    private double moveInterval = 1.0 / 2.0;
    private char[,] Chunk;
    public int chunkkey;
    private int _newchunkkey;
    public Enemy(int x, int y, char representation, char[,] chunk,int chunkkey)
    {
        this.chunkkey = chunkkey;
        X = x;
        Y = y;
        this.representation = representation;
        lastMoveTime = Stopwatch.GetTimestamp();
        Chunk = chunk;
    }
    public void Render()
    {
        if(chunkkey == _newchunkkey) 
        {
            Console.SetCursorPosition(X * 2, Y);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(representation);
            Console.ResetColor();
        }
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
            if (Chunk[newY, newX] != '■')
            {
                X = newX;
                Y = newY;
            }
        }
    }
    public void UpdateChunk(char[,] newChunk,int newchunkkey)
    {
        Chunk = newChunk;
        _newchunkkey = newchunkkey;
    }
}
