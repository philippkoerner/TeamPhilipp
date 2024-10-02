using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame;
class SettingsScene : Scene
{
    public SettingsScene() 
    {
        
    }  
    public override void Update()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    OpenScene(new MenuScene());
                    break;
            }
        }
    }
}

