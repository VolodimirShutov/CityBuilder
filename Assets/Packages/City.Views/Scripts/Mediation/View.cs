using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public class View: MonoBehaviour, IView
    {
        private IMediator _mediator;

        [Inject]
        public void Init(IMediator mediator) => _mediator = mediator;

        protected virtual void OnEnable() => _mediator?.Enable();

        protected virtual void OnDisable() => _mediator?.Disable();

        protected virtual void OnDestroy() => _mediator?.Dispose();
    }
}