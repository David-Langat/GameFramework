public class ConsoleUI : IUserInterface
{
    public void ClearScreen()
    {
        Console.Clear();
    }

    public void DisplayWelcomeMessage()
    {
        Console.WriteLine("Hello there! welcome to our game.");
        Console.WriteLine("This game framework has two game: Connect Four Game, SOS Game");
    }

    public void DecideGameToPlay()
    {
        Console.WriteLine("Enter 1 for SOS Game. Enter 2 for Connect Four Game.");
    }

    public void DecidePlayerMode()
    {
        Console.WriteLine("Enter 1 for Human Vs Human or Enter 2 for Computer VS Human.");
    }

    public void InvalidInput()
    {
        Console.WriteLine("INVALID INPUT!!!! Please enter input as intructed");
    }

    public void RequestPlayerNameInput()
    {
        Console.WriteLine("Please enter player name:");
    }

    public string GetPlayerName()
    {
        return Console.ReadLine() ?? "";
    }

    public void ShowPlayerTurn(Player player)
    {
        Console.WriteLine("{0} please play move ", player.PlayerName);
    }

    public void SOSHelpGuide()
    {
        Console.WriteLine("*******************************");
        Console.WriteLine("This is a typical 3x3 SOS Game;");
        Console.WriteLine("Enter row number and column number will replace the space;");
        Console.WriteLine("Each player can choose S or O to replace;");
        Console.WriteLine("Who can line up SOS first who can get one point;");
        Console.WriteLine("After all place be replaced, caculating the player points and decide who is winner");
        Console.WriteLine("*******************************");
    }

    public void ConnectFourHelpGuide()
    {
        Console.WriteLine("*******************************");
        Console.WriteLine("This is a typical 6x7 ConnectFour Game;");
        Console.WriteLine("Enter column number will replace the space;");
        Console.WriteLine("Player1 will use O and plyer2 will use X to replace the space");
        Console.WriteLine("Who can line up 4 connection who can we the game;");
        Console.WriteLine("*******************************");
    }

    public int GetIntUserInput()
    {
        string userInput;
        int userIntInput;
        userInput = Console.ReadLine() ?? "";
        while (!int.TryParse(userInput, out userIntInput))
        {
            Console.WriteLine("INVALID INPUT!!! Enter a number");
            userInput = Console.ReadLine() ?? "";
        }
        return userIntInput;
    }

    public void DisplayBoard(string board)
    {
        Console.WriteLine(board);
    }

    public void ShowWinner(string winner)
    {
        Console.WriteLine(winner);
    }

    public (int, int, string) GetPlayerMove(Player player)
    {
        int row = 0, col = 0;
        string piece = "";

        Console.WriteLine($"{player.PlayerName}, it's your turn.");

        if (player.PlayerSymbol == null) // SOS Game
        {
            Console.Write("Enter row (0, 1, or 2): ");
            string rowInput = Console.ReadLine();

            while (!int.TryParse(rowInput, out row) || (row < 0 || row > 2))
            {
                Console.WriteLine("Invalid row selection! Try again.");
                Console.Write("Enter row (0, 1, or 2): ");
                rowInput = Console.ReadLine();
            }

            Console.Write("Enter column (0, 1, or 2): ");
            string colInput = Console.ReadLine();

            while (!int.TryParse(colInput, out col) || (col < 0 || col > 2))
            {
                Console.WriteLine("Invalid column selection! Try again.");
                Console.Write("Enter column (0, 1, or 2): ");
                colInput = Console.ReadLine();
            }

            Console.Write("Enter 'S' or 'O': ");
            piece = Console.ReadLine().ToUpper();

            while (piece != "S" && piece != "O")
            {
                Console.WriteLine("Invalid symbol! Enter 'S' or 'O'.");
                piece = Console.ReadLine().ToUpper();
            }
        }
        else // Connect Four
        {
            Console.Write("Enter column (0-6): ");
            string colInput = Console.ReadLine();

            while (!int.TryParse(colInput, out col) || (col < 0 || col > 6))
            {
                Console.WriteLine("Invalid column selection! Try again.");
                Console.Write("Enter column (0-6): ");
                colInput = Console.ReadLine();
            }
        }

        return (row, col, piece);
    }

    public string GetSaveFileName()
    {
        Console.Write("Enter the file name to save the game: ");
        return Console.ReadLine();
    }

    public string GetLoadFileName()
    {
        Console.Write("Enter the file name to load the game: ");
        return Console.ReadLine();
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public char GetMenuChoice()
    {
        Console.WriteLine("\nEnter 'm' to make a move, 'u' to undo, 'r' to redo, 's' to save, 'l' to load, 'q' to quit:");
        return Console.ReadKey().KeyChar;
    }
}