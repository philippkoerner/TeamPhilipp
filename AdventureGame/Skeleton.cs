using AdventureGame.Enemies;
using AdventureGame;
using AdvntureGame;
using System.Xml.Linq;

namespace AdventureGame.Enemies;

public class Skeleton : EnemyBase
{
    public Skeleton()
    {
        Health = 50;
        MaxHealth = 50;
        Damage = 30;

        _name.Value = "Skeleton";
    }

    public override void ReceiveDamage(int amount)
    {
        if (Random.Shared.NextDouble() > 0.80)
        {
            return; // Miss
        }

        base.ReceiveDamage(amount);
    }

    public override Decision MakeTurn(FightingArea fightingArea)
    {
        if (Random.Shared.NextDouble() > 0.8)
        {
            return new Decision("Miss", () => { });
        }

        return new Decision("Attack", () =>
        {
            fightingArea.Player.ReceiveDamage(Damage);
        });
    }
}
