using System.Drawing;
using FastConsole.Engine.Elements;

namespace AdventureGame;
public class Player : Element
{
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }

    public bool IsAlive => Health > 0;
    private ChunkManager _chunk;
    private Canvas _canvas;
    private Map _map;
    public Player(int health, int damage,Map map,ChunkManager chunk )
    {
        _chunk = chunk;
        _map = map;
        Health = health;
        MaxHealth = health;
        Damage = damage;

        _canvas = new Canvas(new Size(1, 1))
        {
            CellWidth = 2,
        };
        _canvas.Fill(Color.Blue, Color.AliceBlue, ' ');
    }

    public void Move(Point delta)
    {
        Point newPosition = new Point(Position.X + delta.X, Position.Y + delta.Y);
        Canvas.Image image = _map.GetImage();
        
        bool IsOneRefrenceTrue = false; 
        if (_map.IsPointInsideMap(newPosition))
        {
            IsOneRefrenceTrue = true;
            if (image.Data[newPosition.X, newPosition.Y] != new Canvas.Cell { Background = Color.White, Foreground = Color.White, Value = ' ' })
            {
                IsOneRefrenceTrue = true;
            }
            else
            {
                IsOneRefrenceTrue = false;
            }
        }
        else
        {
            if ( newPosition.Y < 0)
            {
                _chunk._chunkkey -= 1;
            }
            if (newPosition.X < 0)
            {
                _chunk._chunkkey -= 10;
            }
            if ( newPosition.Y >= _map.Size.Height)
            {
                _chunk._chunkkey += 1;
            }
            if (newPosition.X >= _map.Size.Width)
            {
                _chunk._chunkkey += 10;
            }

        }
        if (IsOneRefrenceTrue)
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