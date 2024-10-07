using System.Diagnostics;
using System.Drawing;
using AdventureGame;
using AdventureGame.Elements;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;

namespace AdventureGame;
public class GameScene : Scene
{
    private Map _map;
    private Player _player;
    private Enemy _enemy;
    public GameScene()
    {
        _map = new Map();
        _enemy = new Enemy(100,10,_map);
        _player = new Player(100, 10, _map);

        
        Elements.Add(_map);
        Elements.Add(_enemy);
        Elements.Add(_player);
    }
    
    public override void Update()
    {
        if (_player.Position == _enemy.Position)
        {
            OpenScene(new FightingScene(_player));
        }
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
            }
        }
    }
}