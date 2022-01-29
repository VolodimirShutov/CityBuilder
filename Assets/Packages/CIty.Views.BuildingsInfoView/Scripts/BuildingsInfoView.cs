using City.Views;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public class BuildingsInfoView : View
    {
        [SerializeField] private BuildingInfoModel[] _buildings;

        public BuildingInfoModel[] Buildings => _buildings;
    }
}