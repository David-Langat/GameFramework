
class Program
{
        public static void Main(string[] args)
    {
        
        //declare variables
        string playerMode, gameType;
        int userInput;
        

        //User selects either SOS game || Conectfour  as gameType
        UserInterface.DisplayWelcomeMessage();
        UserInterface.DecideGameToPlay();
        userInput = IntUserInput();
        GameType(userInput, out gameType);
        Console.WriteLine("{0}",gameType);

        //users selects either Human Vs Human || Computer VS Human as playerMode
        UserInterface.DecidePlayerMode();
        userInput = IntUserInput();
        PlayerMode(userInput, out playerMode);
        Console.WriteLine("{0}", playerMode);

        //play game according to selection
        if(gameType == "SOS")
        {
            int playerNumber = 2;
            Game sosGame = new SOSGame();
            sosGame.PlayGame(playerNumber, playerMode);

        }else if (gameType == "ConnectFour")
        {
            int playerNumber = 2;
            Game connectFour = new ConnectFour();
            connectFour.PlayGame(playerNumber,playerMode);
        }
        

        //play selected game 
        /*if(gameType == "SOS")
        {
            int playerNumber = 2;
            Game sosGame = new SOSGame();
            sosGame.PlayGame(playerNumber, playerMode);
        }else if (gameType == "ConnectFour")
        {
            int playerNumber = 2;
            Game connectFourGame = new SOSGame();
            //connectFourGame.PlayGame(playerNumber);
        }

        IBoard board1= new ConnectFourBoard(7,6);
    
        board1.Display();*/

        
        
    }

    //method to capture user integer input
    public static int IntUserInput()
    {
        //delcare local variables
        string userInput;
        int userIntInput;
        //check if inputis interger if not reprompt user for input
        userInput = Console.ReadLine() ?? "";
        while(!int.TryParse(userInput,out userIntInput))
        {
            Console.WriteLine("INVALID INPUT!!! Enter a number");
            userInput = Console.ReadLine() ?? "";
        }
        return userIntInput;
    }

     
    //method that checks if input is valid
    public static bool IsValid(int userInput)
    {
        
        if(userInput == 1 || userInput == 2)
        {
            return true ;
        }else
        {
            return false;
        }

    }
    //method that allows user to select either Human VS Human or Computer VS Human
    public static void PlayerMode(int selectedPlayerMode , out string playerMode)
    {
        while(!IsValid(selectedPlayerMode))
        {
            UserInterface.InvalidInput();
            UserInterface.DecidePlayerMode();
            selectedPlayerMode = IntUserInput();
        }
        
            if(selectedPlayerMode == 1)
            {
                playerMode = "HVH";

            }else
            {
                playerMode = "CVH";
            }
    }


    //method that allows user to select either SOS || ConnectFour
    public static void GameType(int selectedGameType , out string gameType)
    {
        while(!IsValid(selectedGameType))
        {
            UserInterface.InvalidInput();
            UserInterface.DecideGameToPlay();
            selectedGameType = IntUserInput();
        }
        
            if(selectedGameType == 1)
            {
                gameType = "SOS";
           
            }else
            {
                gameType = "ConnectFour";
            }
        }
}