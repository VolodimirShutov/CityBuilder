using UnityEngine;

namespace City.Views
{
    public class CameraProvider<TSource> : ComponentProvider<Camera,TSource>, ICameraProvider
        where TSource : class
    {
    }
}