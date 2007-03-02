using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;

namespace StickyWindow
{
    public class StoryboardExtender : Storyboard
    {

        private UIElement _target;

        public UIElement TargetElement
        {
            get { return _target; }
            set { _target = value; }
        } 


        protected override Freezable CreateInstanceCore()
        {
            StoryboardExtender p = new StoryboardExtender();
            p.TargetElement = this.TargetElement;
            return p;
        }

    }
}
