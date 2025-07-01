using System.Data;
using System.Diagnostics;
using System.Runtime;

public abstract class Game
{
    protected IUserInterface ui;
    private int numberOfPlayers;

    public Game(IUserInterface ui)
    {
        this.ui = ui;
    }

    protected abstract void InitializeNewGame(string playerMode, out Player player1, out Player player2);
    protected abstract void PlayMove(int player, Player player1, Player player2);
    protected abstract bool EndOfGame();
    protected abstract void ShowWinner(Player player1, Player player2);

    public void PlayGame(int numberOfPlayers, string playerMode)
    {
        this.numberOfPlayers = numberOfPlayers;
        InitializeNewGame(playerMode, out Player player1, out Player player2);
        int a = 0;
        while (!EndOfGame())
        {
            PlayMove(a, player1, player2);
            a = (a + 1) % numberOfPlayers;
        }
        ShowWinner(player1, player2);
    }
}

public class SOSGame : Game
{
    SOSBoard sosBoard = new SOSBoard(3, 3);
    private bool horizontal = true, vertical = true, leftToRight = true, rightToLeft = true;

    public SOSGame(IUserInterface ui) : base(ui) { }

    protected override void InitializeNewGame(string playerMode, out Player player1, out Player player2)
    {
        player1 = new HumanPlayer();
        ui.RequestPlayerNameInput();
        player1.PlayerName = ui.GetPlayerName();

        if (playerMode == "HVH")
        {
            player2 = new HumanPlayer();
            ui.RequestPlayerNameInput();
            player2.PlayerName = ui.GetPlayerName();
        }
        else
        {
            player2 = new ComputerPlayer();
        }
    }

    protected override void PlayMove(int player, Player player1, Player player2)
    {
        ui.ClearScreen();
        Player currentPlayer = (player == 0) ? player1 : player2;

        ui.SOSHelpGuide();
        ui.DisplayBoard(sosBoard.GetBoardAsString());

        int row, col;
        string piece;

        if (currentPlayer is ComputerPlayer computer)
        {
            do
            {
                computer.SelectValidMove(sosBoard.Rows, sosBoard.Cols);
                row = computer.Row;
                col = computer.Col;
            } while (sosBoard.Board[row, col].RetrievePiece() != " ");
            piece = new Random().Next(0, 2) == 0 ? "S" : "O";
            Console.WriteLine($"Computer places '{piece}' at [{row}, {col}]");
            System.Threading.Thread.Sleep(1000); // Pause for effect
        }
        else
        {
            var move = ui.GetPlayerMove(currentPlayer);
            row = move.Item1;
            col = move.Item2;
            piece = move.Item3;

            while (sosBoard.Board[row, col].RetrievePiece() != " ")
            {
                ui.InvalidInput();
                move = ui.GetPlayerMove(currentPlayer);
                row = move.Item1;
                col = move.Item2;
                piece = move.Item3;
            }
        }

        sosBoard.PlacePiece(row, col, piece);
        int points = AddPoint();
        if (points > 0)
        {
            currentPlayer.PlayerPoint += points;
            Console.WriteLine($"{currentPlayer.PlayerName} scored {points} point(s)!");
            System.Threading.Thread.Sleep(1500);
        }


        ui.DisplayBoard(sosBoard.GetBoardAsString());
        ui.ShowPlayerTurn(currentPlayer);
    }

    protected override bool EndOfGame()
    {
        for (int row = 0; row < sosBoard.Rows; row++)
        {
            for (int col = 0; col < sosBoard.Cols; col++)
            {
                if (sosBoard.Board[row, col].RetrievePiece() == " ")
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected override void ShowWinner(Player player1, Player player2)
    {
        if (player1.PlayerPoint > player2.PlayerPoint)
        {
            ui.ShowWinner($"{player1.PlayerName} are the winner!");
        }
        else if (player2.PlayerPoint > player1.PlayerPoint)
        {
            ui.ShowWinner($"{player2.PlayerName} are the winner!");
        }
        else
        {
            ui.ShowWinner("The game is a draw!!!");
        }
    }

    public int AddPoint()
    {
        int points = 0;

        // Check for horizontal "SOS"
        for (int row = 0; row < sosBoard.Rows; row++)
        {
            for (int col = 0; col < sosBoard.Cols - 2; col++)
            {
                if (sosBoard.Board[row, col].RetrievePiece() == "S" && sosBoard.Board[row, col + 1].RetrievePiece() == "O" && sosBoard.Board[row, col + 2].RetrievePiece() == "S")
                {
                    points++;
                }
            }
        }

        // Check for vertical "SOS"
        for (int row = 0; row < sosBoard.Rows - 2; row++)
        {
            for (int col = 0; col < sosBoard.Cols; col++)
            {
                if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                    sosBoard.Board[row + 1, col].RetrievePiece() == "O" &&
                    sosBoard.Board[row + 2, col].RetrievePiece() == "S")
                {
                    points++;
                }
            }
        }

        // Check for left-to-right diagonal "SOS"
        for (int row = 0; row < sosBoard.Rows - 2; row++)
        {
            for (int col = 0; col < sosBoard.Cols - 2; col++)
            {
                if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                    sosBoard.Board[row + 1, col + 1].RetrievePiece() == "O" &&
                    sosBoard.Board[row + 2, col + 2].RetrievePiece() == "S")
                {
                    points++;
                }
            }
        }

        // Check for right-to-left diagonal "SOS"
        for (int row = 2; row < sosBoard.Rows; row++)
        {
            for (int col = 0; col < sosBoard.Cols - 2; col++)
            {
                if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                    sosBoard.Board[row - 1, col + 1].RetrievePiece() == "O" &&
                    sosBoard.Board[row - 2, col + 2].RetrievePiece() == "S")
                {
                    points++;
                }
            }
        }

        return points;
    }
}

public class ConnectFour : Game
{
    ConnectFourBoard connectFourBoard = new ConnectFourBoard(7, 6);

    public ConnectFour(IUserInterface ui) : base(ui) { }

    protected override void InitializeNewGame(string playerMode, out Player player1, out Player player2)
    {
        player1 = new HumanPlayer();
        ui.RequestPlayerNameInput();
        player1.PlayerName = ui.GetPlayerName();
        player1.PlayerSymbol = "X";

        if (playerMode == "HVH")
        {
            player2 = new HumanPlayer();
            ui.RequestPlayerNameInput();
            player2.PlayerName = ui.GetPlayerName();
        }
        else
        {
            player2 = new ComputerPlayer();
        }
        player2.PlayerSymbol = "O";
    }

    protected override void PlayMove(int player, Player player1, Player player2)
    {
        ui.ClearScreen();
        Player currentPlayer = (player == 0) ? player1 : player2;

        ui.ConnectFourHelpGuide();
        ui.DisplayBoard(connectFourBoard.GetBoardAsString());

        int col;

        if (currentPlayer is ComputerPlayer computer)
        {
            do
            {
                computer.SelectValidMove(connectFourBoard.Rows, connectFourBoard.Cols);
                col = computer.Col;
            } while (connectFourBoard.Board[0, col].RetrievePiece() != " "); // Check if the top of the column is full
            Console.WriteLine($"Computer chooses column {col}");
            System.Threading.Thread.Sleep(1000); // Pause for effect
        }
        else
        {
            var move = ui.GetPlayerMove(currentPlayer);
            col = move.Item2;
        }

        connectFourBoard.PlacePiece(col, currentPlayer.PlayerSymbol);
        ui.DisplayBoard(connectFourBoard.GetBoardAsString());
    }

    protected override bool EndOfGame()
    {
        if (connectFourBoard.CheckWinningCondition("X") || connectFourBoard.CheckWinningCondition("O"))
        {
            return true;
        }

        for (int row = 0; row < connectFourBoard.Rows; row++)
        {
            for (int col = 0; col < connectFourBoard.Cols; col++)
            {
                if (connectFourBoard.Board[row, col].RetrievePiece() == " ")
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override void ShowWinner(Player player1, Player player2)
    {
        if (connectFourBoard.CheckWinningCondition(player1.PlayerSymbol))
        {
            ui.ShowWinner($"Congratulations {player1.PlayerName}! You are the winner with the symbol '{player1.PlayerSymbol}'.");
        }
        else if (connectFourBoard.CheckWinningCondition(player2.PlayerSymbol))
        {
            ui.ShowWinner($"Congratulations {player2.PlayerName}! You are the winner with the symbol '{player2.PlayerSymbol}'.");
        }
        else
        {
            ui.ShowWinner("It's a draw! No winner this time.");
        }

        ui.DisplayBoard(connectFourBoard.GetBoardAsString());
    }
}
