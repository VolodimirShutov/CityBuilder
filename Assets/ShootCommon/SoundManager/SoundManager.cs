using System;
using System.Collections.Generic;
using System.Linq;
using Common.SoundManager.Config;
using ShootCommon.AssetReferences;
using UnityEngine.Audio;

namespace Common.SoundManager
{
    public class SoundManager : ISoundManager
    {
        
        private Dictionary<string, AudioClipModel> _sounds = new Dictionary<string, AudioClipModel>();
        private AudioMixer _defaultAudioMixer;
        private IAssetReferenceDownloader _assetReferenceDownloader;

        public AudioMixer SetDefaultAudioMixer
        {
            set => _defaultAudioMixer = value;
        }
        
        public void AddSoundsConfig (List<SoundConfigModel> models)
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
            if (_sounds != null && _sounds.ContainsKey(clipName))
            {
                AudioClipModel clipModel = _sounds[clipName];
                PlayAudioClip(clipModel, onDownloadComplete);
            }
            else
            {
                CreateAudioClipModel(clipName, onDownloadComplete);
            }
        }

        public void CreateAudioClipModel(string clipName, Action<AudioClipModel> onDownloadComplete )
        {
            
        }
        
        public void PlayAudioClip(AudioClipModel clipInfo, Action<AudioClipModel> onDownloadComplete = null)
        {
            /*
            AudioClipConfig clip;
            if (_soundsConfig.ClipsDictionary.TryGetValue(clipInfo.ClipName, out clip))
            {
                if (clip.Reference != null)
                {
                    PlayExistedAudioClip(clipInfo, clip);
                    return;
                }
            }

            _assetReferenceDownloader.DownloadAudioById(clipInfo.ClipName, model =>
            {
                if (model == null)
                {
                    Debug.LogError($"No clip config found for {clipInfo.ClipName} audio clip type");
                    return;
                }
                
                PlayExistedAudioClip(clipInfo, new AudioClipConfig()
                {
                    Reference = model,
                    OutputAudioMixerGroup = EffectsOutputGroup
                });
                
                onDownloadComplete?.Invoke();
            });
            */
        }
        

    }
}