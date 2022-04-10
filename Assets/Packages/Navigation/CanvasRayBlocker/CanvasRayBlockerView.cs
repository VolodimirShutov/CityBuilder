using System;
using ShootCommon.Views.Mediation;
using UnityEngine.EventSystems;

namespace Packages.Navigation.CanvasRayBlocker
{
    public class CanvasRayBlockerView : View, IPointerExitHandler, IPointerEnterHandler
    {
        public Action EnterAction;
        public Action ExitAction;

        public string keyName;

        public void Start()
        {
            if(keyName == null)
                keyName = name + "_" + this.gameObject.GetInstanceID();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnterAction?.Invoke();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ExitAction?.Invoke();
        }
    }
}