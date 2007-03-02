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


        public void HandleScrollChange(StickyWindowModel stickyWindow)
        {
            MyScrollViewer scroller = stickyWindow.sScroller;

            if (scroller.ExtentHeight > scroller.Height && stickyWindow.MyWindowState == WindowState.Normal)
                this.MyVisibility = Visibility.Visible;
            else
                this.MyVisibility = Visibility.Hidden;

            this.Value = scroller.MyVerticalOffset;
            this.Maximum = scroller.ExtentHeight;
        }


        public void AnimateSlider(SliderAnimateMode sliderAnimateMode)
        {
            double slideFrom = 0;
            double slideTo = 0;

            switch (sliderAnimateMode)
            {
                case SliderAnimateMode.Show:
                    slideFrom = (double)this.GetValue(Canvas.RightProperty);
                    slideTo = 0;
                    break;

                case SliderAnimateMode.Hide:
                    slideFrom = (double)this.GetValue(Canvas.RightProperty);
                    slideTo = -5;
                    break;
            }

            this.BeginAnimation(Canvas.RightProperty,
                new DoubleAnimation(slideFrom, slideTo, new TimeSpan(0, 0, 0, 0, 250)));
        }

    }
}