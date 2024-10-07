using System.Drawing;
using AdventureGame;
using AdvntureGame;
using FastConsole.Engine.Elements;

namespace AdventureGame.Elements;

public class Player : Element , IEntity, IFightTurnEndListener
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public bool HasShield => ShieldLifeTime > 0;

    public bool IsAlive => Health > 0;

    public int ShieldLifeTime;
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
        _canvas.Fill(Color.Blue,Color.AliceBlue,'@');
    }

    public void Heal(int amount)
    {
        Health += amount;
        Health = Math.Min(Health, MaxHealth);
    }

    public void ReceiveDamage(int amount)
    {
        if (HasShield)
        {
            amount = (int)Math.Ceiling(0.35 * amount);
        }

        Health -= amount;
    }

    public void ActivateShield()
    {
        ShieldLifeTime = 3;
    }

    public Decision MakeTurn(FightingArea fightingArea)
    {
        return new Decision("Heal", () =>
        {
            Heal(25);
        });
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

    public void OnTurnEnded()
    {
        if (ShieldLifeTime > 0)
        {
            ShieldLifeTime--;
        }
    }
}