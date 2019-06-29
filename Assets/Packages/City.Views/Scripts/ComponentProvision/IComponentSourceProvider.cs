using Zenject;

namespace City.Views
{
    public interface IComponentSourceProvider<TSource>
        where TSource: class
    {
        [Inject]
        void Init(IComponentProvisionService componentProvisionService, IProviderWithSource<TSource>[] componentProviders);
    }
}