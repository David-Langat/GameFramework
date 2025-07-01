class Program
{
    public static void Main(string[] args)
    {
        IUserInterface ui = new ConsoleUI();
        string playerMode, gameType;
        int userInput;

        ui.DisplayWelcomeMessage();
        ui.DecideGameToPlay();
        userInput = ui.GetIntUserInput();
        GameType(userInput, out gameType, ui);
        Console.WriteLine("{0}", gameType);

        ui.DecidePlayerMode();
        userInput = ui.GetIntUserInput();
        PlayerMode(userInput, out playerMode, ui);
        Console.WriteLine("{0}", playerMode);

        if (gameType == "SOS")
        {
            int playerNumber = 2;
            Game sosGame = new SOSGame(ui);
            sosGame.PlayGame(playerNumber, playerMode);
        }
        else if (gameType == "ConnectFour")
        {
            int playerNumber = 2;
            Game connectFour = new ConnectFour(ui);
            connectFour.PlayGame(playerNumber, playerMode);
        }
    }

    public static bool IsValid(int userInput)
    {
        if (userInput == 1 || userInput == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void PlayerMode(int selectedPlayerMode, out string playerMode, IUserInterface ui)
    {
        while (!IsValid(selectedPlayerMode))
        {
            ui.InvalidInput();
            ui.DecidePlayerMode();
            selectedPlayerMode = ui.GetIntUserInput();
        }

        if (selectedPlayerMode == 1)
        {
            playerMode = "HVH";
        }
        else
        {
            playerMode = "CVH";
        }
    }

    public static void GameType(int selectedGameType, out string gameType, IUserInterface ui)
    {
        while (!IsValid(selectedGameType))
        {
            ui.InvalidInput();
            ui.DecideGameToPlay();
            selectedGameType = ui.GetIntUserInput();
        }

        if (selectedGameType == 1)
        {
            gameType = "SOS";
        }
        else
        {
            gameType = "ConnectFour";
        }
    }
}