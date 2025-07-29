
class Program
{
    public static void Main(string[] args)
    {
        IUserInterface ui = new ConsoleUI();
        GameCaretaker caretaker = new GameCaretaker();
        Game game = null;

        ui.DisplayWelcomeMessage();
        ui.DisplayMessage("Enter 1 to start a new game, or 2 to load a saved game:");
        int choice = ui.GetIntUserInput();

        if (choice == 1)
        {
            string playerMode, gameType;
            ui.DecideGameToPlay();
            int gameTypeChoice = ui.GetIntUserInput();
            GameType(gameTypeChoice, out gameType, ui);

            ui.DecidePlayerMode();
            int playerModeChoice = ui.GetIntUserInput();
            PlayerMode(playerModeChoice, out playerMode, ui);

            if (gameType == "SOS")
            {
                game = new SOSGame(ui);
            }
            else if (gameType == "ConnectFour")
            {
                game = new ConnectFour(ui);
            }

            game.PlayGame(2, playerMode);
        }
        else if (choice == 2)
        {
            string fileName = ui.GetLoadFileName();
            try
            {
                GameState memento = caretaker.LoadGame(fileName);
                if (memento.GameType == "SOS")
                {
                    game = new SOSGame(ui);
                }
                else if (memento.GameType == "ConnectFour")
                {
                    game = new ConnectFour(ui);
                }
                game.RestoreMemento(memento);
                game.PlayGame(2, memento.PlayerMode);
            }
            catch (Exception ex)
            {
                ui.DisplayMessage($"Error loading game: {ex.Message}");
                return;
            }
        }

        // After the game loop, ask to save
        ui.DisplayMessage("Do you want to save the game? (y/n)");
        string saveChoice = Console.ReadLine();
        if (saveChoice.ToLower() == "y")
        {
            string fileName = ui.GetSaveFileName();
            GameState memento = game.CreateMemento();
            caretaker.SaveGame(memento, fileName);
            ui.DisplayMessage("Game saved successfully!");
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
