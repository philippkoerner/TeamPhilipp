using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;

namespace AdventureGame.Elements;


public class ShieldBar : Element
{
    public Player Player { get; set; }

    private Canvas _canvas;
    private Text _noShieldText;

    public ShieldBar(Player player)
    {
        Player = player;
        _canvas = new Canvas(new Size())
        {
            CellWidth = 2
        };
        _noShieldText = new Text()
        {
            Value = "No shield"
        };
    }

    public override void Update()
    {
        if (Player.HasShield)
        {
            _canvas.Position = Position;
            _canvas.CanvasSize = new Size(Size.Width / 2, Size.Height);

            int maxBlocks = 3;
            int spacing = 1;
            int blockWidth = (_canvas.CanvasSize.Width - (maxBlocks - 1) * spacing) / maxBlocks;

            _canvas.Clear();
            for (int i = 0; i < maxBlocks; i++)
            {
                int x = (blockWidth + spacing) * i;

                if (i < Player.ShieldLifeTime)
                {
                    _canvas.FillRect(x, 0, blockWidth, 1, Color.Aqua);
                }
                else
                {
                    _canvas.FillRect(x, 0, blockWidth, 1, Color.Gray);
                }
            }

            _canvas.Update();
        }
        else
        {
            _noShieldText.Position = Position;
            _noShieldText.Size = Size;

            _noShieldText.Update();
        }
    }

    protected override void OnRender()
    {
        if (Player.HasShield)
        {
            _canvas.Render();
        }
        else
        {
            _noShieldText.Render();
        }
    }
}