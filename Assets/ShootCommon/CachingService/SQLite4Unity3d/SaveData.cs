using System;
using SQLite4Unity3d.Interfaces;

namespace SQLite4Unity3d
{
    public class SaveData<T> : ISaveData<T>, IDataBind<T>, IInitbleSaveData
    {
        public event Action<T> OnValueChangeHandler;
        protected int id;
        protected string tableName;
        protected string key;
        protected T value;

        private ISaveManager _saveManager;
        
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (string.IsNullOrEmpty(Key))
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

        public virtual T Value
        {
            get => value;
            set
            {
                if (value != null && !value.Equals(this.value))
                {
                    this.value = value;
                    OnValueChange();
                    _saveManager.Save<T>(this, true);

                    return;
                }

                this.value = value;
            }
        }
        
        public void Initialize(ISaveManager saveManager)
        {
            _saveManager = saveManager;
        }
        
        public void UpdateValue(T value)
        {
            if (value != null && !value.Equals(this.value))
            {
                this.value = value;
                OnValueChange();
                UpdateValue();

                return;
            }
        }

        public void UpdateValue()
        {
            _saveManager.Save<T>(this, true);
        }

        public void SetValueWithoutSave(T value)
        {
            this.value = value;
            OnValueChange();
        }
        
        public SaveData(string key, string tableName)
        {
            this.tableName = tableName;
            this.key = key;
            Load();
        }
        
        public void Load()
        {
            var tempValue = value;
            if (_saveManager.Load(this))
            {
                if (tempValue != null && value != null && !tempValue.Equals(value))
                {
                    OnValueChange();
                }
            }
            else
            {
                _saveManager.Save<T>(this, false);
            }
        }

        public void OnValueChange()
        {
            OnValueChangeHandler?.Invoke(Value);
        }

        public void Remove()
        {
            _saveManager.Remove<T>(this);
        }
    }
}