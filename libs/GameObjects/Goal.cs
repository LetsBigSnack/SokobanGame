namespace libs;

public class Goal : GameObject {

    public Goal () : base(){
        Type = GameObjectType.Goal;
        CharRepresentation = '⚑';
        Color = ConsoleColor.Red;
    }

    public Goal(GameObject gameObject): base(gameObject){
        Type = GameObjectType.Goal;
        CharRepresentation = '⚑';
    }
}