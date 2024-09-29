using System.Diagnostics;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;



class SceneManager
{
    public static Scene ActiveScene => _loadedScenes.FirstOrDefault();

    private static List<Scene> _loadedScenes = new List<Scene>();
    private static List<Scene> _sceneToAdd = new List<Scene>();
    private static List<Scene> _sceneToClose = new List<Scene>();
    private static bool _exit;

    public static void OpenScene(Scene scene)
    {
        _sceneToAdd.Add(scene);
    }

    public static void CloseScene(Scene scene)
    {
        _sceneToClose.Add(scene);
    }

    public static void Exit()
    {
        _exit = true;
    }

    public static void Run()
    {
        while (true)
        {
            if (Time.TryUpdate())
            {
                
                foreach (Scene scene in _sceneToAdd)
                {
                    _loadedScenes.Insert(0, scene);
                }
                _sceneToAdd.Clear();

                if (ActiveScene == null || _exit)
                    return;

                ActiveScene.Update();
                Element.UpdateAndRender(ActiveScene.Elements);

                foreach (Scene scene in _sceneToClose)
                {
                    _loadedScenes.Remove(scene);
                }
                _sceneToClose.Clear();
            }
        }
    }
}