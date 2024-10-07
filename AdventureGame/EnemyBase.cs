using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;
using AdventureGame.Elements;
using AdventureGame;
using AdvntureGame;

namespace AdventureGame.Enemies;

public abstract class EnemyBase : Element, IEntity
{
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int Damage { get; protected set; }

    public bool IsAlive => Health > 0;

    protected Text _name;
    private HealthBar _healthBar;
    private Text _damageText;
    protected FlexBox _flexBox;
    private Box _box;
    private Text _deadText;

    protected EnemyBase()
    {
        _name = new Text()
        {
            Alignment = Alignment.Center,
        };
        _healthBar = new HealthBar(this);
        _damageText = new Text();

        _flexBox = new FlexBox()
        {
            GrowDirection = GrowDirection.Vertical,
            AlwaysRecalculate = true
        };

        _flexBox.Children.Add(_name);
        _flexBox.Children.Add(_healthBar);
        _flexBox.Children.Add(_damageText);

        _deadText = new Text()
        {
            Value = "Dead",
            Foreground = Color.Red,
            Alignment = Alignment.Center
        };

        _box = Box.DefaultBox(new Point(), new Size(), _flexBox);
    }

    public virtual void Heal(int amount)
    {
        Health += amount;
        Health = Math.Min(Health, MaxHealth);
    }

    public virtual void ReceiveDamage(int amount)
    {
        Health -= amount;
    }

    public void Kill()
    {
        Health = 0;
    }

    public override void Update()
    {
        if (IsAlive)
        {
            _name.Size = new Size(Size.Width - 2, 1);
            _healthBar.Size = new Size(Size.Width - 2, 1);
            _damageText.Size = new Size(Size.Width - 2, 1);

            _damageText.Value = $"Damage: {Damage}";
            _box.Child = _flexBox;
        }
        else
        {
            _box.Child = _deadText;
        }

        _box.Position = Position;
        _box.Size = new Size(Size.Width, _flexBox.Children.Count + 2);

        _box.Update();

        Size = new Size(Size.Width, _box.Size.Height);
    }

    protected override void OnRender()
    {
        _box.Render();
    }

    public abstract Decision MakeTurn(FightingArea fightingArea);
}