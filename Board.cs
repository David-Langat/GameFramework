
// Component interface
public interface IBoard
{
    // The following methods will be used by all child class
    void Display();
    string GetBoardAsString();
}

// Leaf class (represents individual cells in the "SOS" an "Connectfour" game board)
public class Cell : IBoard
{
    //declare local variables
    private int row;
    private int col;
    private string piece;

    //constructor
    public Cell(int row, int col)
    {
        this.row = row;
        this.col = col;
        piece = " "; // Initialize as empty
    }

  

    // methods
    public string RetrievePiece()
    {
        return piece;
    }

    // method to input piece depending on the game being played
    public void PlacePiece(string pieceInput)
    {
        //Check if cell is already occupied, if so return Error message
        if (piece == " ")
        {
            piece = pieceInput;
        }
        else
        {
            Console.WriteLine($"INVALID MOVE!!!Cell [{row}, {col}] is already occupied.");
        }
    }

   //Overrides parent interface methods with its own implementation
    public void Display()
    {
        Console.Write($"[{piece}]\t");
        
    }

    public string GetBoardAsString()
    {
        return $"[{piece}]";
    }
}

// Composite class (represents the "SOS" game board containing cells)
public class SOSBoard : IBoard
{
    //declare local variables
    private Cell[,] board;
    private int rows;
    private int cols;

    //property

    public Cell [,] Board
    {
        get { return board;}
    }
    public int Rows
    {
        get { return rows; }
        set { rows = value; }
    }
    public int Cols
    {
        get { return cols; }
        set { cols = value; }
    }
    //constructor
    public SOSBoard(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        board = new Cell[rows, cols];

        // Initialize the board with empty cells
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j] = new Cell(i, j);
            }
        }
    }
    
    //override inteface method
    public void Display()
    {
        Console.WriteLine("SOS Game Board:({0}*{1})", rows,cols);
        Console.WriteLine("--------------------------------------------------------");
        for (int i = 0; i < rows; i++)
        {
            
            for (int j = 0; j < cols; j++)
            {
                board[i, j].Display();
            }
            Console.WriteLine();
           
        }
        Console.WriteLine("--------------------------------------------------------\n");
    }

   public string GetBoardAsString()
    {
        string boardString = "SOS Game Board:(" + rows + "*" + cols + ")\n";
        boardString += "--------------------------------------------------------\n";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                boardString += "[" + board[i, j].RetrievePiece() + "]\t";
            }
            boardString += "\n";
        }
        boardString += "--------------------------------------------------------\n";
        return boardString;
    }

   // method that takes row and column and places input in cell
    public void PlacePiece(int row, int col, string pieceInput)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols)
        {
            Console.WriteLine($"Invalid row or column: [{row}, {col}]");
            return;
        }

        board[row, col].PlacePiece(pieceInput);
    }
}

// Composite class (represents the Connect Four game board containing cells)
public class ConnectFourBoard : IBoard
{
    //declare local variables
    private Cell[,] board;
    private int rows;
    private int cols;


    //properties
    public int Rows
    {
        get { return rows;}
    }
    public int Cols
    {
        get { return cols;}
    }
    public Cell [,] Board
    {
        get { return board;}
    }
    
    //constructor
    public ConnectFourBoard(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        board = new Cell[rows, cols];

        // Initialize the board with empty cells
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j] = new Cell(i, j);
            }
        }
    }

    //override interface method
    public void Display()
    {
        Console.WriteLine("ConnectFour Board:({0}*{1})", rows,cols);
        Console.WriteLine("--------------------------------------------------------");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j].Display();
            }
            Console.WriteLine();
        }
        Console.WriteLine("--------------------------------------------------------\n");
    }

    public string GetBoardAsString()
    {
        string boardString = "ConnectFour Board:(" + rows + "*" + cols + ")\n";
        boardString += "--------------------------------------------------------\n";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                boardString += "[" + board[i, j].RetrievePiece() + "]\t";
            }
            boardString += "\n";
        }
        boardString += "--------------------------------------------------------\n";
        return boardString;
    }

    public void PlacePiece(int col, string pieceInput)
    {
        if (col < 0 || col >= cols)
        {
            Console.WriteLine($"Invalid column index: {col}");
            return;
        }

        for (int i = rows - 1; i >= 0; i--)
        {
            if ( board[i, col].RetrievePiece() == " ")
            {
                board[i, col].PlacePiece(pieceInput);
                return;
            }
        }
        Console.WriteLine($"Column {col} is already full.");
    }
    // Check Winning Condition
    public bool CheckWinningCondition(string piece)
    {
        // Check horizontal
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col <= cols - 4; col++)
            {
                if (board[row, col].RetrievePiece() == piece &&
                    board[row, col + 1].RetrievePiece() == piece &&
                    board[row, col + 2].RetrievePiece() == piece &&
                    board[row, col + 3].RetrievePiece() == piece)
                {
                    return true; // Horizontal win
                }
            }
        }

        // Check vertical
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row <= rows - 4; row++)
            {
                if (board[row, col].RetrievePiece() == piece &&
                    board[row + 1, col].RetrievePiece() == piece &&
                    board[row + 2, col].RetrievePiece() == piece &&
                    board[row + 3, col].RetrievePiece() == piece)
                {
                    return true; // Vertical win
                }
            }
        }

        // Check diagonal (left to right)
        for (int row = 0; row <= rows - 4; row++)
        {
            for (int col = 0; col <= cols - 4; col++)
            {
                if (board[row, col].RetrievePiece() == piece &&
                    board[row + 1, col + 1].RetrievePiece() == piece &&
                    board[row + 2, col + 2].RetrievePiece() == piece &&
                    board[row + 3, col + 3].RetrievePiece() == piece)
                {
                    return true; // Diagonal left-to-right win
                }
            }
        }

        // Check diagonal (right to left)
        for (int row = 0; row <= rows - 4; row++)
        {
            for (int col = 3; col < cols; col++)
            {
                if (board[row, col].RetrievePiece() == piece &&
                    board[row + 1, col - 1].RetrievePiece() == piece &&
                    board[row + 2, col - 2].RetrievePiece() == piece &&
                    board[row + 3, col - 3].RetrievePiece() == piece)
                {
                    return true; // Diagonal right-to-left win
                }
            }
        }

        return false;
    }
}