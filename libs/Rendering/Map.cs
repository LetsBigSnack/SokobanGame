namespace libs;
using Newtonsoft.Json;

public class Map {
    private char[,] RepresentationalLayer;
    private GameObject?[,] GameObjectLayer;

    private int _mapWidth;
    private int _mapHeight;

    public Map () {
        _mapWidth = 30;
        _mapHeight = 8;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    public Map (int width, int height) {
        _mapWidth = width;
        _mapHeight = height;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    public Map (Map map){
        _mapWidth = map.MapWidth;
        _mapHeight = map.MapHeight;
        RepresentationalLayer = map.DeepCopyRepresentationalLayer();
        GameObjectLayer = map.DeepCopyGameObjectLayer();
    }


    public void Initialize()
    {
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];


        // Initialize the map with some default values
        for (int i = 0; i < GameObjectLayer.GetLength(0); i++)
        {
            for (int j = 0; j < GameObjectLayer.GetLength(1); j++)
            {
                GameObjectLayer[i, j] = new Floor();
            }
        }
    }

    public int MapWidth
    {
        get { return _mapWidth; } // Getter
        set { _mapWidth = value; Initialize();} // Setter
    }

    public int MapHeight
    {
        get { return _mapHeight; } // Getter
        set { _mapHeight = value; Initialize();} // Setter
    }

    public GameObject Get(int x, int y){
        return GameObjectLayer[x, y];
    }

    public void Set(GameObject gameObject){
        int posY = gameObject.PosY;
        int posX = gameObject.PosX;
        int prevPosY = gameObject.GetPrevPosY();
        int prevPosX = gameObject.GetPrevPosX();
    
        //Map

        if (prevPosX >= 0 && prevPosX < _mapWidth &&
                prevPosY >= 0 && prevPosY < _mapHeight)
        {
            if(GameObjectLayer[prevPosY, prevPosX] is Floor){
            GameObjectLayer[prevPosY, prevPosX] = new Floor();
            }
            
        }
        
        if (posX >= 0 && posX < _mapWidth &&
                posY >= 0 && posY < _mapHeight)
        {
            GameObjectLayer[posY, posX] = gameObject;
            RepresentationalLayer[gameObject.PosY, gameObject.PosX] = gameObject.CharRepresentation;
        }
    }

    public GameObject?[,] DeepCopyGameObjectLayer(){
        
        GameObject?[,] copiedGameObjectLayer = new GameObject[_mapHeight, _mapWidth];

        for (int i = 0; i < GameObjectLayer.GetLength(0); i++)
        {
            for (int j = 0; j < GameObjectLayer.GetLength(1); j++)
            {


            switch (GameObjectLayer[i, j].Type)
            {
                case GameObjectType.Player:
                    copiedGameObjectLayer[i, j] = Player.Instance;
                    break;
                case GameObjectType.Obstacle:
                    copiedGameObjectLayer[i, j] = new Obstacle(GameObjectLayer[i, j]);
                    break;
                case GameObjectType.Box:
                    copiedGameObjectLayer[i, j] = new Box(GameObjectLayer[i, j]);
                    break;
                case GameObjectType.Goal:
                    copiedGameObjectLayer[i, j] = new Goal(GameObjectLayer[i, j]);
                    break;
                case GameObjectType.Floor:
                     copiedGameObjectLayer[i, j] =new Floor(GameObjectLayer[i, j]);
                    break;
            }
            }
        }

        return copiedGameObjectLayer;
    }

    public char[,] DeepCopyRepresentationalLayer(){

        char[,] copiedPresentationLayer = new char[_mapHeight, _mapWidth];

        for (int i = 0; i < RepresentationalLayer.GetLength(0); i++)
        {
            for (int j = 0; j < RepresentationalLayer.GetLength(1); j++)
            {
                copiedPresentationLayer[i, j] = RepresentationalLayer[i,j];
            }
        }
        return copiedPresentationLayer;
    }

    public void GameFinished(){
        List<GameObject> _boxes = GameEngine.Instance.GetGameObjects().FindAll(e=>e.Type == GameObjectType.Box);
        List<GameObject> _goals = GameEngine.Instance.GetGameObjects().FindAll(e=>e.Type == GameObjectType.Goal);

        int counter = 0;

        foreach(GameObject box in _boxes){
            foreach(GameObject goal in _goals){
                if((box.PosX == goal.PosX) && (box.PosY == goal.PosY)){
                    counter++;
                }
            }
        }

        if(counter == _goals.Count){
            Console.WriteLine("You've won!");
        } else {
            counter = 0;
        }        
    }



}