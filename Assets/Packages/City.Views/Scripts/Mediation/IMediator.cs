using System;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public interface IMediator: IInitializable, IDisposable
    {
        void Enable();
        void Disable();
    }

    public interface IMediator<TView> : IMediator
        where TView: Component, IView
    {
        TView View { get; }
        
        [Inject]
        void Init(TView view);
        
        [Inject]
        void Visit(IMediatorVisitor<TView>[] visitors);
    }
}