using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;
namespace AdventureGame;
public class GameOverScene : Scene
{
    public GameOverScene()
    {
        Elements.Add(new Text()
        {
            Size = new Size(50, 12),
            Value = "Adventure game \nVersion: 0.1 \n \nShowcase for GoIteens \n \nPress E to go back...",
            Alignment = Alignment.Center
        });
    }
    public override void Update()
    {

    }
}