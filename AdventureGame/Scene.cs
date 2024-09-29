using FastConsole.Engine.Elements;
using AdventureGame;
public abstract class Scene
{
    public List<Element> Elements { get; } = new List<Element>();

    public void OpenScene(Scene scene)
    {
        SceneManager.OpenScene(scene);
    }

    public void CloseScene()
    {
        SceneManager.CloseScene(this);
    }

    public abstract void Update();
}