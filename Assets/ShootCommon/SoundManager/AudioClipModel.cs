using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.SoundManager
{
    [Serializable]
    public class AudioClipModel
    {
        public AudioClip AudioClip;
        public float Volume = 1;
        public AudioMixerGroup AudioMixerGroup;
    }
}