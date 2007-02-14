using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StickyWindow;
using System.Windows.Media.Animation;

namespace stickiesSpace
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        StickyWindowCommands commands = new StickyWindowCommands();

        public Window1()
        {
            InitializeComponent();
        }


        public void CreateStickyWindow(object sender, EventArgs e)
        {

            #region Window Constructor

            StickyWindowModel stickyWindow = new StickyWindowModel();
            stickyWindow.Name = "stickyWindow";
            stickyWindow.Height = 200;
            stickyWindow.Width = 200;
            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.MyWindowState = WindowState.Normal;
            stickyWindow.Background = Brushes.Transparent;
            stickyWindow.AllowsTransparency = true;
            stickyWindow.WindowStyle = WindowStyle.None;


            stickyWindow.Show();

            #endregion


            #region UIElement grabbers

            Canvas container = stickyWindow.sContainer;
            Border border = stickyWindow.sBorder;
            Ellipse contextCircle = stickyWindow.sContextCircle;
            MyScrollViewer scroller = stickyWindow.sScroller;
            TextBox txt = stickyWindow.sTextArea;
            MySlider slider = stickyWindow.sSlider;

            #endregion


            #region Event Wireup

            stickyWindow.MouseLeftButtonDown += new MouseButtonEventHandler(stickyWindow_MouseLeftButtonDown);
            stickyWindow.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            slider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            slider.MouseLeave += new MouseEventHandler(slider_MouseLeave);

            #endregion


            contextCircle.ContextMenu = commands.GetContextMenu(stickyWindow);

        }




        #region Events

        void slider_MouseLeave(object sender, MouseEventArgs e)
        {
            MySlider slider = (MySlider)sender;
            AnimateSlider(slider, SliderAnimateMode.Hide);
            e.Handled = true;
        }

        void slider_MouseEnter(object sender, MouseEventArgs e)
        {
            MySlider slider = (MySlider)sender;
            AnimateSlider(slider, SliderAnimateMode.Show);
            e.Handled = true;
        }

        void scroller_ScrollChanged(object sender, RoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)sender;
            HandleScrollChange(stickyWindow);
            e.Handled = true;
        }

        public void stickyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)sender;
            stickyWindow.DragMove();
        }

        #endregion


        #region Slider Helpers

        private void HandleScrollChange(StickyWindowModel stickyWindow)
        {
            MySlider slider = stickyWindow.sSlider;
            MyScrollViewer scroller = stickyWindow.sScroller;

            if (scroller.ExtentHeight > scroller.Height && stickyWindow.MyWindowState == WindowState.Normal)
                slider.MyVisibility = Visibility.Visible;
            else
                slider.MyVisibility = Visibility.Hidden;

            slider.Value = scroller.MyVerticalOffset;
            slider.Maximum = scroller.ExtentHeight;

            lblHit.Content = String.Format("{0}, {1}", slider.Maximum, scroller.ExtentHeight);
            lblHit2.Content = String.Format("{0}", slider.Value);
        }

        private void AnimateSlider(MySlider slider, SliderAnimateMode sliderAnimateMode)
        {
            double slideFrom = 0;
            double slideTo = 0;

            switch (sliderAnimateMode)
            {
                case SliderAnimateMode.Show:
                    slideFrom = (double)slider.GetValue(Canvas.RightProperty);
                    slideTo = 0;
                    break;

                case SliderAnimateMode.Hide:
                    slideFrom = (double)slider.GetValue(Canvas.RightProperty);
                    slideTo = -5;
                    break;
            }

            slider.BeginAnimation(Canvas.RightProperty,
                new DoubleAnimation(slideFrom, slideTo, new TimeSpan(0, 0, 0, 0, 250)));
        }

        #endregion



    }
}