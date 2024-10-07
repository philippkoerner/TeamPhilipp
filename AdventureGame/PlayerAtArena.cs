using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;
using AdventureGame.Elements;
using AdventureGame;

namespace AdventureGame.Elements;

public class PlayerAtArena : Element
{
    public Player Player { get; set; }

    private Text _name;
    private HealthBar _healthBar;
    private Text _damageText;
    private ShieldBar _shieldBar;
    private FlexBox _flexBox;
    private Box _box;

    public PlayerAtArena(Player player)
    {
        Player = player;

        _name = new Text()
        {
            Alignment = Alignment.Center,
            Value = "Player"
        };
        _healthBar = new HealthBar(player);
        _damageText = new Text();
        _shieldBar = new ShieldBar(player);

        _flexBox = new FlexBox()
        {
            GrowDirection = GrowDirection.Vertical,
            AlwaysRecalculate = true
        };

        _flexBox.Children.Add(_name);
        _flexBox.Children.Add(_healthBar);
        _flexBox.Children.Add(_damageText);
        _flexBox.Children.Add(_shieldBar);

        _box = Box.DefaultBox(new Point(), new Size(), _flexBox);
    }

    public override void Update()
    {
        _name.Size = new Size(Size.Width - 2, 1);
        _healthBar.Size = new Size(Size.Width - 2, 1);
        _damageText.Size = new Size(Size.Width - 2, 1);
        _shieldBar.Size = new Size(Size.Width - 2, 1);

        _damageText.Value = $"Damage: {Player.Damage}";

        if (Player.HasShield)
        {
            _box.Background = Color.Aqua;
        }
        else
        {
            _box.Background = null;
        }

        _box.Size = new Size(Size.Width, _flexBox.Children.Count + 2);
        _box.Position = Position;

        _box.Update();
    }

    protected override void OnRender()
    {
        _box.Render();
    }
}