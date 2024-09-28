using System.Diagnostics;
using AdventureGame;

class Program
{
    public static void Main()
    {
        SceneManager.OpenScene(new MenuScene());
        SceneManager.Run();
    }
}
