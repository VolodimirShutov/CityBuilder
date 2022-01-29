using System;
using Zenject;

namespace ShootCommon.Views.Mediation
{
    public interface IMediator: IInitializable, IDisposable
    {
        void Enable();
        void Disable();
    }

    public interface IMediator<TView> : IMediator
        where TView: IView
    {
        TView View { get; }
        
        [Inject]
        void Init(TView view);
        
        [Inject]
        void Visit(IMediatorVisitor<TView>[] visitors);
    }
}