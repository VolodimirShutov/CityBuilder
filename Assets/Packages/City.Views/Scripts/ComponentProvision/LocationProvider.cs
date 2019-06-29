using UnityEngine;

namespace City.Views
{
    public class LocationProvider<TSource> : ComponentProvider<Transform,TSource>, ILocationProvider
        where TSource : class
    {
    }
}