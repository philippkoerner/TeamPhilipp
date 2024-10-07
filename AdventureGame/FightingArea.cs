using System.Drawing;
using AdventureGame;
using FastConsole.Engine.Core;
using FastConsole.Engine.Elements;
using AdventureGame.Elements;
using AdventureGame.Enemies;

namespace AdvntureGame;

public interface IFightTurnEndListener
{
    void OnTurnEnded();
}

public class FightingArea : Element
{
    public Player Player { get; set; }
    public List<EnemyBase> Enemies { get; set; }
    public bool IsFighting { get; set; }
    public bool IsPlayerWin { get; set; }

    private PlayerAtArena _playerRenderer;
    private FlexBox _enemiesFlexBox;
    private FlexBox _playerFlexBox;
    private FlexBox _arenaFlexBox;
    private double _lastUpdate;

    private int TurnsInCycle => 1 + Enemies.Count;
    private int _participantIndex;

    public FightingArea(Player player)
    {
        Player = player;
        Enemies = new List<EnemyBase>();

        _playerRenderer = new PlayerAtArena(player)
        {
            Size = new Size(16, 4)
        };

        _enemiesFlexBox = new FlexBox()
        {
            GrowDirection = GrowDirection.Horizontal,
            Alignment = Alignment.Center,
            Spacing = 1,
            AlwaysRecalculate = true
        };
        _playerFlexBox = new FlexBox()
        {
            Position = new Point(10,1),
            GrowDirection = GrowDirection.Horizontal,
            Alignment = Alignment.Center,
            Spacing = 1,
            AlwaysRecalculate = true
        };

        _arenaFlexBox = new FlexBox()
        {
            GrowDirection = GrowDirection.Vertical,
            Alignment = Alignment.Center,
            AlwaysRecalculate = true
        };

        _playerFlexBox.Children.Add(_playerRenderer);
        _arenaFlexBox.Children.Add(_enemiesFlexBox);
        _arenaFlexBox.Children.Add(_playerFlexBox);
    }

    public override void Update()
    {
        _playerFlexBox.Size = new Size(Size.Width, 7);
        _enemiesFlexBox.Size = new Size(Size.Width, 7);

        _arenaFlexBox.Spacing = 2;

        _arenaFlexBox.Size = Size;
        _arenaFlexBox.Position = Position;

        _arenaFlexBox.Update();

        if (Time.NowSeconds - _lastUpdate > 1)
        {
            ProcessCycle();
            _lastUpdate = Time.NowSeconds;
        }
    }

    protected override void OnRender()
    {
        _arenaFlexBox.Render();
    }

    public void StartFight()
    {
        IsFighting = true;
        _participantIndex = 0;

        Enemies.Clear();
        int enemiesAmount = 3;//Random.Shared.Next(1, 3);
        for (int i = 0; i < enemiesAmount; i++)
        {
            Enemies.Add(GetRandomEnemy());
        }

        _enemiesFlexBox.Children.Clear();
        foreach (EnemyBase enemy in Enemies)
        {
            _enemiesFlexBox.Children.Add(enemy);
            enemy.Size = new Size(24, 4);
        }

        // TODO: Refactor
        //while (IsFighting)
        //{
        //	ProcessCycle();
        //}
    }

    private EnemyBase GetRandomEnemy()
    {
        return Random.Shared.Next(0, 3) switch
        {
            0 => new Zombie(),
            1 => new Skeleton(),
            2 => new Creeper()
        };
    }

    private void ProcessCycle()
    {
        if (_participantIndex >= TurnsInCycle)
        {
            EndTurn();
        }
        else
        {
            MakeTurn();
            _participantIndex++;
        }
    }

    private void MakeTurn()
    {
        if (_participantIndex == 0)
        {
            Player.MakeTurn(this).PerformAction();
        }
        else
        {
            Enemies[_participantIndex - 1].MakeTurn(this).PerformAction();
        }
    }

    private void EndTurn()
    {
        if (Player is IFightTurnEndListener player)
        {
            player.OnTurnEnded();
        }

        foreach (EnemyBase enemy in Enemies)
        {
            if (enemy is IFightTurnEndListener listener)
            {
                listener.OnTurnEnded();
            }
        }

        _participantIndex = 0;
        TryEndFight();
    }

    private void TryEndFight()
    {
        if (Player.IsAlive == false)
        {
            IsFighting = false;
            IsPlayerWin = false;
        }
        else if (Enemies.All(enemy => enemy.IsAlive == false))
        {
            IsFighting = false;
            IsPlayerWin = true;
        }
    }
}