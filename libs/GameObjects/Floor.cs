namespace libs;

public class Floor : GameObject {

    public Floor () : base(){
        Type = GameObjectType.Floor;
        CharRepresentation = '.';
    }

    public Floor(GameObject gameObject): base(gameObject){
        Type = GameObjectType.Floor;
        CharRepresentation = '.';
    }
}