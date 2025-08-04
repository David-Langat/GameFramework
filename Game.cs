using System.Data;
using System.Diagnostics;
using System.Runtime;

public abstract class Game
{
    protected IUserInterface ui;
    private int numberOfPlayers;
    protected Player player1;
    protected Player player2;
    protected int currentPlayerIndex;
    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    public Game(IUserInterface ui)
    {
        this.ui = ui;
    }

    protected abstract void InitializeNewGame(string playerMode, out Player player1, out Player player2);
    public abstract bool PlayMove(int player, Player player1, Player player2);
    protected abstract bool EndOfGame();
    protected abstract void ShowWinner(Player player1, Player player2);
    public abstract GameState CreateMemento();
    public abstract void RestoreMemento(GameState memento);

    public void PlayGame(int numberOfPlayers, string playerMode)
    {
        this.numberOfPlayers = numberOfPlayers;
        InitializeNewGame(playerMode, out player1, out player2);
        currentPlayerIndex = 0;
        while (!EndOfGame())
        {
            char choice = ui.GetMenuChoice();
            switch (choice)
            {
                case 'm':
                    ICommand move = new MoveCommand(this, currentPlayerIndex, player1, player2);
                    move.Execute();
                    undoStack.Push(move);
                    redoStack.Clear();
                    if (!PlayMove(currentPlayerIndex, player1, player2))
                    {
                        currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
                    }
                    break;
                case 'u':
                    if (undoStack.Count > 0)
                    {
                        ICommand lastMove = undoStack.Pop();
                        lastMove.Unexecute();
                        redoStack.Push(lastMove);
                    }
                    else
                    {
                        ui.DisplayMessage("No moves to undo.");
                    }
                    break;
                case 'r':
                    if (redoStack.Count > 0)
                    {
                        ICommand nextMove = redoStack.Pop();
                        nextMove.Execute();
                        undoStack.Push(nextMove);
                    }
                    else
                    {
                        ui.DisplayMessage("No moves to redo.");
                    }
                    break;
                case 's':
                    string saveFileName = ui.GetSaveFileName();
                    GameState memento = CreateMemento();
                    new GameCaretaker().SaveGame(memento, saveFileName);
                    ui.DisplayMessage("Game saved successfully!");
                    break;
                case 'l':
                    string loadFileName = ui.GetLoadFileName();
                    try
                    {
                        GameState loadedMemento = new GameCaretaker().LoadGame(loadFileName);
                        RestoreMemento(loadedMemento);
                        ui.DisplayMessage("Game loaded successfully!");
                    }
                    catch (Exception ex)
                    {
                        ui.DisplayMessage($"Error loading game: {ex.Message}");
                    }
                    break;
                case 'q':
                    return;
            }
        }
        ShowWinner(player1, player2);
    }
}

public class SOSGame : Game
{
    SOSBoard sosBoard = new SOSBoard(3, 3);

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
        this.player1 = player1;
        this.player2 = player2;
    }

    public override bool PlayMove(int player, Player player1, Player player2)
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
            ui.DisplayBoard(sosBoard.GetBoardAsString());
            ui.ShowPlayerTurn(currentPlayer);
            return true;
        }


        ui.DisplayBoard(sosBoard.GetBoardAsString());
        ui.ShowPlayerTurn(currentPlayer);
        return false;
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

    public override GameState CreateMemento()
    {
        string[,] boardState = new string[sosBoard.Rows, sosBoard.Cols];
        for (int i = 0; i < sosBoard.Rows; i++)
        {
            for (int j = 0; j < sosBoard.Cols; j++)
            {
                boardState[i, j] = sosBoard.Board[i, j].RetrievePiece();
            }
        }

        return new GameState
        {
            GameType = "SOS",
            BoardState = boardState,
            CurrentPlayerIndex = this.currentPlayerIndex,
            Player1Name = this.player1.PlayerName,
            Player2Name = this.player2.PlayerName,
            Player1Score = this.player1.PlayerPoint,
            Player2Score = this.player2.PlayerPoint
        };
    }

    public override void RestoreMemento(GameState memento)
    {
        for (int i = 0; i < sosBoard.Rows; i++)
        {
            for (int j = 0; j < sosBoard.Cols; j++)
            {
                sosBoard.Board[i, j].PlacePiece(memento.BoardState[i, j]);
            }
        }

        this.currentPlayerIndex = memento.CurrentPlayerIndex;
        this.player1.PlayerName = memento.Player1Name;
        this.player2.PlayerName = memento.Player2Name;
        this.player1.PlayerPoint = memento.Player1Score;
        this.player2.PlayerPoint = memento.Player2Score;
    }
}

public class ConnectFour : Game
{
    ConnectFourBoard connectFourBoard = new ConnectFourBoard(6, 7);

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
        this.player1 = player1;
        this.player2 = player2;
    }

    public override bool PlayMove(int player, Player player1, Player player2)
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
        return false;
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

    public override GameState CreateMemento()
    {
        string[,] boardState = new string[connectFourBoard.Rows, connectFourBoard.Cols];
        for (int i = 0; i < connectFourBoard.Rows; i++)
        {
            for (int j = 0; j < connectFourBoard.Cols; j++)
            {
                boardState[i, j] = connectFourBoard.Board[i, j].RetrievePiece();
            }
        }

        return new GameState
        {
            GameType = "ConnectFour",
            BoardState = boardState,
            CurrentPlayerIndex = this.currentPlayerIndex,
            Player1Name = this.player1.PlayerName,
            Player2Name = this.player2.PlayerName,
            Player1Symbol = this.player1.PlayerSymbol,
            Player2Symbol = this.player2.PlayerSymbol
        };
    }

    public override void RestoreMemento(GameState memento)
    {
        for (int i = 0; i < connectFourBoard.Rows; i++)
        {
            for (int j = 0; j < connectFourBoard.Cols; j++)
            {
                connectFourBoard.Board[i, j].PlacePiece(memento.BoardState[i, j]);
            }
        }

        this.currentPlayerIndex = memento.CurrentPlayerIndex;
        this.player1.PlayerName = memento.Player1Name;
        this.player2.PlayerName = memento.Player2Name;
        this.player1.PlayerSymbol = memento.Player1Symbol;
        this.player2.PlayerSymbol = memento.Player2Symbol;
    }
}