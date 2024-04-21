using System.Windows.Input;

namespace libs;

public sealed class InputHandler{

    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance {
        get{
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler() {
        //INIT PROPS HERE IF NEEDED
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = Player.Instance;

        if (focusedObject != null) {
            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    focusedObject.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.Move(1, 0);
                    break;
                case ConsoleKey.Z:
                    if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control) {
                        engine.UndoMove();
                    }
                    break;
                case ConsoleKey.Y:
                    if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control) {
                        engine.RedoMove();
                    }
                    break;
                default:
                    break;
            }
        } 
    }
}