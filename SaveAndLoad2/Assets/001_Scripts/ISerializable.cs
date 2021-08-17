using Newtonsoft.Json.Linq;

public interface ISerializable
{
    JObject Serialize();
    void DeSerialize(string jsonString);
    string GetJsonKey();
}