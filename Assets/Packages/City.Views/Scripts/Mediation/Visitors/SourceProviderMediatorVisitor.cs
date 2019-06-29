using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public class SourceProviderMediatorVisitor<TView> : MediatorVisitor<TView>, IComponentSourceProvider<TView>
        where TView : Component, IView
    {
        protected IComponentProvisionService ComponentProvisionService { get; private set; }
        protected IProviderWithSource<TView>[] ComponentProviders { get; private set; }

        [Inject]
        public void Init(IComponentProvisionService componentProvisionService, IProviderWithSource<TView>[] componentProviders)
        {
            ComponentProvisionService = componentProvisionService;
            ComponentProviders = componentProviders;
        }

        public override void Initialize()
        {
            for (var i = 0; i < ComponentProviders.Length; i++)
            {
                ComponentProviders[i].InitSource(View);
                ComponentProvisionService.Bind(ComponentProviders[i]);
            }
        }
        
        public override void Dispose()
        {
            for (var i = 0; i < ComponentProviders.Length; i++)
                ComponentProvisionService.Unbind(ComponentProviders[i]);
        }
    }
}