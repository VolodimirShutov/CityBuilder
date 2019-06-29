using System;
using UniRx;
using UnityEngine;

namespace City.Common.Utils
{
    public static class ObservableExtensions
    {
        public static IObservable<T> DoOnCompletedOrOnError<T>(this IObservable<T> observable, Action action)
        {
            return observable.DoOnError(exception =>
                {
                    Debug.Log(exception);
                    action?.Invoke();
                })
                .DoOnCompleted(action);
        }
    }
}