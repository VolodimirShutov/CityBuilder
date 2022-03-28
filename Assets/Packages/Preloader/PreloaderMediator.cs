using ShootCommon.AssetReferences.Signals;
using ShootCommon.Views.Mediation;
using UniRx;
using UnityEngine;
using Zenject;

namespace Packages.Preloader
{
    public class PreloaderMediator : Mediator<PreloaderView>
    {
        private int _maxProgress;
        
        [Inject]
        public void Init()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            SignalService.Receive<AssetDownloaderProgressSignal>().Subscribe(AssetDownloaderProgress)
                .AddTo(DisposeOnDestroy);
        }

        private void AssetDownloaderProgress(AssetDownloaderProgressSignal signal)
        {
            int progress = signal.Value;
            if (_maxProgress < progress)
                _maxProgress = progress;
            int value = Mathf.RoundToInt((_maxProgress - progress) / _maxProgress * 100);
            View.SetProgress(value);
        }
    }
}