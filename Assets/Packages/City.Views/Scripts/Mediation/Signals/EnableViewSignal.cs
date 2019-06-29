using City.Common.Signals;
using UnityEngine;

namespace City.Views
{
    public class EnableViewSignal<T>: ISignal
        where T: Component
    {}
}