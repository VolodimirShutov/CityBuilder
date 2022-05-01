using System;

namespace SQLite4Unity3d.Interfaces
{
    public interface IDataBind<T>
    {
        event Action<T> OnValueChangeHandler;

        T Value { get; set; }
    }
}