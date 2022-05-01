using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using SQLite4Unity3d.Interfaces;
using Zenject;

namespace ShootCommon.CachingService
{
    public class CachingServiceSQL : ICachingService
    {
        private Dictionary<string, object> _modelsDictionary = new Dictionary<string, object>() ;
        private ISaveManager _saveManager;

        [Inject]
        public void Init(ISaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        private void LoadSerializable<T>(string key, Action<SerializableSaveData<T>> callback) where T : class
        {
            var data = new SerializableSaveData<T>(key, key);
            
            _modelsDictionary.Add(key, data);
            callback.Invoke(data);
        }
        
        public void Save<T>(string key, T value, Action callback) where T : class
        {
            if (_modelsDictionary.ContainsKey(key))
            {
                SerializableSaveData<T> model = _modelsDictionary[key] as SerializableSaveData<T>;
                if (model == null)
                {
                    callback?.Invoke();
                    return;
                }

                model.UpdateValue(value);
                model.Value = value;
                model.UpdateValue();
                callback?.Invoke();
            }
            else
            {
                LoadSerializable<T>(key, data =>
                {
                    SerializableSaveData<T> model = _modelsDictionary[key] as SerializableSaveData<T>;
                    if (model == null)
                    {
                        callback?.Invoke();
                        return;
                    }

                    model.Value = value;
                    model.UpdateValue();
                });
            }
            
        }

        public void Read<T>(string key, Action<T> callback) where T : class
        {
            if (_modelsDictionary.ContainsKey(key))
            {
                SerializableSaveData<T> model = _modelsDictionary[key] as SerializableSaveData<T>;
                if(model == null)
                    callback.Invoke(null);
                else
                    callback.Invoke(model.Value);
            }
            else
            {
                LoadSerializable<T>(key, data =>
                {
                    SerializableSaveData<T> model = _modelsDictionary[key] as SerializableSaveData<T>;
                    if(model == null)
                        callback.Invoke(null);
                    else
                        callback.Invoke(model.Value);
                });
            }
        }
    }
}