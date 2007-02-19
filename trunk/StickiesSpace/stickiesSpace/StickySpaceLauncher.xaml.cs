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
using System.Windows.Media.Effects;

namespace stickiesSpace
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        #region click debugging
        private int cDownCount = 0;
        private int cUpCount = 0;

        private void cDown(object sender)
        {
            cDownCount += 1;
            txtCustomWindowDown.Text = cDownCount.ToString();
            downType.Text = sender.GetType().ToString();
        }
        private void cUp(object sender)
        {
            cUpCount += 1;
            txtCustomWindowUp.Text = cUpCount.ToString();
            upType.Text = sender.GetType().ToString();
        }
        private void ResetCounts(object sender, RoutedEventArgs e)
        {
            ClearTypes();
            cDownCount = 0;
            txtCustomWindowDown.Text = String.Empty;
            cUpCount = 0;
            txtCustomWindowUp.Text = String.Empty;
        }
        private void ClearTypes()
        {
            upType.Text = String.Empty;
            downType.Text = String.Empty;
        }
        #endregion


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
            stickyWindow.Height = 220;
            stickyWindow.Width = 220;
            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.MyWindowState = WindowState.Normal;
            stickyWindow.Background = Brushes.Transparent;
            stickyWindow.AllowsTransparency = true;
            stickyWindow.WindowStyle = WindowStyle.None;
            stickyWindow.Title = "This is the stickyWindow";


            stickyWindow.Show();

            this.RegisterName(stickyWindow.Name, stickyWindow);
            this.RegisterName(stickyWindow.sContainer.Name, stickyWindow.sContainer);


            //stickyWindow.SetContainerCanvasBindings(SetBindingMode.SetBinding);

            #endregion


            #region UIElement grabbers

            Canvas container = stickyWindow.sContainer;
            Border border = stickyWindow.sBorder;
            Ellipse contextCircle = stickyWindow.sContextCircle;
            MyScrollViewer scroller = stickyWindow.sScroller;
            MyTextBox txt = stickyWindow.sTextArea;
            MySlider slider = stickyWindow.sSlider;

            #endregion


            txt.Focusable = false;
            contextCircle.ContextMenu = commands.GetContextMenu(stickyWindow);


            #region Event Wireup

            //slider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            //slider.MouseLeave += new MouseEventHandler(slider_MouseLeave);
            //stickyWindow.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            //contextCircle.MouseLeftButtonDown += new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown);
            //txt.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            //txt.MouseDoubleClick += new MouseButtonEventHandler(txt_MouseDoubleClick);

            txt.MouseLeftButtonDown += new MouseButtonEventHandler(txt_MouseLeftButtonDown);
            txt.AddHandler(StickyWindowModel.PreviewMouseUpEvent, new MouseButtonEventHandler(txt_ForceMouseUp));
            #endregion


        }


        void txt_ForceMouseUp(object sender, MouseButtonEventArgs e)
        {
            cUp(sender);
            TextBox txt = sender as TextBox;
            txt.ReleaseMouseCapture();
        }

        void txt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cDown(sender);

            MyTextBox txt = sender as MyTextBox;
            StickyWindowModel stickyWindow = txt.TemplatedParent as StickyWindowModel;
            stickyWindow.DragMove();
            txt.CaptureMouse();
            

            //stickyWindow.StartDrag();



            //Break Canvas bindings
            //stickyWindow.SetContainerCanvasBindings(SetBindingMode.ClearBinding);

            //Create DropShadow
            //DropShadowBitmapEffect dps = new DropShadowBitmapEffect();
            //dps.SetValue(NameProperty, "dps");
            //dps.Softness = 1;
            //dps.ShadowDepth = 0;
            //stickyWindow.sContainer.BitmapEffect = dps;



            //Resize Window
            //this.Height += 10;
            //this.Width += 10;

            //Animate DropShadow
            //Storyboard animationsDropShadowGrow = this.Template.Resources["animationsDropShadowGrow"] as Storyboard;
            //animationsDropShadowGrow.Begin(this.sContainer);

            //this.RegisterName(dps.GetValue(NameProperty).ToString(), dps);
            //DoubleAnimation animDropShadow = new DoubleAnimation(0, 5, new TimeSpan(0, 0, 0, 0, 500));
            //Storyboard.SetTargetName(animDropShadow, dps.GetValue(NameProperty).ToString());
            //Storyboard.SetTargetProperty(animDropShadow, new PropertyPath(DropShadowBitmapEffect.ShadowDepthProperty));
            //Storyboard storyMin = new Storyboard();
            //storyMin.Children.Add(animDropShadow);
            //storyMin.Begin(stickyWindow.sContainer);


        }





        #region Events

        
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