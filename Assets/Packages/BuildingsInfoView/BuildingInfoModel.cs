using System;
using UnityEngine;

namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    [Serializable]
    public class BuildingInfoModel
    {
        public int Id;
        public GameObject Prefab;
        public String Name;
        public int SizeX = 1;
        public int SizeY = 1;
        public int PriceGold = 0;
        public int PriceWood = 0;
        public int PriceIron = 0;

        public bool AutomaticProduction = false;
        public int GoldProduction = 0;
        public int WoodProduction = 0;
        public int IronProduction = 0;

        public float TimeProduction = 0;

    }
}