using AdventureGame.Enemies;
using AdventureGame;
using AdvntureGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventureGame.Enemies;

public class Zombie : EnemyBase
{
    public Zombie()
    {
        Health = 100;
        MaxHealth = 100;
        Damage = 25;

        _name.Value = "Zombie";
    }


    public override Decision MakeTurn(FightingArea area)
    {
        return new Decision("Attack", () =>
        {

        });
    }
}