using libs;

class Program
{    
    static void Main(string[] args)
    {
        //Setup
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;
        var inputHandler = InputHandler.Instance;
        
        engine.Setup();

        // Main game loop
        do 
        {
            engine.Render();

            if(engine.IsGameComplete){
                break;
            }

            // Handle keyboard input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
        }while(true);
        Console.WriteLine("You have won! Now touch some grass!");
    }
}