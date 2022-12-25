using System;
using Common.SoundManager.Config;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.SoundManager
{
    [Serializable]
    public class AudioClipModel
    {
        public bool useAddressable;
        [ConditionalField(nameof(useAddressable), true)]public AudioClip audioClip;
        [ConditionalField(nameof(useAddressable), false)] public string addressableId;
        public float volume = 1;
        public SoundType soundType;
        public AudioMixerGroup audioMixerGroup;
        public float pitch = 1;

        public float AudioClipLength => audioClip == null ? 0 : audioClip.length;
        public AudioSource AudioSource { get; set; }

        public AudioClipModel Clone()
        {
            return new AudioClipModel()
            {
                useAddressable = this.useAddressable,
                audioClip = this.audioClip,
                addressableId = this.addressableId,
                volume = this.volume,
                soundType = this.soundType,
                audioMixerGroup = this.audioMixerGroup
            };
        }
    }
}