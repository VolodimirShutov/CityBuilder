using UnityEngine;
using Zenject;

namespace City.Views
{
    public class MonoMediatorWithController<TView, TController> : MonoMediator<TView>
        where TView : Component, IView
        where TController : class
    {
        protected TController Controller { get; private set; }

        [Inject]
        public void Init(TController controller) => Controller = controller;
    }
}