using Zenject;

namespace ShootCommon.AssetReferences
{
    public class AssetReferenceInstaller : Installer<AssetReferenceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AssetReferenceStorage>().AsSingle();
        }
    }
}