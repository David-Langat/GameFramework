public class UserInterface
{
    public static void DisplayWelcomeMessage()
    {
        Console.WriteLine("Hello there! welcome to our game.");
        Console.WriteLine("This game framework has two game: Connect Four Game, SOS Game");
        

    }
    public static void DecideGameToPlay()
    {
        Console.WriteLine("Enter 1 for SOS Game. Enter 2 for Connect Four Game.");

    }

    public static void DecidePlayerMode()
    {
        Console.WriteLine("Enter 1 for Human Vs Human or Enter 2 for Computer VS Human.");

    }


    // public static void Displayplayer1()
    // {
    //     Console.WriteLine("enter first player type:");
        
    // }
    public static void InvalidInput()
    {
        Console.WriteLine("INVALID INPUT!!!! Please enter input as intructed");
        
    }
    public static void RequestPlayerNameInput()
    {
        Console.WriteLine("Please enter player name:");
        
    }
    public static void ShowPlayerTurn(Player player)
    {
        Console.WriteLine("{0} please play move ", player.PlayerName);
        
    }
    public static void SOSHelpGuide()
    {
        Console.WriteLine("*******************************");
        Console.WriteLine("This is a typical 3x3 SOS Game;");
        Console.WriteLine("Enter row number and column number will replace the space;");
        Console.WriteLine("Each player can choose S or O to replace;");
        Console.WriteLine("Who can line up SOS first who can get one point;");
        Console.WriteLine("After all place be replaced, caculating the player points and decide who is winner");
        Console.WriteLine("*******************************");    
    }
        public static void ConnectFourHelpGuide()
    {
        Console.WriteLine("*******************************");
        Console.WriteLine("This is a typical 7x6 ConnectFour Game;");
        Console.WriteLine("Enter column number will replace the space;");
        Console.WriteLine("Player1 will use O and plyer2 will use X to replace the space");
        Console.WriteLine("Who can line up 4 connection who can we the game;");
        Console.WriteLine("*******************************");    
    }
}