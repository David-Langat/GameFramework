using Microsoft.VisualBasic.FileIO;

public abstract class Player
{
//data field 
protected string playerName, playerSymbol;
protected int[] playerMoveHistory;
protected int playerPoint;
//properties
public virtual string PlayerName
{
    get;
    set;
}
public string PlayerSymbol
{
    get{return playerSymbol;}
    set{playerSymbol=value;}    
}
public int PlayerPoint
{
    get{return playerPoint;}
    set{playerPoint=value;}    
}
public int[] PlayerMoveHistory
{ 
    get { return playerMoveHistory; }
    set { playerMoveHistory = value; }

}


//constructors 

//abstract method to be inherited
//public Player GetPlayer(){return this;}


}
 class ComputerPlayer : Player
{
    //data fields
    private  string playerName = "Computer";
    private int row,col;
    //properties
    public int Row
    {
        get { return row; }
    
    }

    public int Col
    {
        get { return col; }
    
    }
    public override string PlayerName
    {
        get{return playerName;}
    }
    
    public int ComputerPlayerMove
    {
     get;set;
    }
    
    
    //methods
    public Player GetPlayer(){return this;}
    //method that selects valid move for computer to play
    public void SelectValidMove(int boardRows , int boardCols)
    {
        //use random class method next to generate random value 
        Random rnd = new Random();

        //sets computerPlayer class data fields to random integers
        row = rnd.Next(0, boardRows );
        col = rnd.Next(0, boardCols);

    }

}
 class HumanPlayer : Player
{
    //data fields
    private string playerName;

    //properties
    public override string PlayerName
    {
      get{return playerName;}
      set{playerName = value;}
    }
    
    //methods

    //overirde parent class method.
    
}