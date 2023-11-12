using Newtonsoft.Json;

namespace Utmark_ECS.Systems.DataHandling
{

    public class JsonDataHandler
    {
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public void WriteToJsonFile(string filePath, string jsonString)
        {
            File.WriteAllText(filePath, jsonString);
        }

        public string ReadFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file path provided does not exist.");
            }
            return File.ReadAllText(filePath);
        }

        public void SerializeObjectToFile(string filePath, object obj)
        {
            string jsonString = SerializeObject(obj);
            WriteToJsonFile(filePath, jsonString);
        }

        public T DeserializeObjectFromFile<T>(string filePath)
        {
            string jsonString = ReadFromJsonFile(filePath);
            return DeserializeObject<T>(jsonString);
        }
    }

}
