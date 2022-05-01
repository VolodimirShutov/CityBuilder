using ShootCommon.Views.Mediation;
using Zenject;

namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public class BuildingInfoInstaller : Installer<BuildingInfoInstaller>
    {
        
        public override void InstallBindings()
        {
            Container.Bind<IBuildingInfoContainer>().To<BuildingInfoContainer>().AsSingle();
            Container.BindViewToMediator<BuildingsInfoView, BuildingInfoMediator>();
        }
    }

    public class Facade
    {
        public ClassA _classA { set; private get; }
        public ClassB _classB{ set; private get; }
        public ClassC _classC{ set; private get; }

        public void Action1()
        {
            //...
        }
        
        public void Action2()
        {
            //...
        }
    }

    public class ClassA
    {
        
    }
    public class ClassB
    {
        
    }
    
    public class ClassC
    {
        
    }
}