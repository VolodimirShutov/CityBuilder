using ShootCommon.Views.Mediation;
using Zenject;

namespace City.Views.Hud
{
    public class HudInstaller: Installer<HudInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<HudView, HudMediator>();
            //Container.BindPrefabToRoot<View, CanvasHudRoot>();
        }
    }
}