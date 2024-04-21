using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;

namespace libs;

using Newtonsoft.Json;

public static class FileHandler
{
    private static string filePath;
    private readonly static string envVar = "GAME_SETUP_PATH";

    static FileHandler()
    {
        Initialize();
    }

    private static void Initialize()
    {
        if(Environment.GetEnvironmentVariable(envVar) != null){
            filePath = Environment.GetEnvironmentVariable(envVar);
        };
    }

    public static dynamic ReadJson()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static void SaveGameToJson(GameStateNode _currentSate){
        string fileName = "SavedGame.json";
        var settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        string jsonString = JsonConvert.SerializeObject(_currentSate, settings);
        File.WriteAllText(fileName, jsonString);
    }

    public static GameStateNode LoadGameFromJson(){
        try
        {
            string jsonContent = File.ReadAllText("SavedGame.json");
            GameStateNode newGameStateNode = JsonConvert.DeserializeObject<GameStateNode>(jsonContent);
            return newGameStateNode;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: SavedGame.json");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
        return null;
    }
}
