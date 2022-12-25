using ShootCommon.Signals;
using UnityEngine;
using Zenject;

namespace Common.SoundManager.SoundsUI
{
    public class PlaySoundEffect : MonoBehaviour
    {
        private ISignalService _signalService;
        
        [Inject]
        public void Init(ISignalService signalService)
        {
            _signalService = signalService;
        }
    }
}