using ShootCommon.Views.Mediation;
using UnityEngine;
using UnityEngine.UI;

namespace City.Views.Hud
{
    public class HudView : View
    {
        [SerializeField] private Text _gold;
        [SerializeField] private Text _wood;
        [SerializeField] private Text _iron;

        public void UpdateGold(long value)
        {
            _gold.text = value.ToString();
        }
        
        public void UpdateWood(long value)
        {
            _wood.text = value.ToString();
        }
        
        public void UpdateIron(long value)
        {
            _iron.text = value.ToString();
        }
    }
}