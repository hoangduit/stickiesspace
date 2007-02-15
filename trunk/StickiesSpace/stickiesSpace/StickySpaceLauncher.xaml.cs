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
        StickyWindowAnimations animations = new StickyWindowAnimations();

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
            stickyWindow.SetContainerCanvasBindings(SetBindingMode.SetBinding);

            #endregion


            #region UIElement grabbers

            Canvas container = stickyWindow.sContainer;
            Border border = stickyWindow.sBorder;
            Ellipse contextCircle = stickyWindow.sContextCircle;
            MyScrollViewer scroller = stickyWindow.sScroller;
            MyTextBox txt = stickyWindow.sTextArea;
            MySlider slider = stickyWindow.sSlider;

            #endregion


            #region Event Wireup

            slider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            slider.MouseLeave += new MouseEventHandler(slider_MouseLeave);

            stickyWindow.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            stickyWindow.MouseLeftButtonUp += new MouseButtonEventHandler(stickyWindow_MouseLeftButtonUp);
            stickyWindow.MouseLeftButtonDown += new MouseButtonEventHandler(stickyWindow_MouseLeftButtonDown);

            contextCircle.MouseLeftButtonDown += new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown);

            txt.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            txt.MouseDoubleClick += new MouseButtonEventHandler(txt_MouseDoubleClick);

            #endregion


            contextCircle.ContextMenu = commands.GetContextMenu(stickyWindow);

        }




        #region Events

        void stickyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StickyWindowModel stickyWindow = new StickyWindowModel();

            if (sender.GetType() == typeof(StickyWindowModel))
            {
                stickyWindow = sender as StickyWindowModel;
            }
            else if (sender.GetType() == typeof(MyTextBox))
            {
                MyTextBox txt = sender as MyTextBox;
                stickyWindow = txt.TemplatedParent as StickyWindowModel;
            }

            stickyWindow.StartDrag();
        }

        void stickyWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lblHit.Content += ";";

            StickyWindowModel stickyWindow = new StickyWindowModel();

            if (sender.GetType() == typeof(StickyWindowModel))
            {
                stickyWindow = sender as StickyWindowModel;
            }
            else if (sender.GetType() == typeof(MyTextBox))
            {
                MyTextBox txt = sender as MyTextBox;
                stickyWindow = txt.TemplatedParent as StickyWindowModel;
            }

            stickyWindow.ReleaseDrag();
        }

        
        void txt_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MyTextBox txt = sender as MyTextBox;
            txt.ActiveState = TextBoxActiveState.Inactive;
        }

        void txt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MyTextBox txt = sender as MyTextBox;
            txt.ActiveState = TextBoxActiveState.Active;
        }

        void contextCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Ellipse contextCircle = (Ellipse)sender as Ellipse;
                StickyWindowModel stickyWindow = contextCircle.TemplatedParent as StickyWindowModel;

                switch (stickyWindow.MyWindowState)
                {
                    case WindowState.Minimized:
                        animations.RestoreAnimation(stickyWindow);
                        break;

                    case WindowState.Normal:
                        animations.MinimizeAnimation(stickyWindow);
                        break;
                }
            }
            e.Handled = true;
        }

        void slider_MouseLeave(object sender, MouseEventArgs e)
        {
            MySlider slider = (MySlider)sender;
            slider.AnimateSlider(SliderAnimateMode.Hide);
            e.Handled = true;
        }

        void slider_MouseEnter(object sender, MouseEventArgs e)
        {
            MySlider slider = (MySlider)sender;
            slider.AnimateSlider(SliderAnimateMode.Show);
            e.Handled = true;
        }

        void scroller_ScrollChanged(object sender, RoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)sender;
            MySlider slider = stickyWindow.sSlider;
            slider.HandleScrollChange(stickyWindow);
            e.Handled = true;
        }



        #endregion


        #region Slider Helpers


        #endregion



    }
}