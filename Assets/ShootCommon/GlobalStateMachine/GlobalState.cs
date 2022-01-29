using System;
using ShootCommon.GlobalStateMachine.Signals;
using ShootCommon.GlobalStateMachine.States;
using ShootCommon.Signals;
using Stateless;
using UniRx;
using UnityEngine;
using Zenject;

namespace ShootCommon.GlobalStateMachine
{
    public class GlobalState : IState, IInitializable, IDisposable
    {
        protected CompositeDisposable DisposeOnExit { get; private set; }
        protected ISignalService SignalService { get; private set; }
        private StateMachine<IState, StateMachineTriggers>.StateConfiguration _stateConfiguration;
        private IStateMachineController _stateMachine;

        protected StateMachineTriggers GetCurrentState => _stateMachine.CurrentState;
        
        [Inject]
        public void Init(ISignalService signalService,
            IStateMachineController stateMachine)
        {
            SignalService = signalService;
            _stateMachine = stateMachine;
        }

        public virtual void Initialize()
        {
            _stateConfiguration = _stateMachine.Configure(this)
                .OnEntry(OnEntryInternal)
                .OnExit(OnExitInternal);
            Configure();
        }

        public virtual void Dispose()
        {
            OnExitInternal();            
        }

        protected virtual void Configure()
        {
            
        }

        private void OnEntryInternal(StateMachine<IState, StateMachineTriggers>.Transition transition)
        {
            DisposeOnExit = new CompositeDisposable();
            OnEntry(transition);
        }
        
        private void OnExitInternal()
        {
            OnExit();
            DisposeOnExit?.Dispose();
        }
        
        protected virtual void OnEntry(StateMachine<IState, StateMachineTriggers>.Transition transition = null)
        {
        }
        
        protected virtual void OnExit()
        {
        }
        
        protected void Fire(StateMachineTriggers trigger)
        {
            Debug.Log(" Fire " + trigger);
            _stateMachine.FireState(trigger);
            SignalService.Publish(new ChangeStateSignal(){SelectedState = GetCurrentState});
        }

        protected void Permit<TState>(StateMachineTriggers trigger)
            where TState: IState
        {
            _stateConfiguration.Permit(trigger, _stateMachine.GetState<TState>());
        }
        
        protected void SubstateOf<TState>() where TState : IState
        {
            _stateConfiguration.SubstateOf(_stateMachine.GetState<TState>());
        }
        
        protected void SubscribeToSignal<T>(Action<T> onSignalTriggered) where T : ISignal
        {
            SignalService.Receive<T>().Subscribe(onSignalTriggered).AddTo(DisposeOnExit);
        }
        
    }
}