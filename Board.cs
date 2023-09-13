
// Component interface
public interface IBoard
{
    // The following methods will be used by all child class
    void Display();
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
}