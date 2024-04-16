namespace libs;

public class GameObject : IGameObject, IMovement
{
    private char _charRepresentation = '#';
    private ConsoleColor _color;

    private int _posX;
    private int _posY;
    
    private int _prevPosX;
    private int _prevPosY;

    public GameObjectType Type;

    public GameObject() {
        this._posX = 5;
        this._posY = 5;
        this._color = ConsoleColor.Gray;
    }

    public GameObject(int posX, int posY){
        this._posX = posX;
        this._posY = posY;
    }

    public GameObject(int posX, int posY, ConsoleColor color){
        this._posX = posX;
        this._posY = posY;
        this._color = color;
    }

    public char CharRepresentation
    {
        get { return _charRepresentation ; }
        set { _charRepresentation = value; }
    }

    public ConsoleColor Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public int PosX
    {
        get { return _posX; }
        set { _posX = value; }
    }

    public int PosY
    {
        get { return _posY; }
        set { _posY = value; }
    }

    public int GetPrevPosY() {
        return _prevPosY;
    }
    
    public int GetPrevPosX() {
        return _prevPosX;
    }
    //TODO: move this to player/service
    public void Move(int dx, int dy) {
        if(this.checkIfPossible(this._posX, this._posY, dx, dy)){
            if(this.pushBox(this._posX, this._posY,dx, dy)){
                _prevPosX = _posX;
                _prevPosY = _posY;
                _posX += dx;
                _posY += dy;
            }
        }
    }

    public bool checkIfPossible(int currentX, int currentY, int newPosX, int newPosY){
        List<GameObject> gameObjects = GameEngine.Instance.GetGameObjects();
        Console.WriteLine(gameObjects);

        //go trough the map and check if
        // -> upcoming position is "taken" && isNot a Floor
        // -> if type is "movable" apply changes to movable
        // -> return either true/false

        for(int i = 0; i < gameObjects.Count; i++){
            if(gameObjects[i].Type == GameObjectType.Obstacle && gameObjects[i].PosX == currentX + newPosX &&
             gameObjects[i].PosY == currentY + newPosY){
                Console.WriteLine("I happened");
                return false;
            } 
        }
        return true;
    }

    public bool checkIfNoBox(int currentX, int currentY, int newPosX, int newPosY){
         List<GameObject> gameObjects = GameEngine.Instance.GetGameObjects();
        Console.WriteLine(gameObjects);

        for(int i = 0; i < gameObjects.Count; i++){
            if(gameObjects[i].Type == GameObjectType.Box && gameObjects[i].PosX == currentX + newPosX &&
             gameObjects[i].PosY == currentY + newPosY){
                Console.WriteLine("I happened");
                return false;
            } 
        }
        return true;
    }

    public bool pushBox(int currentX, int currentY, int newPosX, int newPosY){
        List<GameObject> gameObjects = GameEngine.Instance.GetGameObjects();
        for(int i = 0; i < gameObjects.Count; i++){
            if(gameObjects[i].Type == GameObjectType.Box && gameObjects[i].PosX == currentX + newPosX &&
             gameObjects[i].PosY == currentY + newPosY){
                if(checkIfPossible(gameObjects[i]._posX, gameObjects[i]._posY, newPosX, newPosY)){
                    if(checkIfNoBox(gameObjects[i]._posX, gameObjects[i]._posY, newPosX, newPosY)){
                    gameObjects[i].PosX += newPosX;
                    gameObjects[i].PosY += newPosY;
                    return true;
                    }
                }
                return false;
            }
        }
        return true;
    }
}
