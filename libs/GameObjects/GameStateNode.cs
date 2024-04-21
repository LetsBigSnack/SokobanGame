using Newtonsoft.Json;

namespace libs;


public class GameStateNode{

    [JsonProperty]
    private Map _currentMap;
    
    [JsonProperty]
    private List<GameObject> _currentGameObjects = new List<GameObject>();

    [JsonProperty]
    private int _playerXPos;

    [JsonProperty]
    private int _playerYPos;

    [JsonProperty]
    private GameStateNode? _previousNode;

    [JsonProperty]
    private GameStateNode? _nextNode;

    public GameStateNode(){
        _currentMap = new Map();
        _currentGameObjects = new List<GameObject>();
    }

    public GameStateNode(GameStateNode gameState){
        _currentGameObjects = gameState.DeepCopyGameObjects();
        _currentMap = gameState.DeepCopyMap();
        _playerXPos = gameState.PlayerXPos;
        _playerYPos = gameState.PlayerYPos;
    }

    public Map CurrentMap{
        get { return _currentMap ; }
        set { _currentMap = value; }
    }

    public List<GameObject>  CurrentGameObjects{
        get { return _currentGameObjects; }
        set { _currentGameObjects = value; }
    }

    public int PlayerXPos {
         get { return _playerXPos; }
        set { _playerXPos = value; }
    }

    public int PlayerYPos {
        get { return _playerYPos; }
        set { _playerYPos = value; }
    }

    public GameStateNode? PreviousNode {
        get { return _previousNode; }
        set { _previousNode = value; }
    }

    public GameStateNode? NextNode {
        get { return _nextNode; }
        set { _nextNode = value; }
    }


    public List<GameObject> DeepCopyGameObjects(){

        List<GameObject> copiedList = new List<GameObject>();

        foreach(GameObject gameObject in _currentGameObjects){
            switch (gameObject.Type)
            {
                case GameObjectType.Player:
                    copiedList.Add(Player.Instance);
                    break;
                case GameObjectType.Obstacle:
                    copiedList.Add(new Obstacle(gameObject));
                    break;
                case GameObjectType.Box:
                    copiedList.Add(new Box(gameObject));
                    break;
                case GameObjectType.Goal:
                    copiedList.Add(new Goal(gameObject));
                    break;
                case GameObjectType.Floor:
                    copiedList.Add(new Floor(gameObject));
                    break;
            }            
        }

        return copiedList;
    }

    public Map DeepCopyMap(){
        return new Map(_currentMap);
    }

}