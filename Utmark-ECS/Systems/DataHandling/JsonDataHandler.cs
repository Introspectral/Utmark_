using Newtonsoft.Json;

namespace Utmark_ECS.Systems.DataHandling
{

    public class JsonDataHandler
    {
        // Method to serialize an object to a JSON string
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        // Method to deserialize a JSON string to an object
        public T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        // Method to write the JSON representation to a file
        public void WriteToJsonFile(string filePath, string jsonString)
        {
            File.WriteAllText(filePath, jsonString);
        }

        // Method to read the JSON representation from a file
        public string ReadFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file path provided does not exist.");
            }
            return File.ReadAllText(filePath);
        }

        // (Optional) Method to serialize and write directly to a file
        public void SerializeObjectToFile(string filePath, object obj)
        {
            string jsonString = SerializeObject(obj);
            WriteToJsonFile(filePath, jsonString);
        }

        // (Optional) Method to read from a file and deserialize directly
        public T DeserializeObjectFromFile<T>(string filePath)
        {
            string jsonString = ReadFromJsonFile(filePath);
            return DeserializeObject<T>(jsonString);
        }
    }

}
