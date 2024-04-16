namespace libs;

public sealed class Player : GameObject {

public static Player instance;
private static readonly object lockObject = new object();

    public Player () : base(){
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
    }

 public static Player Instance
        {
            get
            {   
                if (instance == null)
                {
                    lock (lockObject)
                    {
                    
                        if (instance == null)
                        {
                            instance = new Player();
                        }
                    }
                }
                return instance;
            }
        }
    }