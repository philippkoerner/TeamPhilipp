using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;

class MenuButton : Element
{
    public string Name { get; set; }
    public bool IsSelected { get; set; }

    private Action _action;
    private Text _text;

    public MenuButton(string name, Action action)
    {
        Name = name;
        _action = action;

        _text = new Text()
        {
            Alignment = Alignment.Center,
        };
    }

    public void PerformAction()
    {
        _action();
    }

    public override void Update()
    {
        _text.Value = Name;

        if (IsSelected)
        {
            _text.Foreground = Color.Chartreuse;
            _text.Background = Color.Black;
        }
        else
        {
            _text.Foreground = null;
            _text.Background = null;
        }

        _text.Position = Position;
        _text.Size = Size;
        _text.Update();
    }

    protected override void OnRender()
    {
        _text.Render();
    }
}