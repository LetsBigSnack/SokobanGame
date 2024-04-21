namespace libs;

public class SavedMap {

    private Map _map;
    private List<GameObject> _gameObjects;

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
}