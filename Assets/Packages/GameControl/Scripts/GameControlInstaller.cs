using City.Views;
using Zenject;

namespace City.GameControl
{
    public class GameControlInstaller : Installer<GameControlInstaller>
    {        
        public override void InstallBindings()
        {
            Container.Bind<IGameControl>().To<GameControl>().AsSingle();
        }
    }
}