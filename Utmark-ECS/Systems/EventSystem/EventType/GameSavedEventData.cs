namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class GameSavedEventData
    {
        public DateTime TimeStamp { get; private set; } // The time the game was saved.
        public string SaveFilePath { get; private set; } // The location of the save file.
        public string Message { get; private set; } // A message regarding the save event, e.g., "Game saved successfully."

        // Constructor that sets the properties. You could set default values or allow them to be passed in.
        public GameSavedEventData(string saveFilePath, string message = "Game saved successfully.")
        {
            TimeStamp = DateTime.Now; // Set the current time as the timestamp.
            SaveFilePath = saveFilePath ?? throw new ArgumentNullException(nameof(saveFilePath));
            Message = message;
        }
        // ... any other relevant information or methods related to the game saved event.
    }
}
