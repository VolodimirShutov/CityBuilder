using System.Collections.Generic;

namespace SQLite4Unity3d.Interfaces
{
    public interface ISaveMethod
    {
        bool Load<T>(ISaveData<T> saveData);
        void Save<T>(ISaveData<T> saveData, bool replace);
        void Remove<T>(ISaveData<T> saveData);
        List<ISaveData<T>> LoadList<T>(string tableName);
        ISaveData<T> AddToList<T>(T item, string tableName, string key, bool replace);
        bool CheckSaveExists(string name);
        void CreateSave(string name);
        void ResetSave(string name);
        void DropTable(string tableName);
        void Disconnect();
        void ConnectTo(string fileName);
    }
}