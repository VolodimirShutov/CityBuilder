
using System;
using ShootCommon.Views.Mediation;

namespace City.Common.ModePanel
{
    public class ModePanelView : View
    {
        
        public Action RegularMode;
        public Action BuildMode;

        public void RegularModeOnClick()
        {
            RegularMode?.Invoke();
        }
        
        public void BuildModeOnClick()
        {
            BuildMode?.Invoke();
        }
    }
}