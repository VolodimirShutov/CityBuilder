using UnityEngine;
using Zenject;

namespace City.Views
{
    public abstract class MediatorWithController<TView, TController> : Mediator<TView>
        where TView : Component, IView
        where TController : class
    {
        protected TController Controller { get; private set; }

        [Inject]
        public void Init(TController controller) => Controller = controller;
    }
}