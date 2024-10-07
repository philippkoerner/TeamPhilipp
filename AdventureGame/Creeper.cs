using System.Drawing;
using FastConsole.Engine.Elements;
using AdventureGame.Enemies;
using AdventureGame;
using AdvntureGame;
using System.Xml.Linq;

namespace AdventureGame.Enemies;

public class Creeper : EnemyBase, IFightTurnEndListener
{
    public int TimeToExplosion { get; set; } = 4;

    private Text _timeToExplodeText;

    public Creeper() : base()
    {
        Health = 200;
        MaxHealth = 200;
        Damage = 350;

        _name.Value = "Creeper";
        _timeToExplodeText = new Text();

        _flexBox.Children.Add(_timeToExplodeText);
    }

    public override void ReceiveDamage(int amount)
    {
        if (Random.Shared.NextDouble() > 0.90)
        {
            ResetExplosionTimer();
        }

        base.ReceiveDamage(amount);
    }

    public void ResetExplosionTimer()
    {
        TimeToExplosion = 4;
    }

    public override void Update()
    {
        _timeToExplodeText.Value = $"Time to explode: {TimeToExplosion}";

        _timeToExplodeText.Size = new Size(Size.Width - 2, 1);
        base.Update();
    }

    public override Decision MakeTurn(FightingArea fightingArea)
    {
        if (TimeToExplosion <= 0)
        {
            return new Decision("Explode", () =>
            {
                Kill();

                fightingArea.Player.ReceiveDamage(Damage);
                foreach (EnemyBase enemy in fightingArea.Enemies.Where(enemy => enemy != this && enemy.IsAlive))
                {
                    enemy.ReceiveDamage(Damage);
                }
            });
        }

        return new Decision("Skip", () => { });
    }

    public void OnTurnEnded()
    {
        if (TimeToExplosion > 0)
        {
            TimeToExplosion--;
        }
    }
}