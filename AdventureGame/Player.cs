using System.Drawing;
using FastConsole.Engine.Elements;

namespace AdventureGame;
public class Player : Element
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }

    public bool IsAlive => Health > 0;

    private Canvas _canvas;
    private Map _map;

    public Player(int health, int damage, Map map)
    {
        _map = map;
        Health = health;
        MaxHealth = health;
        Damage = damage;

        _canvas = new Canvas(new Size(1, 1))
        {
            CellWidth = 2,
        };
        _canvas.Fill(Color.Blue, Color.AliceBlue, '@');
    }

    public void Move(Point delta)
    {
        Point newPosition = new Point(Position.X + delta.X, Position.Y + delta.Y);

        if (_map.IsPointInsideMap(newPosition))
        {
            Position = newPosition;
        }
    }

    public override void Update()
    {
        _canvas.Position = new Point(Position.X * 2, Position.Y);
        _canvas.Update();
    }

    protected override void OnRender()
    {
        _canvas.Render();
    }
}