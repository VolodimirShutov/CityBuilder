using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite4Unity3d.Interfaces;
using UnityEngine;

namespace SQLite4Unity3d
{
    public class SaveDataList<T>
    {
        private SaveManager saveManager;
        private string tableName;
        private string normalizedTableName;

        public Dictionary<string, ISaveData<T>> Dictionary { get; private set; }

        public SaveDataList(string tableName)
        {
            this.tableName = tableName;
            normalizedTableName = NormalizeTableName(tableName, typeof(T));
            var list = saveManager.LoadList<T>(tableName);
            Dictionary = list.ToDictionary(x => x.Key);
        }

        private string NormalizeTableName(string name, Type type)
        {
            StringBuilder tableNameSb = new StringBuilder(name, 50);
            tableNameSb.Append("_");
            tableNameSb.Append(type.ToString().Replace("System", "").Replace(".", "").Replace("+", "").ToLower());
            return tableNameSb.ToString();
        }

        public ISaveData<T> Add(T item, string key = "", bool replace = false)
        {
            if (replace)
            {
                var value = Dictionary[key];
                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    value.UpdateValue(item);
                    return value;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(key) && Dictionary.ContainsKey(key)) return null;
            }

            ISaveData<T> result = saveManager.AddToList(item, tableName, key, replace);
            Dictionary.Add(result.Key, result);

            return result;
        }

        public ISaveData<T> Find(T obj)
        {
            if (obj.GetType().IsValueType)
            {
                Debug.LogError("Can't find value type " + obj.ToString() + " from " + tableName);
            }

            return Dictionary.First(x => x.Value.Value.Equals(obj)).Value;
        }

        /// <summary>
        /// Removing reference types only. For all types please use RemoveById(string key)
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(T obj)
        {
            var result = Find(obj);

            if (result.Key == string.Empty) return;
            result.Remove();
            Dictionary.Remove(result.Key);

        }

        /// <summary>
        /// Removing value types only. For references please use Remove(T obj)
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(string id)
        {
            if (Dictionary.ContainsKey(id))
            {
                Dictionary[id].Remove();
                Dictionary.Remove(id);
            }
        }

        /// <summary>
        /// Remove all valuses.
        /// </summary>
        public void Reset()
        {
            saveManager.DropTable(normalizedTableName);
            Dictionary.Clear();
        }

        public int Count
        {
            get => Dictionary.Count();
        }
    }
}