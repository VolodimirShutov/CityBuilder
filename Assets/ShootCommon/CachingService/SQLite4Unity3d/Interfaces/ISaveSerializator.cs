namespace SQLite4Unity3d.Interfaces
{
    public interface ISaveSerializator
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
    }
}