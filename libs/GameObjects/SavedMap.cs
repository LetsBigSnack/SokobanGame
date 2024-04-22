namespace libs;

public class SavedMap {

    private Map _map;
    private List<GameObject> _gameObjects;
    private int _playerStartingX;
    private int _playerStartingY;
    



    public SavedMap () : base(){
        _map = new Map();
        _gameObjects = new List<GameObject>();
    }

    public Map CurrentMap{
        get {return _map;}
        set { _map = value;}
    }

    public List<GameObject> GameObjects{
        get {return _gameObjects;}
        set { _gameObjects = value;}
    }

    public int PlayerStartingX{
        get {return _playerStartingX;}
        set { _playerStartingX = value;}
    }

     public int PlayerStartingY{
        get {return _playerStartingY;}
        set { _playerStartingY = value;}
    }
}