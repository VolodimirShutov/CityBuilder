using System;
using ShootCommon.Signals;

namespace Common.SoundManager.Signals
{
    public class PlayAudioClipSignal : Signal
    {
        public string ClipName;
        public Action<AudioClipModel> OnDownloadComplete;
    }
}