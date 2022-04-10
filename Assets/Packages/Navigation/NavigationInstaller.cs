using Packages.Navigation.CameraControl;
using Packages.Navigation.CanvasRayBlocker;
using Packages.Navigation.MousePosition;
using Packages.Navigation.SelectionBuildPlace;
using ShootCommon.Views.Mediation;
using Zenject;

namespace Packages.Navigation
{
    public class NavigationInstaller: Installer<NavigationInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<CameraControlView, CameraControlMediator>();
            Container.BindViewToMediator<CanvasRayBlockerView, CanvasRayBlockerMediator>();
            Container.BindViewToMediator<SelectionBuildPlaceView, SelectionBuildPlaceMediator>();
            Container.BindViewToMediator<FieldMousePositionView, FieldMousePositionMediator>();
        }
    }
}