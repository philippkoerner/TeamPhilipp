using System.Drawing;
using FastConsole.Engine.Elements;

namespace AdventureGame;

public class Map : Element
{
    public Size MapSize => _canvas.CanvasSize;

    private Canvas _canvas;

    public Map()
    {
        _canvas = new Canvas(new Size(20, 20))
        {
            CellWidth = 2
        };
        _canvas.Fill(Color.Green);
        
    }

    public bool IsPointInsideMap(Point point)
    {
        if (point.X < 0 || point.Y < 0)
            return false;

        if (point.X >= MapSize.Width || point.Y >= MapSize.Height)
            return false;

        return true;
    }

    public override void Update()
    {
        _canvas.Position = Position;
        _canvas.Update();
    }

    protected override void OnRender()
    {
        _canvas.Render();
    }
}