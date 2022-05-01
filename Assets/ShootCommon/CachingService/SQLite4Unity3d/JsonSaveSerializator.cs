using System;
using SQLite4Unity3d.Interfaces;
using UnityEngine;

namespace SQLite4Unity3d
{
    public class JsonSaveSerializator : ISaveSerializator
    {
        public T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            byte[] mBuffer = System.Text.Encoding.UTF8.GetBytes(value);

            try
            {
                if (mBuffer != null && mBuffer.Length > 0)
                {
                    return JsonUtility.FromJson<T>(value);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Debug.Log("BinarySaveSerializator.Deserialize" + typeof(T).ToString() + value);
                return default;
            }
        }

        public string Serialize<T>(T value)
        {
            if (value == null) return default;

            try
            {
                return JsonUtility.ToJson(value);
            }
            catch (Exception)
            {
                Debug.Log("BinarySaveSerializator.Serialize" + typeof(T).ToString() + value.ToString());
                return default;
            }
        }
    }
}