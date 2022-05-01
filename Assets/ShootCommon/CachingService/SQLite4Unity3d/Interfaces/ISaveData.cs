
using System;

namespace SQLite4Unity3d.Interfaces
{
    public interface ISaveData<T>
    {
        int Id { get; set; }
        string Key { get; set; }
        string TableName { get; set; }
        T Value { get; set; }

        event Action<T> OnValueChangeHandler;

        void UpdateValue();
        void UpdateValue(T value);
        void SetValueWithoutSave(T value);
        void Load();
        void Remove();
        void OnValueChange();
    }
}