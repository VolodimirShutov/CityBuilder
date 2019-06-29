using City.Common.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public abstract class Mediator: IMediator
    {
        protected SignalBus SignalBus { get; private set; }
        protected ISignalService SignalService { get; private set; }
        protected CompositeDisposable DisposeOnDestroy { get; } = new CompositeDisposable();
        protected CompositeDisposable DisposeOnDisable { get; } = new CompositeDisposable();
        
        private DisposableManager _disposableManager;
        private bool _isDisposed;
        private bool _isInitialized;
        private bool _isStarted;

        [Inject]
        public void Init(
            SignalBus signalBus, 
            DisposableManager disposableManager,
            ISignalService signalService
            )
        {
            SignalBus = signalBus;
            SignalService = signalService;
            
            _disposableManager = disposableManager;
        }
        
        public virtual void Initialize()
        {
            OnMediatorInitialize();
            _isInitialized = true;
        }

        public void Dispose()
        {
            if(_isDisposed)
                return;
            
            OnMediatorDispose();
            
            _disposableManager.Remove(this);
            
            DisposeOnDestroy.Dispose();
            
            _isDisposed = true;
            Debug.Log($"Mediator {this.GetType().Name} disposed");
        }

        public void Enable()
        {
            if(!_isInitialized)
                return;
            
            OnMediatorEnable();
            
            if (_isStarted)
                return;
            
            OnMediatorStart();
            _isStarted = true;
        }

        public void Disable()
        {
            if(!_isInitialized)
                return;
            
            OnMediatorDisable();
            DisposeOnDisable.Dispose();
        }
        
        protected virtual void OnMediatorInitialize() {}
        protected virtual void OnMediatorDispose() {}
        protected virtual void OnMediatorStart() {}
        protected virtual void OnMediatorEnable() {}
        protected virtual void OnMediatorDisable() {}
    }
    
    public abstract class Mediator<TView>: Mediator, IMediator<TView>
        where TView : Component, IView
    {
        public TView View { get; private set; }
       

        private IMediatorVisitor<TView>[] _visitors;

        [Inject]
        public void Init(TView view)
        {
            Debug.Log($"Injected {view.GetType().Name} to {this.GetType().Name}");
            View = view;
        }
        
        [Inject]
        public void Visit(IMediatorVisitor<TView>[] visitors)
        {
            Debug.Log($"Injected {View.GetType().Name} with {(visitors == null ? 0 : visitors.Length)} visitors");
            _visitors = visitors;
        }

        public sealed override void Initialize()
        {
            InitializeVisitors();

            base.Initialize();   
            
            SignalService.Receive<EnableViewSignal<TView>>()
                .Do(_ => Debug.Log($"ShowView for {typeof(TView).Name}"))
                .Subscribe(_ => EnableView()).AddTo(DisposeOnDestroy);

            SignalService.Receive<DisableViewSignal<TView>>()
                .Do(_ => Debug.Log($"HideView for {typeof(TView).Name}"))
                .Subscribe(_ => DisableView()).AddTo(DisposeOnDestroy);

            if(View.gameObject.activeInHierarchy)
                Enable();
        }
        
        private void EnableView() => View.gameObject.SetActive(true);
        private void DisableView() => View.gameObject.SetActive(false);
        
        private void InitializeVisitors()
        {
            for (var i = 0; i != _visitors.Length; i++)
            {
                _visitors[i].Initialize();
                DisposeOnDestroy.Add(_visitors[i]);
            }
        }
    }
}