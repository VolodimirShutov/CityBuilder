using System.Collections.Generic;
using Common.SoundManager.Config;
using UnityEngine;
using Zenject;

namespace Common.SoundManager.Initializators
{
    public class SoundsConfigsInitializator : MonoBehaviour
    {
        [SerializeField] private List<SoundsConfig> soundsConfigs;

        private ISoundManager _soundController;
        
        [Inject]
        public void Init(ISoundManager soundController)
        {
            _soundController = soundController;
            foreach (SoundsConfig soundsConfig in soundsConfigs)
            {
                soundController.AddSoundsConfig(soundsConfig.sounds);
            }
            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            foreach (SoundsConfig soundsConfig in soundsConfigs)
            {
                _soundController.RemoveSoundsConfig(soundsConfig.sounds);
            }
        }
    }
}