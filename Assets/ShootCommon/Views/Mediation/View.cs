using UnityEngine;
using Zenject;

namespace ShootCommon.Views.Mediation
{
    public class View: MonoBehaviour, IView
    {
        private IMediator _mediator;

        public GameObject GetGameObject => this.gameObject;

        [Inject]
        public void Init(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected virtual void OnEnable() => _mediator?.Enable();

        protected virtual void OnDisable() => _mediator?.Disable();

        protected virtual void OnDestroy() => _mediator?.Dispose();
    }
}