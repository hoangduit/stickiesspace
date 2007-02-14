using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace StickyWindow
{
    public class MySlider : Slider
    {
        public Visibility MyVisibility
        {
            get
            {
                if ((double)this.GetValue(Canvas.RightProperty) >= -5)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;

            }
            set
            {
                switch (value)
                {
                    case Visibility.Visible:
                        if ((double)this.GetValue(Canvas.RightProperty) < -5)
                            AnimateSliderPeek(this, value);
                        break;
                    case Visibility.Hidden:
                        if ((double)this.GetValue(Canvas.RightProperty) >= -5)
                            AnimateSliderPeek(this, value);
                        break;
                }

            }
        }

        private void AnimateSliderPeek(MySlider mySlider, Visibility vis)
        {
            switch (vis)
            {
                case Visibility.Visible:
                    mySlider.Visibility = Visibility.Visible;
                    mySlider.BeginAnimation(Canvas.RightProperty,
                        new DoubleAnimation(-20, -5, new TimeSpan(0, 0, 0, 0, 500)));
                    break;

                case Visibility.Hidden:
                    mySlider.BeginAnimation(Canvas.RightProperty,
                        new DoubleAnimation((double)this.GetValue(Canvas.RightProperty), -20, new TimeSpan(0, 0, 0, 0, 500)));
                    break;
            }
        }
    }
}