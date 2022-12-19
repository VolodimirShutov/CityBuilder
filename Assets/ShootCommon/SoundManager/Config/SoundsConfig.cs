using System.Collections.Generic;
using UnityEngine;

namespace Common.SoundManager.Config
{
    [CreateAssetMenu(fileName = "Sounds Config", menuName = "ShootCommon/Audio")]
    public class SoundsConfig: ScriptableObject
    {
        [SerializeField] public List<SoundConfigModel> sounds;
    }
}