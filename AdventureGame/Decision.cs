namespace AdventureGame;

public class Decision
{
    public string Name { get; set; }
    public Action Action { get; set; }

    public Decision(string name, Action action)
    {
        Name = name;
        Action = action;
    }

    public void PerformAction()
    {
        Action();
    }
}