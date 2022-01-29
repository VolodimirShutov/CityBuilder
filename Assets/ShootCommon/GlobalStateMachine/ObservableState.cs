using System;
using ModestTree;
using Stateless;
using UniRx;
using UnityEngine;

namespace ShootCommon.GlobalStateMachine
{
    public class ObservableState : GlobalState
    {
        private IObservable<Unit> _observable;

        public override void Initialize()
        {
            //Debug.Log("ObservableState Initialize");
            base.Initialize();
            
            _observable = CreateObservable();
        }
        
        protected override void OnEntry(StateMachine<IState, StateMachineTriggers>.Transition transition = null)
        {
            base.OnEntry(transition);
            
            _observable
                .DoOnError(exception => { })
                .Subscribe().AddTo(DisposeOnExit);
        }

        protected virtual IObservable<Unit> CreateObservable()
        {
            return null;
        }
    }
}