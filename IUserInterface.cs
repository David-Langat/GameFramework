
public interface IUserInterface
{
    void ClearScreen();
    void DisplayWelcomeMessage();
    void DecideGameToPlay();
    void DecidePlayerMode();
    void InvalidInput();
    void RequestPlayerNameInput();
    string GetPlayerName();
    void ShowPlayerTurn(Player player);
    void SOSHelpGuide();
    void ConnectFourHelpGuide();
    int GetIntUserInput();
    void DisplayBoard(string board);
    void ShowWinner(string winner);
    (int, int, string) GetPlayerMove(Player player);
    string GetSaveFileName();
    string GetLoadFileName();
    void DisplayMessage(string message);
    char GetMenuChoice();
}
