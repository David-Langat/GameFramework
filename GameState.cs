
// GameState.cs
public class GameState
{
    public string GameType { get; set; }
    public string PlayerMode { get; set; }
    public string[,] BoardState { get; set; }
    public int CurrentPlayerIndex { get; set; }
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public string Player1Symbol { get; set; }
    public string Player2Symbol { get; set; }
}
