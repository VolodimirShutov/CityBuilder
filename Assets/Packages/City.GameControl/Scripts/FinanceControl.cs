using System;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using UnityEngine;

namespace City.GameControl
{
    public class FinanceControl
    {
        private long _currentGold = 500;
        private long _currentWood = 0;
        private long _currentIron = 0;

        public Action GoldUpdateAction;
        public Action WoodUpdateAction;
        public Action IronUpdateAction;
        
        public long GetCurrentGold()
        {
            return _currentGold;
        }

        public long GetCurrentWood()
        {
            return _currentWood;
        }

        public long GetCurrentIron()
        {
            return _currentIron;
        }

        public long AddGold(long addValue)
        {
            _currentGold += addValue;
            GoldUpdated();
            return _currentGold;
        }
        
        public long AddWood(long addValue)
        {
            _currentWood += addValue;
            WoodUpdated();
            return _currentWood;
        }
        
        public long AddIron(long addValue)
        {
            _currentIron += addValue;
            IronUpdated();
            return _currentIron;
        }

        public bool RemoveGold(long value)
        {
            long _gold = _currentGold - value;
            if (_gold < 0)
            {
                _currentGold = 0;
                Debug.LogError("Check finance control! Problem with gold!!!");
                return false;
            }

            _currentGold = _gold;
            GoldUpdated();
            return true;
        }
        
        
        public bool RemoveWood(long value)
        {
            long _wood = _currentWood - value;
            if (_wood < 0)
            {
                _currentWood = 0;
                Debug.LogError("Check finance control! Problem with wood!!!");
                return false;
            }

            _currentWood = _wood;
            WoodUpdated();
            return true;
        }
        
        public bool RemoveIron(long value)
        {
            long _iron = _currentIron - value;
            if (_iron < 0)
            {
                _currentIron = 0;
                Debug.LogError("Check finance control! Problem with iron!!!");
                return false;
            }

            _currentIron = _iron;
            IronUpdated();
            return true;
        }

        public bool CanBuildCheck(BuildingInfoModel building)
        {
            if (_currentGold < building.PriceGold)
                return false;
            if (_currentWood < building.PriceWood)
                return false;
            if (_currentIron < building.PriceIron)
                return false;
            return true;
        }

        public void OnBuild(BuildingInfoModel building)
        {
            if (CanBuildCheck(building))
            {
                RemoveGold(building.PriceGold);
                RemoveWood(building.PriceWood);
                RemoveIron(building.PriceIron);
            }
        }
        
        private void GoldUpdated()
        {
            GoldUpdateAction?.Invoke();
        }
        
        private void WoodUpdated()
        {
            WoodUpdateAction?.Invoke();
        }
        
        private void IronUpdated()
        {
            IronUpdateAction?.Invoke();
        }
    }
}