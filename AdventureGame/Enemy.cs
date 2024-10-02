using System.Drawing;
using FastConsole.Engine.Core;
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
    private double _lastMoveTime;
    private int _chunkkey;
    public Enemy(int health, int damage, Map map,int chunkkey)
    {
        _map = map;
        Health = health;
        MaxHealth = health;
        Damage = damage;
        _chunkkey = chunkkey;
        _canvas = new Canvas(new Size(1, 1))
        {
            CellWidth = 2,
        };
        _canvas.Fill(Color.Red,Color.White,' ');
    }

   

    public override void Update()
    {
        const double MoveDelay = 0.5;
        if (Time.NowSeconds - _lastMoveTime >= MoveDelay)
        {
            _lastMoveTime = Time.NowSeconds;
            int rnd = Random.Shared.Next(4);
            Point delta = new Point(0, 0);
            switch (rnd)
            {
                case 0:
                    delta = new Point(0, -1);
                    break;
                case 1:
                    delta = new Point(-1, 0);
                    break;
                case 2:
                    delta = new Point(0, 1);
                    break;
                case 3:
                    delta = new Point(1, 0);
                    break;
            }
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

            }
            if (IsOneRefrenceTrue)
            {
                Position = newPosition;
            }
        }
        
        _canvas.Position = new Point(Position.X * 2, Position.Y);
        _canvas.Update();
    }

    protected override void OnRender()
    {
        _canvas.Render();
    }
}