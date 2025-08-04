# GameFramework

An extensible C# framework for creating and playing two-player board games, built with .NET 7. This project serves as a practical demonstration of object-oriented design patterns to promote code reusability, flexibility, and extensibility.

## Implemented Games

The framework currently includes two fully playable games:

*   **SOS:** A game where two players take turns adding either an 'S' or an 'O' to a grid. A player scores a point for creating an 'SOS' sequence (horizontally, vertically, or diagonally) and gets another turn. The player with the most points when the grid is full wins.
*   **Connect Four:** The classic game where two players take turns dropping their colored discs into a 7x6 vertically suspended grid. The first player to form a horizontal, vertical, or diagonal line of four of their own discs wins.

## Core Design Patterns

The architecture of this framework is heavily influenced by several key design patterns, which make it easy to modify existing games or add new ones.

### 1. Template Method Pattern

The core game loop is defined in the abstract `Game` class. The `PlayGame()` method acts as a template method, defining the skeleton of the algorithm for a game's execution.

```csharp
// In Game.cs
public abstract class Game
{
    // ...
    public void PlayGame(int numberOfPlayers, string playerMode)
    {
        InitializeNewGame(playerMode, out Player player1, out Player player2);
        int currentPlayerIndex = 0;
        while (!EndOfGame())
        {
            PlayMove(currentPlayerIndex, player1, player2);
            currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        }
        ShowWinner(player1, player2);
    }

    protected abstract void InitializeNewGame(string playerMode, out Player player1, out Player player2);
    protected abstract void PlayMove(int player, Player player1, Player player2);
    protected abstract bool EndOfGame();
    protected abstract void ShowWinner(Player player1, Player player2);
}
```

Concrete game classes like `SOSGame` and `ConnectFour` provide their own implementations for the abstract steps (`InitializeNewGame`, `PlayMove`, etc.), allowing them to define game-specific logic while reusing the overall game flow structure.

### 2. Strategy Pattern

The framework uses the Strategy Pattern to allow the game type to be selected at runtime. The `Program.cs` file decides which concrete `Game` implementation (`SOSGame` or `ConnectFour`) to instantiate based on user input. This encapsulates the game logic into separate strategy objects.

```csharp
// In Program.cs
if (gameType == "SOS")
{
    Game sosGame = new SOSGame(ui);
    sosGame.PlayGame(playerNumber, playerMode);
}
else if (gameType == "ConnectFour")
{
    Game connectFour = new ConnectFour(ui);
    connectFour.PlayGame(playerNumber, playerMode);
}
```

This makes it easy to add new games without changing the main program's structure. You would simply create a new class that inherits from `Game` and instantiate it.

### 3. Composite Pattern

The game board is structured using the Composite Pattern. The `IBoard` interface defines a common contract for both individual `Cell` objects (Leaf) and the composite `SOSBoard` or `ConnectFourBoard` objects (Composite).

```csharp
// In Board.cs
public interface IBoard
{
    void Display();
    string GetBoardAsString();
}

// Leaf
public class Cell : IBoard { /* ... */ }

// Composite
public class SOSBoard : IBoard
{
    private Cell[,] board;
    // ...
}
```

This pattern allows the client code (like the UI) to treat a single cell and a full game board uniformly. For example, the `Display()` method can be called on the entire board, which then iterates and calls `Display()` on its individual cells.

### 4. Memento Pattern

The Memento pattern is used to implement the save and restore functionality. The `GameState` class acts as the Memento, storing the state of the game. The `Game` class is the Originator, creating and restoring Mementos. The `GameCaretaker` class is the Caretaker, responsible for storing and retrieving the Mementos.

```csharp
// In GameState.cs
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
```

### 5. Command Pattern

The Command pattern is used to implement the undo and redo functionality. The `ICommand` interface defines the contract for executing and unexecuting a command. The `MoveCommand` class is a concrete command that encapsulates a player's move.

```csharp
// In ICommand.cs
public interface ICommand
{
    void Execute();
    void Unexecute();
}

// In MoveCommand.cs
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
```

## Features

*   **Human vs Human and Computer vs Human modes.**
*   **Save and Restore Game:** Save the current game state to a file and restore it later.
*   **Undo and Redo Moves:** Undo and redo moves made by the players.

## How to Run

1.  Ensure you have the .NET 7 SDK installed.
2.  Clone the repository.
3.  Navigate to the project directory in your terminal.
4.  Run the command: `dotnet run`
5.  Follow the on-screen prompts to select a game and player mode.