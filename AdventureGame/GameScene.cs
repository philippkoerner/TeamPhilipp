using System.Diagnostics;
using System.Drawing;
using AdventureGame;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;

namespace AdventureGame;

public class GameScene : Scene
{
    public ChunkManager _chunk;
    private Player _player;
    private Enemy _enemy;
    public int _chunkkey = 0;
    private Map map => _chunk.chunks[_chunkkey];
    private Map _map => map;
    public GameScene()
    {
        _chunk = new ChunkManager();
        _chunk.GenerateChunk(_chunkkey);
        _chunk.GenerateChunk(_chunkkey + 1);
        _chunk.GenerateChunk(_chunkkey -1);
        _chunk.GenerateChunk(_chunkkey - 10);
        _chunk.GenerateChunk(_chunkkey + 10);
        _enemy = new Enemy(20, 2, _map,_chunkkey);
        _player = new Player(10, 2, _map,_chunk);
        Elements.Add(_map);
        Elements.Add(_enemy);
        Elements.Add(_player);
    }
    public override void Update()
    {
        
        while (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    _player.Move(new Point(0, -1));
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    _player.Move(new Point(0, 1));
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    _player.Move(new Point(-1, 0));
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    _player.Move(new Point(1, 0));
                    break;
                case ConsoleKey.I:
                    break;
                case ConsoleKey.Escape:
                    OpenScene(new MenuScene());
                    break;
                case ConsoleKey.F:
                    _chunkkey = 1;
                    break;
            }
        }
    }
}