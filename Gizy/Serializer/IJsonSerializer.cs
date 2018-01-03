namespace Gizy.Serializer
{
    public interface IJsonSerializer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject<T>(T value);
    }
}