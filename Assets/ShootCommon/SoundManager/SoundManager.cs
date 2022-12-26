using System;
using System.Collections.Generic;
using System.Linq;
using Common.SoundManager.Config;
using Common.SoundManager.Signals;
using ShootCommon.AssetReferences;
using ShootCommon.InteractiveObjectsSpawnerService;
using ShootCommon.InteractiveObjectsSpawnerService.Containers;
using ShootCommon.Signals;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Object = UnityEngine.Object;

namespace Common.SoundManager
{
    public class SoundManager : ISoundManager
    {
        private const string EffectsMixerKey = "EffectsMixer";
        private const string MusicMixerKey = "MusicMixer";
        private const string VoiceMixerKey = "VoiceMixer";

        private AudioMixerGroup EffectsGroup { get; set; }
        private AudioMixerGroup MusicGroup { get; set; }
        private AudioMixerGroup VoiceGroup { get; set; }

        private Dictionary<string, AudioClipModel> _sounds = new Dictionary<string, AudioClipModel>();
        private AudioMixer _defaultAudioMixer;
        private IAssetReferenceDownloader _assetReferenceDownloader;
        private IInteractiveObjectsManager _interactiveObjectsManager;
        private readonly List<AudioSource> _activeAudioSources;

        private ISignalService _signalService;
        
        [Inject]
        public void Init(ISignalService signalService, IInteractiveObjectsManager interactiveObjectsManager)
        {
            _interactiveObjectsManager = interactiveObjectsManager;
            _signalService = signalService;
            _signalService.Receive<PlayAudioClipSignal>().Subscribe(PlayAddressableSound);
        }

        private void PlayAddressableSound(PlayAudioClipSignal signal)
        {
            PlayAudioClip(signal.ClipName, signal.OnDownloadComplete);
        }
        
        public AudioMixer SetDefaultAudioMixer
        {
            set
            {
                _defaultAudioMixer = value;
                EffectsGroup = value.FindMatchingGroups(EffectsMixerKey)[0];
                MusicGroup = value.FindMatchingGroups(MusicMixerKey)[0];
                VoiceGroup = value.FindMatchingGroups(VoiceMixerKey)[0];
            }
        }

        public void AddSoundsConfig(List<SoundConfigModel> models)
        {
            foreach (var model in models.Where(model => !_sounds.ContainsKey(model.clipName)))
            {
                _sounds.Add(model.clipName, model.clipModel);
            }
        }

        public void RemoveSoundsConfig(List<SoundConfigModel> models)
        {
            foreach (var model in models.Where(model => !_sounds.ContainsKey(model.clipName)))
            {
                _sounds.Remove(model.clipName);
            }
        }

        public void PlayAudioClip(string clipName, Action<AudioClipModel> onDownloadComplete = null)
        {
            AudioClipModel clipModel;
            if (_sounds != null && _sounds.ContainsKey(clipName))
            {
                clipModel = _sounds[clipName].Clone();
            }
            else
            {
                clipModel = CreateAudioClipModel(clipName);
            }
            PlayAudioClip(clipModel, onDownloadComplete);
        }

        public AudioClipModel CreateAudioClipModel(string clipName)
        {
            return new AudioClipModel
            {
                useAddressable = true,
                addressableId = clipName,
                audioMixerGroup = EffectsGroup,
                soundType = SoundType.Effects
            };
        }

        public void PlayAudioClip(AudioClipModel clipInfo, Action<AudioClipModel> onDownloadComplete = null)
        {
            if (!clipInfo.useAddressable)
            {
                if (clipInfo.audioClip != null)
                {
                    PlayAudioClipFactory(clipInfo);
                    onDownloadComplete?.Invoke(clipInfo);
                }
                else
                {
                    Debug.LogError($"Wrong configuration for clip");
                }
            }
            else
            {
                _assetReferenceDownloader.SpawnAudioById(clipInfo.addressableId, model =>
                {
                    if (model == null)
                        return;
                    clipInfo.audioClip = model;
                    PlayAudioClipFactory(clipInfo);
                });
            }
        }

        private void PlayAudioClipFactory(AudioClipModel clipInfo)
        {
            switch (clipInfo.soundType)
            {
                case SoundType.Effects:
                    CreateAndPlayGameAudioClip(clipInfo, false);
                    break;
                case SoundType.BackgroundMusic:
                    CreateAndPlayGameAudioClip(clipInfo, true);
                    break;
                case SoundType.Voice:
                    CreateAndPlayGameAudioClip(clipInfo, false);
                    break;
                default:
                    CreateAndPlayGameAudioClip(clipInfo, false);
                    break;
            }
        }

        private void CreateAndPlayGameAudioClip(AudioClipModel audioClipModel, bool isOnLoop = false)
        {
            AudioSource audioSource = new GameObject($"{audioClipModel.audioClip.name}").AddComponent<AudioSource>();
            
            audioSource.playOnAwake = false;
            audioSource.loop = isOnLoop;
            audioSource.spatialize = false;
            audioSource.clip = audioClipModel.audioClip;
            audioSource.volume = audioClipModel.volume; 
            audioSource.outputAudioMixerGroup = audioClipModel.audioMixerGroup;
            audioSource.pitch = audioClipModel.pitch;
            audioSource.Play();

            _activeAudioSources.Add(audioSource);
            
            if(!isOnLoop)
                DestroyClipOnFinish(audioClipModel);

            if (audioClipModel.is3dSound)
            {
                IInteractiveObjectContainer container = _interactiveObjectsManager.GetContainer(audioClipModel.containerName);
                if (container != null)
                {
                    container.AddItem(audioSource.gameObject);
                    audioSource.transform.localPosition = audioClipModel.position;
                }
            }
        }
        
        private void Delete2DAudioClip(AudioSource source) {
            if ( source == null || _activeAudioSources == null || _activeAudioSources.Count == 0)
                return;

            if (!_activeAudioSources.Contains(source))
                return;
            
            _activeAudioSources.Remove(source);
            Object.Destroy(source.gameObject);
        }
        
        private void DestroyClipOnFinish(AudioClipModel clipConfig)
        {
            Observable.Timer(TimeSpan.FromSeconds(clipConfig.AudioClipLength))
                .Subscribe(_ =>
                {
                    CompositeDisposable disposeOnExit = new CompositeDisposable();
                    Observable.Timer(TimeSpan.FromTicks(1))
                        .Repeat()
                        .Subscribe(_ =>
                        {
                            if (clipConfig.AudioSource != null && !clipConfig.AudioSource .isPlaying) {
                                _activeAudioSources.Remove(clipConfig.AudioSource);
                                Object.Destroy(clipConfig.AudioSource.gameObject);
                                disposeOnExit.Dispose();
                            }
                        }).AddTo(disposeOnExit);
                });
        }
    }
}