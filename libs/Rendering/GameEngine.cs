using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class GameEngine
{
    private static GameEngine? _instance;
    private IGameObjectFactory gameObjectFactory;

    public static GameEngine Instance {
        get{
            if(_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameStateNode _currentGameState;

    public GameStateNode CurrentGameState{
        get { return _currentGameState; }
        set { _currentGameState = value; }
    }

    private List<SavedMap> _savedMaps;


    private GameEngine() {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
        _currentGameState = new GameStateNode();
        _savedMaps = new List<SavedMap>();
    }

    private GameObject? _focusedObject;

    public Map GetMap() {
        return _currentGameState.CurrentMap;
    }

    public List<GameObject> GetGameObjects(){
        return _currentGameState.CurrentGameObjects;
    }

    public void Setup(){

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        dynamic gameData = FileHandler.ReadJson();

        //gameData.maps

        foreach (var mapData in gameData.maps)
        {
            Map tempMap = new Map();
            List<GameObject> objList = new List<GameObject>();

            tempMap.MapWidth = mapData.width;
            tempMap.MapHeight = mapData.height;

            foreach (var gameObject in mapData.gameObjects)
            {
                AddGameObjectToList(CreateGameObject(gameObject),objList);

            }

            SavedMap savedMap = new SavedMap();
            savedMap.CurrentMap = tempMap;
            savedMap.GameObjects = objList;

            savedMap.PlayerStartingX = savedMap.GameObjects.Find(x => x.Type == GameObjectType.Player).PosX;
            savedMap.PlayerStartingY = savedMap.GameObjects.Find(x => x.Type == GameObjectType.Player).PosY;    

            _savedMaps.Add(savedMap);
        }

        _currentGameState.CurrentMap = new Map(_savedMaps[0].CurrentMap);
        _currentGameState.CurrentGameObjects = _savedMaps[0].GameObjects;
        _currentGameState.CurrentMapIndex = 0;

        _focusedObject = Player.Instance;
        _currentGameState.PlayerXPos = _focusedObject.PosX;
        _currentGameState.PlayerYPos = _focusedObject.PosY;
    }

    public void Render() {
        
        //Clean the map
        Console.Clear();

        _currentGameState.CurrentMap.Initialize();

        PlaceGameObjects();

        //Render the map
        for (int i = 0; i < _currentGameState.CurrentMap.MapHeight; i++)
        {
            for (int j = 0; j < _currentGameState.CurrentMap.MapWidth; j++)
            {
                DrawObject(_currentGameState.CurrentMap.Get(i, j));
            }
            Console.WriteLine();
        }

        if(_currentGameState.CurrentMap.GameFinished()){
            LoadNextLevel();
        }
    }
    
    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj)
    {
        return gameObjectFactory.CreateGameObject(obj);
    }

    public void AddGameObject(GameObject gameObject){
        _currentGameState.CurrentGameObjects.Add(gameObject);
    }

    public void AddGameObjectToList(GameObject gameObject, List<GameObject> list){
        list.Add(gameObject);
    }

    private void PlaceGameObjects(){
        
        _currentGameState.CurrentGameObjects.ForEach(delegate(GameObject obj)
        {
            _currentGameState.CurrentMap.Set(obj);
        });
    }

    private void DrawObject(GameObject gameObject){
        
        Console.ResetColor();

        if(gameObject != null)
        {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }

    public void UndoMove(){
        if(_currentGameState.PreviousNode == null){
            return;
        }
        _currentGameState = _currentGameState.PreviousNode;
        Player.Instance.PosX = _currentGameState.PlayerXPos;
        Player.Instance.PosY = _currentGameState.PlayerYPos;
        _currentGameState.UpdatePlayerInstancesToSingleton();
        _currentGameState.CurrentMap.UpdatePlayerInstancesToSingleton();
        Render();
    }

    public void RedoMove(){
        if(_currentGameState.NextNode == null){
            return;
        }
        _currentGameState = _currentGameState.NextNode;
        Player.Instance.PosX = _currentGameState.PlayerXPos;
        Player.Instance.PosY = _currentGameState.PlayerYPos;
        _currentGameState.UpdatePlayerInstancesToSingleton();
        _currentGameState.CurrentMap.UpdatePlayerInstancesToSingleton();
        Render();

    }

    public void SaveGameToJson(){
        FileHandler.SaveGameToJson(_currentGameState);
    }
    
    public void LoadGameFromJson(){
        GameStateNode lastGame = FileHandler.LoadGameFromJson();
        
        _currentGameState = lastGame;
        Player.Instance.PosX = _currentGameState.PlayerXPos;
        Player.Instance.PosY = _currentGameState.PlayerYPos;
        
        _currentGameState.UpdatePlayerInstancesToSingleton();
        _currentGameState.CurrentMap.UpdatePlayerInstancesToSingleton();

        Render();
    }

    public void LoadNextLevel(){
        
        int currentIndex = _currentGameState.CurrentMapIndex;

        if(currentIndex+1 < _savedMaps.Count){
            
            int newIndex = ++currentIndex;

            GameStateNode node = new GameStateNode();

            node.CurrentMap = new Map(_savedMaps[newIndex].CurrentMap);
            node.CurrentGameObjects = _savedMaps[newIndex].GameObjects;
            node.CurrentMapIndex = newIndex;

            _focusedObject = Player.Instance;
            //
            _focusedObject.PosX = _savedMaps[newIndex].PlayerStartingX;
            _focusedObject.PosY = _savedMaps[newIndex].PlayerStartingX;
            
            
            node.PlayerXPos = _focusedObject.PosX;
            node.PlayerYPos = _focusedObject.PosY;
            _currentGameState = node;
            Render();
        }else{
            //close whole game
        }
    }
}