//using Utmark_ECS.Managers;
//using Utmark_ECS.Systems.DataHandling;

//namespace Utmark_ECS.Systems.Save_Load
//{

//public class SaveLoadManager
//{
//    private readonly JsonDataHandler _jsonDataHandler;
//    private readonly string _saveDirectory;

//    public SaveLoadManager(string saveDirectory)
//    {
//        _jsonDataHandler = new JsonDataHandler();
//        _saveDirectory = saveDirectory;

//        Directory.CreateDirectory(_saveDirectory);
//    }

//    public void SaveGame(GameState gameState, string fileName)
//    {
//        string filePath = Path.Combine(_saveDirectory, fileName);
//        try
//        {
//            _jsonDataHandler.SerializeObjectToFile(filePath, gameState);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
//        }
//    }

//    public GameState LoadGame(string fileName)
//    {
//        string filePath = Path.Combine(_saveDirectory, fileName);
//        try
//        {
//            if (!File.Exists(filePath))
//            {
//                throw new FileNotFoundException($"Save file not found: {filePath}");
//            }

//            return _jsonDataHandler.DeserializeObjectFromFile<GameState>(filePath);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"An error occurred while loading the game: {ex.Message}");
//            return null;
//        }
//    }
//}

//}
