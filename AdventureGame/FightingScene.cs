using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;
using AdventureGame;
using AdvntureGame;
using AdventureGame.Elements;

namespace AdventureGame;
public class FightingScene : Scene
{
    private FightingArea _area;
    private Player _player;
    public FightingScene(Player player) 
    {
        
        _player = player;
        _area = new FightingArea(_player); 
        Elements.Add(_area);
    }
    public override void Update()
    {
        
    }
}

