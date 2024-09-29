using System.Drawing;
using FastConsole.Engine.Elements;

namespace AdventureGame;
public class Enemy : Element
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }

    public bool IsAlive => Health > 0;

    private Canvas _canvas;
    private Map _map;

    public Enemy(int health, int damage, Map map)
    {
        _map = map;
        Health = health;
        MaxHealth = health;
        Damage = damage;

        _canvas = new Canvas(new Size(1, 1))
        {
            CellWidth = 2,
        };
        _canvas.Fill(Color.Red,Color.White,'@');
    }

    public void Move(Point delta)
    {
        Point newPosition = new Point(Position.X + delta.X, Position.Y + delta.Y);
        if (_map.IsPointInsideMap(newPosition))
        {
            Position = newPosition;
        }
        else
        {
            
        }
    }

    public override void Update()
    {
        Thread.Sleep(1000);
        int rnd = Random.Shared.Next(4);
        Point delta = new Point(0, 0);
        switch (rnd)
        {
            case 0:
                delta = new Point(0, -1);
                break;
            case 1:
                delta = new Point(-1,0);
                break;
            case 2:
                delta = new Point(0, 1);
                break;
            case 3:
                delta = new Point(1,0);
                break;
        }
        Point newPosition = new Point(Position.X + delta.X, Position.Y + delta.Y);

        if (_map.IsPointInsideMap(newPosition))
        {
            Position = newPosition;
        }
        _canvas.Position = new Point(Position.X * 2, Position.Y);
        _canvas.Update();
    }

    protected override void OnRender()
    {
        _canvas.Render();
    }
}