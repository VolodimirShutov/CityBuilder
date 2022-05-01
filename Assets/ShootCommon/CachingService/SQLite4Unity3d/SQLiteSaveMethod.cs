
using System;
using System.Collections.Generic;
using System.Text;
using SQLite4Unity3d.Interfaces;
using UnityEngine;

namespace SQLite4Unity3d
{
    public class SQLiteSaveMethod : ISaveMethod
    {
        private const string FileExtension = "db";
        private SaveManager saveManager;
        private SQLiteConnection dbconnection;

        public SQLiteSaveMethod(SaveManager saveManager)
        {
            this.saveManager = saveManager;

            if (!CheckSaveExists(SaveManager.DBName))
            {
                CreateSave(SaveManager.DBName);
            }

            ConnectTo(SaveManager.DBName);
        }

        public void ConnectTo(string fileName)
        {
            if (CheckSaveExists(fileName))
            {
                dbconnection = new SQLiteConnection(GetSavePath(fileName),
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            }
        }

        public static string GetSavePath(string fileName)
        {
            var p = string.Format("{0}/{1}.{2}", SaveManager.GetSavePath(), fileName, FileExtension);

            return p;
        }

        public string GetExplorerSavePath(string fileName)
        {
            return GetSavePath(fileName).Replace(@"/", @"\");
        }

        public bool CheckSaveExists(string fileName)
        {
            return FileManagement.FileExists(GetSavePath(fileName));
        }

        public void CreateSave(string fileName)
        {
            FileManagement.CopyFile(string.Format("{0}.{1}", SaveManager.DBName, FileExtension),
                GetExplorerSavePath(fileName));
        }

        public void DeleteSave(string fileName)
        {
            FileManagement.DeleteFile(GetSavePath(fileName), true);
        }

        public void ResetSave(string fileName)
        {
            DeleteSave(GetSavePath(fileName));
            CreateSave(GetSavePath(fileName));
        }

        public bool Load<T>(ISaveData<T> saveData)
        {
            if (saveData == null) return false;

            try
            {
                string cachedTableName = NormalizeTableName(saveData.TableName, typeof(T));
                string commandText =
                    string.Format("SELECT * FROM {0} WHERE Key = '{1}'", cachedTableName, saveData.Key);
                SQLiteCommand command = new SQLiteCommand(dbconnection);
                command = dbconnection.CreateCommand(commandText, new object[0]);

                if (saveData.GetType() == typeof(SaveData<T>))
                {
                    var result = command.ExecuteQuery<SaveData<T>>(cachedTableName);
                    if (result.Count > 0)
                    {
                        saveData.Id = result[0].Id;
                        saveData.SetValueWithoutSave(result[0].Value);
                        return true;
                    }
                }
                else if (saveData.GetType() == typeof(SerializableSaveData<T>))
                {
                    var result = command.ExecuteQuery<SerializableSaveData<T>>(cachedTableName);
                    if (result.Count > 0)
                    {
                        saveData.Id = result[0].Id;
                        ((SerializableSaveData<T>) saveData).SetSerializedValueWithoutSave(result[0].SerializedValue);
                        return true;
                    }
                }
            }
            catch (Exception exept)
            {
                //Debug.LogError("Load "  + exept.Message );
            }

            return false;
        }

        public void Save<T>(ISaveData<T> saveData, bool replace)
        {
            if (saveData.TableName == string.Empty || saveData.TableName == null) return;

            string cachedTableName = NormalizeTableName(saveData.TableName, typeof(T));

            dbconnection.CreateTable(saveData.GetType(), cachedTableName);

            dbconnection.Insert(saveData, cachedTableName, replace ? "OR REPLACE" : "", saveData.GetType());
        }

        private string NormalizeTableName(string name, Type type)
        {
            string tableName;
            StringBuilder tableNameSb = new StringBuilder(name, 50);
            tableNameSb.Append("_");
            tableNameSb.Append(type.ToString().Replace("System", "").Replace(".", "").Replace("+", "").ToLower());
            return tableName = tableNameSb.ToString();
        }

        public List<ISaveData<T>> LoadList<T>(string tableName)
        {
            List<ISaveData<T>> list = new List<ISaveData<T>>();
            try
            {
                string cachedTableName = NormalizeTableName(tableName, typeof(T));
                string commandText = string.Format("SELECT * FROM {0}", cachedTableName);
                SQLiteCommand command = new SQLiteCommand(dbconnection);
                command = dbconnection.CreateCommand(commandText, new object[0]);

                if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                {
                    var result = command.ExecuteQuery<SaveData<T>>(cachedTableName);

                    foreach (var item in result)
                    {
                        item.TableName = tableName;
                        list.Add(item);
                    }
                }
                else
                {
                    var result = command.ExecuteQuery<SerializableSaveData<T>>(cachedTableName);
                    foreach (var item in result)
                    {
                        item.TableName = tableName;
                        item.SetValueWithoutSave(saveManager.saveSerializator.Deserialize<T>(item.SerializedValue));
                        list.Add(item);
                    }
                }
            }
            catch (Exception)
            {

            }

            return list;
        }

        public ISaveData<T> AddToList<T>(T item, string tableName, string key = "", bool replace = false)
        {
            if (tableName == string.Empty || tableName == null || item == null) return null;

            string cachedTableName = NormalizeTableName(tableName, typeof(T));

            ISaveData<T> temp;

            if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
            {
                temp = new SaveData<T>(key, tableName);
            }
            else
            {
                temp = new SerializableSaveData<T>();
                temp.Key = key;
            }

            temp.SetValueWithoutSave(item);
            temp.TableName = tableName;

            dbconnection.CreateTable(temp.GetType(), cachedTableName);
            dbconnection.Insert(temp, cachedTableName, replace ? "OR REPLACE" : "", temp.GetType());
            return temp;
        }

        public void Remove<T>(ISaveData<T> saveData)
        {
            string cachedTableName = NormalizeTableName(saveData.TableName, typeof(T));
            dbconnection.Delete(saveData, cachedTableName);
        }

        public void DropTable(string tableName)
        {
            dbconnection.DropTable(tableName);
        }

        ~SQLiteSaveMethod()
        {
            dbconnection.Close();
            SQLite4Unity3d.SQLite3.Shutdown();
        }

        public void Disconnect()
        {
            dbconnection.Close();
        }
    }
}