using City.Common.Signals;
using UnityEngine;

namespace City.Views
{
    public static class SignalServiceExtensions
    {
        public static void EnableView<TView>(this ISignalService signalService)
            where TView : Component =>
            signalService.Publish<EnableViewSignal<TView>>();
        
        public static void DisableView<TView>(this ISignalService signalService)
            where TView : Component =>
            signalService.Publish<DisableViewSignal<TView>>();
        
        public static void SetViewActive<TView>(this ISignalService signalService, bool active)
            where TView: Component
        {
            if(active)
                signalService.Publish<EnableViewSignal<TView>>();
            else
                signalService.Publish<DisableViewSignal<TView>>();
        }
    }
}