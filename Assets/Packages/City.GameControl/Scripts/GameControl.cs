using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace City.GameControl
{
    public class GameControl : IGameControl
    {
        private ISignalService _signalService;
        private IBuildingInfoContainer _buildingInfoContainer;
        
        private readonly FieldControl _fieldControl = new FieldControl();
        private readonly FinanceControl _financeControl = new FinanceControl();
        private readonly BuildingsControl _buildingsControl = new BuildingsControl();
        private CompositeDisposable DisposeOnDestroy { get; } = new CompositeDisposable();
        
        [Inject]
        public void Init(
            ISignalService signalService,
            IBuildingInfoContainer buildingInfoContainer)
        {
            _signalService = signalService;
            _buildingInfoContainer = buildingInfoContainer;

            _fieldControl.InitField();
            InitFinanceControl();

            _buildingsControl.ProductionIsFinishedAction += ProductionIsFinished;
            
            _signalService.Receive<TryBuildSignal>()
                .Subscribe(TryBuild).AddTo(DisposeOnDestroy);
            _signalService.Receive<OnStartBuildingProductionSignal>()
                .Subscribe(OnStartBuildingProductionProgress).AddTo(DisposeOnDestroy);
        }

        private void ProductionIsFinished(int Id, BuildingInfoModel model)
        {
            _financeControl.AddGold(model.GoldProduction);
            _financeControl.AddWood(model.WoodProduction);
            _financeControl.AddIron(model.IronProduction);
        }
        
        private void TryBuild(TryBuildSignal signal)
        {
            OnBuild(signal.Value.BuildingInfo, signal.Value.X,-signal.Value.Y);
        }

        private void InitFinanceControl()
        {
            _financeControl.GoldUpdateAction += GoldUpdate;
            _financeControl.WoodUpdateAction += WoodUpdate;
            _financeControl.IronUpdateAction += IronUpdate;
        }

        private void GoldUpdate()
        {
            _signalService.Publish(new UpdateGoldSignal()
            {
                Value = _financeControl.GetCurrentGold()
            });
        }

        private void WoodUpdate()
        {
            _signalService.Publish(new UpdateWoodSignal()
            {
                Value = _financeControl.GetCurrentWood()
            });
        }

        private void IronUpdate()
        {
            _signalService.Publish(new UpdateIronSignal()
            {
                Value = _financeControl.GetCurrentIron()
            });
        }
        
        public bool CanBuild(BuildingInfoModel building, int xPosition, int yPosition)
        {
            if (!_fieldControl.CanBuild(building, xPosition, yPosition))
                return false;
            if (!_financeControl.CanBuildCheck(building))
                return false;
            return true;
        }

        public bool OnBuild(BuildingInfoModel building, int xPosition, int yPosition)
        {
            bool canBuild = CanBuild(building, xPosition, yPosition);
            if (canBuild)
            {
                _fieldControl.OnBuild(building, xPosition, yPosition);
                _financeControl.OnBuild(building);
                int id = _buildingsControl.BuildNew(building);
                
                BigBuildingModel model = new BigBuildingModel()
                {
                    X = xPosition,
                    Y = - yPosition,
                    BuildingInfo = building,
                    Id = id
                };
                
                _signalService.Publish(new CreateBuildingViewSignal()
                {
                    Value = model
                });
            }
            
            return canBuild;
        }

        public long GetCurrentGold()
        {
            return _financeControl.GetCurrentGold();
        }

        public long GetCurrentWood()
        {
            return _financeControl.GetCurrentWood();
        }

        public long GetCurrentIron()
        {
            return _financeControl.GetCurrentIron();
        }

        public float GetBuildingProductionProgress(int Id)
        {
            return _buildingsControl.GetBuildingProductionProgress(Id);
        }
        
        public bool GetBuildingIsWorking(int Id)
        {
            return _buildingsControl.GetBuildingIsWorking(Id);
        }
        
        private void OnStartBuildingProductionProgress(OnStartBuildingProductionSignal signal)
        {
            _buildingsControl.OnStartBuildingProduction(signal.Value);
        }
    }
    
    public class UpdateGoldSignal : AsyncSignal<long>{}
    public class UpdateWoodSignal : AsyncSignal<long>{}
    public class UpdateIronSignal : AsyncSignal<long>{}
    
    
    public class OnStartBuildingProductionSignal : AsyncSignal<int>{}
    
    public class CreateBuildingViewSignal : AsyncSignal<BigBuildingModel>{}

    public class TryBuildSignal : AsyncSignal<BigBuildingModel>{}

    public class BigBuildingModel
    {
        public int X;
        public int Y;
        public int Id;
        public BuildingInfoModel BuildingInfo;
        public GameObject BuildingGameObject;
    }
}