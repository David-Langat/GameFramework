
// MoveCommand.cs
public class MoveCommand : ICommand
{
    private readonly Game _game;
    private readonly int _player;
    private readonly Player _player1;
    private readonly Player _player2;
    private GameState _beforeState;

    public MoveCommand(Game game, int player, Player player1, Player player2)
    {
        _game = game;
        _player = player;
        _player1 = player1;
        _player2 = player2;
    }

    public void Execute()
    {
        _beforeState = _game.CreateMemento();
        _game.PlayMove(_player, _player1, _player2);
    }

    public void Unexecute()
    {
        _game.RestoreMemento(_beforeState);
    }
}
