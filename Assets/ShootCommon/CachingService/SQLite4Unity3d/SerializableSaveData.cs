using System;
using SQLite4Unity3d.Interfaces;
using Zenject;


namespace SQLite4Unity3d
{
    public class SerializableSaveData<T> : ISaveData<T>, IDataBind<T>
    {
        public event Action<T> OnValueChangeHandler;

        private SaveManager saveManager;
        private string tableName;
        private int id;
        private string key;
        private T value;
        private string serializedValue;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (Key == string.Empty || Key == null)
                {
                    Key = id.ToString();
                    UpdateValue();
                }
            }
        }

        public virtual string Key
        {
            get => key;
            set => key = value;
        }

        [Ignore]
        public string TableName
        {
            get => tableName;
            set => tableName = value;
        }

        [Ignore]
        public virtual T Value
        {
            get => value;
            set
            {
                if (value != null && !value.Equals(this.value))
                {
                    this.value = value;
                    SerializedValue = saveManager.saveSerializator.Serialize(value);
                    OnValueChange();
                    saveManager.Save<T>(this, true);
                    return;
                }
            }
        }

        public virtual string SerializedValue
        {
            get => serializedValue;
            set => serializedValue = value;
        }

        public void UpdateValue(T value)
        {
            if (value != null && (!value.Equals(this.value) || !typeof(T).IsPrimitive || typeof(T) == typeof(string)))
            {
                this.value = value;
                OnValueChange();
                UpdateValue();
            }
        }

        public void UpdateValue()
        {
            SerializedValue = saveManager.saveSerializator.Serialize(value);
            saveManager.Save<T>(this, true);
        }

        [Inject]
        public void Init(ISaveManager manager)
        {
            saveManager = manager as SaveManager;
        }
        
        public SerializableSaveData()
        {
            ProjectContext.Instance.Container.Inject(this);
        }
        

        public SerializableSaveData(string key, string tableName)
        {
            ProjectContext.Instance.Container.Inject(this);
            this.tableName = tableName;
            this.key = key;
            Load();
        }

        public void Load()
        {
            var tempValue = value;
            if (saveManager.Load(this))
            {
                value = saveManager.saveSerializator.Deserialize<T>(SerializedValue);
                if (tempValue != null && !value.Equals(tempValue))
                {
                    OnValueChange();
                }
            }
            else
            {
                saveManager.Save<T>(this, false);
            }
        }

        public void OnValueChange()
        {
            OnValueChangeHandler?.Invoke(Value);
        }

        public void SetValueWithoutSave(T value)
        {
            this.value = value;
            SerializedValue = saveManager.saveSerializator.Serialize(value);
            OnValueChange();
        }

        public void SetSerializedValueWithoutSave(string value)
        {
            SerializedValue = value;
        }

        public void Remove()
        {
            saveManager.Remove<T>(this);
        }
    }
}