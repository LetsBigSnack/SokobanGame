namespace libs;

public class Obstacle : GameObject {
    public Obstacle () : base() {
        this.Type = GameObjectType.Obstacle;
        this.CharRepresentation = '█';
        this.Color = ConsoleColor.Cyan;
    }

    public Obstacle(GameObject gameObject): base(gameObject){
        this.Type = GameObjectType.Obstacle;
        this.CharRepresentation = '█';
    }
}