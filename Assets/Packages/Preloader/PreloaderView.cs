using ShootCommon.Views.Mediation;
using TMPro;
using UnityEngine;

namespace Packages.Preloader
{
    public class PreloaderView  : View
    {
        [SerializeField] private TextMeshProUGUI progressText;
        
        public void SetProgress(int value)
        {
            progressText.text = value.ToString();
        }
    }
}