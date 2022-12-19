using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Common.SoundManager.Initializators
{
    public class AudioInitializator : MonoBehaviour
    {
        [SerializeField] private AudioMixer masterMixer;

        [Inject]
        public void Init(ISoundManager soundController)
        {
            soundController.SetDefaultAudioMixer = masterMixer;
        }
    }
}