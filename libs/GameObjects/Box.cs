namespace libs;

public class Box : GameObject {

    public Box () : base(){
        Type = GameObjectType.Box;
        CharRepresentation = '○';
        Color = ConsoleColor.DarkGreen;
    }

    public Box(GameObject gameObject): base(gameObject){
        Type = GameObjectType.Box;
        CharRepresentation = '○';
    }
}