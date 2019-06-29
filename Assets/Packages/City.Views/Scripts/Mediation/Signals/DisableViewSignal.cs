using City.Common.Signals;
using UnityEngine;

namespace City.Views
{
    public class DisableViewSignal<T>: ISignal
        where T: Component
    {}
}