using UnityEngine;

namespace City.Views
{
    public class DisableViewOnInitializationMediatorVisitor<TView>: MediatorVisitor<TView>
        where TView: Component, IView
    {
        public override void Initialize()
        {
            View.gameObject.SetActive(false);
        }
    }
}