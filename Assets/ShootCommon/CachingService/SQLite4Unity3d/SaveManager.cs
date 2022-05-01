using System.Collections.Generic;
using System.IO;
using SQLite4Unity3d.Interfaces;
using UnityEngine;

namespace SQLite4Unity3d
{
    public class SaveManager : ISaveManager
    {
        private enum SaveGamePath
        {
            PersistentDataPath,
            DataPath
        }

        #region const

        public const string DefaultProfileName = "default";
        private const string SaveFolder = "Saves";

        #endregion

        #region variable

        private ISaveMethod saveMethod;

        private static SaveGamePath SavePath = SaveGamePath.PersistentDataPath;

        #endregion

        #region property

        public static string DBName = "default";
        public ISaveSerializator saveSerializator;

        #endregion

        public SaveManager()
        {
            if (!Directory.Exists(GetSavePath()))
            {
                Directory.CreateDirectory(GetSavePath());
            }

            saveMethod = new SQLiteSaveMethod(this);
            saveSerializator = new JsonSaveSerializator();
        }

        public static string GetSavePath()
        {
            switch (SavePath)
            {
                default:
                case SaveGamePath.PersistentDataPath:
                    return $"{Application.persistentDataPath}/{SaveFolder}";
                case SaveGamePath.DataPath:
                    return $"{Application.dataPath}/{SaveFolder}";
            }
        }

        public bool Load<T>(ISaveData<T> saveData)
        {
            return saveMethod.Load<T>(saveData);
        }

        public void Save<T>(ISaveData<T> saveData, bool replace = false)
        {
            saveMethod.Save<T>(saveData, replace);
        }

        public void Remove<T>(ISaveData<T> saveData)
        {
            saveMethod.Remove<T>(saveData);
        }

        public IEnumerable<ISaveData<T>> LoadList<T>(string tableName)
        {
            return saveMethod.LoadList<T>(tableName);
        }

        public ISaveData<T> AddToList<T>(T item, string tableName, string key, bool replace)
        {
            return saveMethod.AddToList<T>(item, tableName, key, replace);
        }

        public void DropTable(string tableName)
        {
            saveMethod.DropTable(tableName);
        }

        public void SaveProgress(string saveName)
        {
            saveMethod.Disconnect();
            FileManagement.CopyFile(SQLiteSaveMethod.GetSavePath(SaveManager.DBName),
                SQLiteSaveMethod.GetSavePath(saveName));
            saveMethod.ConnectTo(SaveManager.DBName);
        }

        public void ClearAllProgress()
        {
            //File.Delete(SQLiteSaveMethod.GetSavePath(SaveManager.DBName));
        }

        public string[] GetSaveList()
        {
            return FileManagement.ListFiles(SaveManager.GetSavePath());
        }


        public void SetSaveName(string saveName)
        {
            var strlist = saveName.Split('.');
            DBName = strlist[0];
        }

        public void LoadSave()
        {
            saveMethod.Disconnect();
            saveMethod.ConnectTo(DBName);
        }
    }
}