using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace City.Common.Utils
{
    public static class ObservableTriggerExtensions
    {
        public static IObservable<PointerEventData> OnPointerClickAsObservable(this GameObject component)
        {
            if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
            return GetOrAddComponent<ObservablePointerClickTrigger>(component.gameObject).OnPointerClickAsObservable();
        }


        public static IObservable<PointerEventData> OnPointerClickAsObservable(this MonoBehaviour component)
        {
            return OnPointerClickAsObservable(component.gameObject);
        }
        
        static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
    
    
}