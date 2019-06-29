
using System;
using City.Views;

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