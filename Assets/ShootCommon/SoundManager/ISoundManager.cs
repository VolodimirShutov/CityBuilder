using System.Collections.Generic;
using Common.SoundManager.Config;
using UnityEngine.Audio;

namespace Common.SoundManager
{
    public interface ISoundManager
    {

        public AudioMixer SetDefaultAudioMixer { set; }
        public void AddSoundsConfig(List<SoundConfigModel> models);
        public void RemoveSoundsConfig(List<SoundConfigModel> models);
    }
}