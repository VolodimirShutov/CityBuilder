using System;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public interface IMediatorVisitor : IDisposable, IInitializable
    {
    }
    
    public interface IMediatorVisitor<TView>: IMediatorVisitor
        where TView: Component, IView
    {
        [Inject]
        void Init(IMediator<TView> mediator);
    }
}