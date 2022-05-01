namespace SQLite4Unity3d.Interfaces
{
    public interface ISaveManager
    {
        void Save<T>(ISaveData<T> saveData, bool replace = false);
        bool Load<T>(ISaveData<T> saveData);
        void Remove<T>(ISaveData<T> saveData);
        void ClearAllProgress();
    }
}