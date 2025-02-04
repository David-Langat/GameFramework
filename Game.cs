using System.Data;
using System.Diagnostics;
using System.Runtime;

public abstract class Game
{
    //declare abstract methods to be implemented by children
    //We template method to achieve this

    //declare variables
    private int numberOfPlayers;
    
    protected abstract void InitializeNewGame(string playerMode , out Player player1, out Player player2);
    protected abstract void PlayMove(int player, Player player1, Player player2);
    
    protected abstract bool EndOfGame();
    protected abstract void ShowWinner(Player player1, Player player2);
    
    //template method to be used by all our children classes
    public void PlayGame(int numberOfPlayers, string playerMode)
    {
        this.numberOfPlayers = numberOfPlayers;
        InitializeNewGame(playerMode, out Player player1, out Player player2);
        int a = 0;
        while(!EndOfGame())
        {
            PlayMove(a,player1,player2);
            
            a = (a + 1) % numberOfPlayers; 
        }
        ShowWinner(player1,player2);

    }
}

public class SOSGame : Game
{
    //data field
    SOSBoard sosBoard = new SOSBoard(3,3);
    private int point = 0;
   
    private bool horizontal = true, vertical = true , leftToRight = true , rightToLeft = true;

    
    protected override void InitializeNewGame(string playerMode, out Player player1, out  Player player2)
    {


        //create appropriate player objects
        // atleast 1 human player is choosen and name is inputted.
         player1 = new HumanPlayer();
         UserInterface.RequestPlayerNameInput();
         player1.PlayerName = Console.ReadLine();


        //select second player according to playerMode
        if(playerMode == "HVH")
        {

         player2 = new HumanPlayer();
         UserInterface.RequestPlayerNameInput();
         player2.PlayerName = Console.ReadLine();

        }else
        {
         player2 = new ComputerPlayer();
        }

    



    }

    protected override void PlayMove(int player, Player player1,Player player2)
    {
        //declare local variables
        int row,col;
        
        

        // Determine the current player based on the player argument
        Player currentPlayer;

        if (player == 0)
        {
            currentPlayer = player1;
        }
        else
        {
            currentPlayer = player2;
        }

        /*test if we can know its computer player
        if(player2.GetType() == typeof(ComputerPlayer))
        {
            Console.WriteLine("PLAYER");
        }*/

        //display previous board
        UserInterface.SOSHelpGuide();
        sosBoard.Display();
        
        
        //ask current player for  row input
        Console.WriteLine($"{currentPlayer.PlayerName}, it's your turn.");
        Console.Write("Enter row (0, 1, or 2): ");
        string rowInput = Console.ReadLine();

        //check if row input is integer 
        while (!int.TryParse(rowInput,out row) || (row < 0 || row > 2))
        {
            Console.WriteLine("Invalid row selection! Try again.");
            Console.Write("Enter row (0, 1, or 2): ");
            rowInput = Console.ReadLine();
    
        }

        // ask current player for column input
        Console.Write("Enter column (0, 1, or 2): ");
        string colInput = Console.ReadLine();

        //check if col input is integer and within range
        while (!int.TryParse(colInput,out col)||(col < 0 || col > 2 ))
        {
            Console.WriteLine("Invalid column selection! Try again.");
            Console.Write("Enter column (0, 1, or 2): ");
            colInput = Console.ReadLine();
    
        }

    
        //check if selected cell is occupied
        while(sosBoard.Board[row,col].RetrievePiece() != " ")
        {
            Console.WriteLine("Cell is occupied");
            Console.Write("Enter row (0, 1, or 2): ");
            rowInput = Console.ReadLine();
              //check if row input is integer 
            while (!int.TryParse(rowInput,out row) || (row < 0 || row > 2))
            {
                Console.WriteLine("Invalid row selection! Try again.");
                Console.Write("Enter row (0, 1, or 2): ");
                rowInput = Console.ReadLine();
    
            }

            Console.Write("Enter column (0, 1, or 2): ");
            colInput = Console.ReadLine();
             //check if col input is integer and within range
            while (!int.TryParse(colInput,out col)||(col < 0 || col > 2 ))
            {
                Console.WriteLine("Invalid column selection! Try again.");
                Console.Write("Enter column (0, 1, or 2): ");
                colInput = Console.ReadLine();
    
            }
        }
 

        

        //ask for piece to play
        Console.Write("Enter 'S' or 'O': ");
        string piece = Console.ReadLine().ToUpper(); // Convert to uppercase for case-insensitive check
        
        //check if piece is valid for the game
        while  (piece != "S" && piece != "O")
        {
            Console.WriteLine("Invalid symbol! Enter 'S' or 'O'.");
            piece = Console.ReadLine().ToUpper();
        }

        
    
        sosBoard.PlacePiece(row, col, piece);
        //take current player point and add it previous currentplayer point

        currentPlayer.PlayerPoint += AddPoint(); 
        

        sosBoard.Display();
        Console.WriteLine($"{currentPlayer.PlayerName} points are {currentPlayer.PlayerPoint}");
        
    }
    protected override bool EndOfGame()
    {
        for(int row=0;row<sosBoard.Rows;row++)
        {
            for(int col=0;col<sosBoard.Cols;col++)
            {
              //check if cell is empty return false so that game continues
              if (sosBoard.Board[row,col].RetrievePiece() == " ")
              {
                return false;
              }
            }
        }
        
        return true;
    }
    
    protected override void ShowWinner(Player player1 , Player player2)
    {
       if (player1.PlayerPoint>player2.PlayerPoint)
       {
        Console.WriteLine($"{player1.PlayerName} are the winner!");
       }
        else if (player2.PlayerPoint>player1.PlayerPoint)
       {
        Console.WriteLine($"{player2.PlayerName} are the winner!");
       }
        else if (player1.PlayerPoint==player2.PlayerPoint)
        {
            Console.WriteLine("The game is draw!!!");
        }
    }
    
    //method add point if SO connect
    public int AddPoint()
    {
        
        // Check for horizontal SOS sequences
        if(horizontal)
        {
            for (int row = 0; row < sosBoard.Rows; row++)
            {
                for (int col = 0; col < sosBoard.Cols - 2; col++)
                {
                    if (sosBoard.Board[row, col].RetrievePiece() == "S" && sosBoard.Board[row, col + 1].RetrievePiece() == "O" && sosBoard.Board[row, col + 2].RetrievePiece() == "S")
                    {
                        horizontal = false;
                        return 1; // Horizontal SOS sequence found
                        
                    }
                }
            }
        }


        // Check for vertical SOS sequences
        if(vertical)
        {
            for (int row = 0; row < sosBoard.Rows - 2; row++)
            {
                for (int col = 0; col < sosBoard.Cols; col++)
                {
                    if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                        sosBoard.Board[row + 1, col].RetrievePiece() == "O" &&
                        sosBoard.Board[row + 2, col].RetrievePiece() == "S")
                    {
                        vertical = false;
                        return 1; // Vertical SOS sequence found
                    }
                }
            }
        }

        // Check for diagonal SOS sequences (top-left to bottom-right)
        if(leftToRight)
        {
            for (int row = 0; row < sosBoard.Rows - 2; row++)
            {
                for (int col = 0; col < sosBoard.Cols - 2; col++)
                {
                    if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                        sosBoard.Board[row + 1, col + 1].RetrievePiece()== "O" &&
                        sosBoard.Board[row + 2, col + 2].RetrievePiece() == "S")
                    {
                        leftToRight = false;
                        return 1; // Diagonal SOS sequence found
                    }
                }
            }
        }

        // Check for diagonal SOS sequences (bottom-left to top-right)
        if(rightToLeft)
        {
            for (int row = 2; row < sosBoard.Rows; row++)
            {
                for (int col = 0; col < sosBoard.Cols - 2; col++)
                {
                    if (sosBoard.Board[row, col].RetrievePiece() == "S" &&
                        sosBoard.Board[row - 1, col + 1].RetrievePiece() == "O" &&
                        sosBoard.Board[row - 2, col + 2].RetrievePiece() == "S")
                    {
                        rightToLeft = false;
                        return 1; // Diagonal SOS sequence found
                    }
                }
            }
        }

        

        return 0;
    }
}

public class ConnectFour : Game
{
    //We choose to initiate connectfour with 7*6
    ConnectFourBoard connectFourBoard = new ConnectFourBoard(7,6); 

    
    protected override void InitializeNewGame(string playerMode, out Player player1, out Player player2)
    {
        //create appropriate player objects
        // atleast 1 human player is choosen and name is inputted.
         player1 = new HumanPlayer();
         UserInterface.RequestPlayerNameInput();
         player1.PlayerName = Console.ReadLine();

         player1.PlayerSymbol = "X";


        //select second player according to playerMode
        if(playerMode == "HVH")
        {

         player2 = new HumanPlayer();
         UserInterface.RequestPlayerNameInput();
         player2.PlayerName = Console.ReadLine();

        }else
        {
         player2 = new ComputerPlayer();
        }
        player2.PlayerSymbol = "O";
    }
    protected override void PlayMove(int player, Player player1, Player player2)
    {
        //declare variable
        int col;
        // Determine the current player based on the player argument
        Player currentPlayer;

        if (player == 0)
        {
            currentPlayer = player1;
        }
        else
        {
            currentPlayer = player2;
        }
        
        UserInterface.ConnectFourHelpGuide();
        connectFourBoard.Display();
        
        //ask current player for input
        Console.WriteLine($"{currentPlayer.PlayerName}, it's your turn.");
        //Console.Write("Enter row (0, 1, or 2): ");
        int row = connectFourBoard.Rows;
        

        Console.Write("Enter column (0, 1,2,3,4, or 5): ");
        string colInput = Console.ReadLine();

        //check if col input is integer 
        while (!int.TryParse(colInput,out col)||(col < 0 || col > 5 ))
        {
            Console.WriteLine("Invalid column selection! Try again.");
            Console.Write("Enter column (0, 1,2,3,4, or 5): ");
            colInput = Console.ReadLine();
    
        }
        connectFourBoard.PlacePiece(col,currentPlayer.PlayerSymbol);
        connectFourBoard.Display();


        
    }
    protected override bool EndOfGame()
    {
         // Check if any player has won
        if (connectFourBoard.CheckWinningCondition("X"))
        {
            Console.WriteLine("Player 1 wins!");
            return true;
        }
        if (connectFourBoard.CheckWinningCondition("O"))
        {
            Console.WriteLine("Player 2 wins!");
            return true;
        }
        for(int row=0;row<connectFourBoard.Rows;row++)
        {
            for(int col=0;col<connectFourBoard.Cols;col++)
            {
              //check if cell is empty return false so that game continues
              if (connectFourBoard.Board[row,col].RetrievePiece() == " ")
              {
                return false;
              }
            }
        }

       
        return true;
    }
    protected override void ShowWinner(Player player1, Player player2)
    {
            // Check for a winner by inspecting the board
        if (connectFourBoard.CheckWinningCondition(player1.PlayerSymbol))
        {
            Console.WriteLine($"Congratulations {player1.PlayerName}! You are the winner with the symbol '{player1.PlayerSymbol}'.");
        }
        else if (connectFourBoard.CheckWinningCondition(player2.PlayerSymbol))
        {
            Console.WriteLine($"Congratulations {player2.PlayerName}! You are the winner with the symbol '{player2.PlayerSymbol}'.");
        }
        else
        {
            Console.WriteLine("It's a draw! No winner this time.");
        }

        // Optionally display final board state
        connectFourBoard.Display();

    }

    protected bool WinningCondition()
    {
        //check if four pieces have been connected
        return false;
    }
}