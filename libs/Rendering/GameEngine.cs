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

    private GameEngine() {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
        _currentGameState = new GameStateNode();
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
        
        _currentGameState.CurrentMap.MapWidth = gameData.map.width;
        _currentGameState.CurrentMap.MapHeight = gameData.map.height;

        foreach (var gameObject in gameData.gameObjects)
        {
            AddGameObject(CreateGameObject(gameObject));
        }
        
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
    }
    
    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj)
    {
        return gameObjectFactory.CreateGameObject(obj);
    }

    public void AddGameObject(GameObject gameObject){
        _currentGameState.CurrentGameObjects.Add(gameObject);
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
}