using System.Drawing;
using FastConsole.Engine.Elements;

namespace AdventureGame;

public class AboutScene : Scene
{
    public AboutScene()
    {
        Elements.Add(new Text()
        {
            Size = new Size(50, 12),
            Value = "Adventure game \nVersion: 0.1 \n   \nPress Esc to go back..."
        });
    }

    public override void Update()
    {
        while (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                CloseScene();
            }
        }
    }
}