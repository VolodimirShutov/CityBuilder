using UnityEngine;
using Zenject;

namespace ShootCommon.Views.Mediation
{
    public class MediatorVisitor<TView>: IMediatorVisitor<TView>
        where TView : Component, IView
    {
        protected IMediator<TView> Mediator { get; private set; }
        protected TView View => Mediator.View;
        
        [Inject]
        public void Init(IMediator<TView> mediator)
        {
            Mediator = mediator;
        }
        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}