using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;


namespace AdventureGame.Elements;



public class HealthBar : Element
{
    public IEntity Entity { get; set; }

    private Canvas _canvas;
    private Text _text;
    private FlexBox _flexBox;

    public HealthBar(IEntity entity)
    {
        Entity = entity;
        _canvas = new Canvas(new Size())
        {
            CellWidth = 1
        };
        _text = new Text();
        _flexBox = new FlexBox()
        {
            GrowDirection = GrowDirection.Horizontal,
            Spacing = 1,
            AlwaysRecalculate = true
        };

        _flexBox.Children.Add(_canvas);
        _flexBox.Children.Add(_text);
    }

    public override void Update()
    {
        _flexBox.Size = Size;
        _flexBox.Position = Position;

        _text.Value = $"{Entity.Health}/{Entity.MaxHealth}";
        _text.Size = _text.FitSize;

        _canvas.CanvasSize = new Size(Size.Width - _text.Size.Width - _flexBox.Spacing, 1);

        _canvas.Fill(Color.FromArgb(57, 20, 19));
        double percent = Entity.Health / (double)Entity.MaxHealth;
        _canvas.FillRect(0, 0, (int)(_canvas.CanvasSize.Width * percent), 1, Color.FromArgb(196, 58, 50));

        _flexBox.Update();
    }

    protected override void OnRender()
    {
        _flexBox.Render();
    }
}