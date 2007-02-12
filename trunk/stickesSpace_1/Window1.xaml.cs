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
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace stickesSpace_1
{
    /// <summary>
    /// Model for StickyWindow
    /// </summary>

    public enum TextBoxActiveState
        {
            Active, Inactive
        }

    public partial class Window1 : System.Windows.Window
    {

        //Command Events
        public static RoutedCommand CloseCmd = new RoutedCommand();
        public static RoutedCommand MinimizeCmd = new RoutedCommand();
        public static RoutedCommand RestoreCmd = new RoutedCommand();
        public static RoutedCommand FitContentCmd = new RoutedCommand();


        public Window1()
        {
            InitializeComponent();
        }


        public enum SetBindingMode
        {
            SetBinding,
            ClearBinding
        }
        public enum SliderAnimateMode
        {
            Show, Hide
        }



        void newWindow(object sender, EventArgs e)
        {


            #region Window

            UCWindow newWindow = new UCWindow();
            newWindow.Name = String.Format("stickyWindow_{0}", DateTime.Now.Ticks);
            newWindow.MinHeight = 50;
            newWindow.MinWidth = 100;
            newWindow.Height = 200;
            newWindow.Width = 200;
            newWindow.WindowStyle = WindowStyle.None;
            newWindow.Background = Brushes.Transparent;
            newWindow.AllowsTransparency = true;
            newWindow.MouseLeftButtonDown += new MouseButtonEventHandler(newWindow_MouseLeftButtonDown);
            newWindow.MouseLeftButtonUp += new MouseButtonEventHandler(newWindow_MouseLeftButtonUp);
            newWindow.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(sview_ScrollChanged));
            newWindow.AddHandler(Ellipse.MouseLeftButtonDownEvent, new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown));

            this.RegisterName(newWindow.Name, newWindow);

            #endregion



            #region Container

            Canvas containerCanvas = new Canvas();
            containerCanvas.Name = "Container";
            containerCanvas.Background = Brushes.Transparent;
            containerCanvas.HorizontalAlignment = HorizontalAlignment.Left;
            containerCanvas.VerticalAlignment = VerticalAlignment.Top;
            containerCanvas.Margin = new Thickness(0);

            #endregion



            #region Border

            Binding bindWinH = new Binding("ActualHeight");
            bindWinH.Source = containerCanvas;
            Binding bindWinW = new Binding("ActualWidth");
            bindWinW.Source = containerCanvas;

            Border newBorder = new Border();
            newBorder.SetBinding(Border.HeightProperty, bindWinH);
            newBorder.SetBinding(Border.WidthProperty, bindWinW);
            newBorder.CornerRadius = new CornerRadius(5);
            newBorder.BorderBrush = Brushes.LightBlue;
            newBorder.BorderThickness = new Thickness(10);

            #endregion



            #region MenuButton

            Ellipse contextCircle = new Ellipse();
            contextCircle.Name = "contextCircle";
            contextCircle.Fill = new SolidColorBrush(Colors.Gray);
            contextCircle.Height = 8;
            contextCircle.Width = 8;
            contextCircle.SetValue(Canvas.LeftProperty, double.Parse("3"));
            contextCircle.SetValue(Canvas.TopProperty, double.Parse("3"));

            #endregion



            #region TextAreea

            TextBox txtTypeHere = new TextBox();
            txtTypeHere.Name = "txtArea";
            txtTypeHere.Background = Brushes.LightBlue;
            txtTypeHere.BorderThickness = new Thickness(0);
            txtTypeHere.Padding = new Thickness(0,0,10,10);
            txtTypeHere.AcceptsTab = true;
            txtTypeHere.AcceptsReturn = true;
            txtTypeHere.TextWrapping = TextWrapping.Wrap;

            SetTextBoxBindings(txtTypeHere, SetBindingMode.SetBinding);

            #endregion



            #region Scroller

            Binding bindTxtH = new Binding("ActualHeight");
            Binding bindTxtW = new Binding("ActualWidth");
            bindTxtH.Source = containerCanvas;
            bindTxtH.Converter = new ArithmeticConverter();
            bindTxtH.ConverterParameter = -10;
            bindTxtW.Source = containerCanvas;
            bindTxtW.Converter = new ArithmeticConverter();
            bindTxtW.ConverterParameter = -10;

            MyScrollView sview = new MyScrollView();
            sview.Name = "scroller";
            sview.Content = txtTypeHere;
            sview.SetBinding(TextBox.HeightProperty, bindTxtH);
            sview.SetBinding(TextBox.WidthProperty, bindTxtW);
            sview.SetValue(Canvas.TopProperty, double.Parse("10"));
            sview.SetValue(Canvas.LeftProperty, double.Parse("10"));
            sview.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            Binding bindSlideH = new Binding("ActualHeight");
            bindSlideH.Source = sview;
            bindSlideH.Converter = new ArithmeticConverter();
            bindSlideH.ConverterParameter = -25;

            MySlider sldTxtBox = new MySlider();
            sldTxtBox.Name = "slider";
            sldTxtBox.Orientation = Orientation.Vertical;
            sldTxtBox.SetBinding(Slider.HeightProperty, bindSlideH);
            sldTxtBox.Width = 13;
            sldTxtBox.Background = Brushes.Transparent;
            sldTxtBox.SetValue(Canvas.RightProperty, double.Parse("-35"));
            sldTxtBox.SetValue(Canvas.TopProperty, double.Parse("15"));
            sldTxtBox.IsDirectionReversed = true;
            sldTxtBox.Minimum = 0;
            sldTxtBox.MouseEnter += new MouseEventHandler(sldTxtBox_MouseEnter);
            sldTxtBox.MouseLeave += new MouseEventHandler(sldTxtBox_MouseLeave);

            Binding bindVertOffset = new Binding("MyVerticalOffset");
            bindVertOffset.Source = sview;
            bindVertOffset.Mode = BindingMode.OneWayToSource;
            sldTxtBox.SetBinding(Slider.ValueProperty, bindVertOffset);

            #endregion


            //add objects to container, add container to window and Show()
            containerCanvas.Children.Add(newBorder);
            containerCanvas.Children.Add(sview);
            containerCanvas.Children.Add(sldTxtBox);
            containerCanvas.Children.Add(contextCircle);

            newWindow.Owner = this;
            newWindow.Content = containerCanvas;
            SetContainerCanvasBindings(newWindow, SetBindingMode.SetBinding);
            newWindow.Show();


            #region reflectionPanel
            //StackPanel stack = new StackPanel();
            //stack.Height = 400;
            //stack.Width = 200;


            //Canvas reflectionCanvas = new Canvas();
            //reflectionCanvas.Height = 200;
            //reflectionCanvas.Width = 200;

            //ScaleTransform scaleIt = new ScaleTransform(1, -1, 100, 101);
            //SkewTransform skewIt = new SkewTransform(-45,0);
            //TransformGroup transG = new TransformGroup();
            //transG.Children.Add(scaleIt);
            //transG.Children.Add(skewIt);
            //VisualBrush reflectionBrush = new VisualBrush(containerCanvas);
            //reflectionBrush.Transform = transG;

            //GradientStopCollection gradStops = new GradientStopCollection();
            //gradStops.Add(new GradientStop(Colors.Black, 0));
            //gradStops.Add(new GradientStop(Colors.Transparent, .25));
            //LinearGradientBrush linGradB = new LinearGradientBrush(gradStops);
            //linGradB.StartPoint = new Point(0, 0);
            //linGradB.EndPoint = new Point(0, 1);
            //reflectionCanvas.OpacityMask = linGradB;

            //reflectionCanvas.Background = reflectionBrush;

            //stack.Children.Add(containerCanvas);
            //stack.Children.Add(reflectionCanvas);
            #endregion


        }









        #region LeftClickMenu

        void contextCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window callingWindow = (Window)sender;
            if (e.Source.GetType() == typeof(Ellipse))
            {
                ContextMenu menu = GetContextMenu(callingWindow);
                menu.IsOpen = true;
            }
                
        }


        private ContextMenu GetContextMenu(Window callingWindow)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem m1, m2, m3, m4;

            //Commands
            CommandBinding CloseCmdBinding = new CommandBinding(CloseCmd, CloseCmdExecuted, CloseCmdCanExecute);
            CommandBinding MinimizeCmdBinding = new CommandBinding(MinimizeCmd, MinizmizeCmdExecuted, MinizmizeCmdCanExecute);
            CommandBinding FitContentCmdBinding = new CommandBinding(FitContentCmd, FitContentCmdExecuted, FitContentCmdCanExecute);
            CommandBinding RestoreCmdBinding = new CommandBinding(RestoreCmd, RestoreCmdExecuted, RestoreCmdCanExecute);

            m1 = new MenuItem();
            m1.Header = "Close";
            m1.CommandBindings.Add(CloseCmdBinding);
            m1.Command = CloseCmd;
            m1.CommandParameter = callingWindow;


            m2 = new MenuItem();
            m2.Header = "Minimize";
            m2.CommandBindings.Add(MinimizeCmdBinding);
            m2.Command = MinimizeCmd;
            m2.CommandParameter = callingWindow;

            m3 = new MenuItem();
            m3.Header = "Restore";
            m3.CommandBindings.Add(RestoreCmdBinding);
            m3.Command = RestoreCmd;
            m3.CommandParameter = callingWindow;

            m4 = new MenuItem();
            m4.Header = "Fit Content";
            m4.CommandBindings.Add(FitContentCmdBinding);
            m4.Command = FitContentCmd;
            m4.CommandParameter = callingWindow;

            menu.Items.Add(m1);
            menu.Items.Add(m2);
            menu.Items.Add(m3);
            menu.Items.Add(m4);

            return menu;
        }

        #endregion



        #region Commands

        //Close menuCommand
        private void CloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)e.Parameter;
            callingWindow.Close();
        }

        private void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Restore menuCommand
        private void RestoreCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)e.Parameter;
            MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(callingWindow, "slider");
            TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(callingWindow, "txtArea");

            SetTextBoxBindings(txt, SetBindingMode.SetBinding);
            SetContainerCanvasBindings(callingWindow, SetBindingMode.SetBinding);

            double currWidth = callingWindow.ActualWidth;
            double currHeight = callingWindow.ActualHeight;
            double newWidth = callingWindow.OriginalSize.Width;
            double newHeight = callingWindow.OriginalSize.Height;

            DoubleAnimation animHeight = new DoubleAnimation(currHeight, newHeight, new TimeSpan(0, 0, 0, 0, 250));
            DoubleAnimation animWidth = new DoubleAnimation(currWidth, newHeight, new TimeSpan(0, 0, 0, 0, 250));

            Storyboard.SetTargetName(animHeight, callingWindow.Name);
            Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            Storyboard.SetTargetName(animWidth, callingWindow.Name);
            Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            Storyboard storyMin = new Storyboard();
            storyMin.Children.Add(animWidth);
            storyMin.Children.Add(animHeight);

            storyMin.Begin(this);

            callingWindow.MinHeight = 50;
            callingWindow.MinWidth = 100;
            callingWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            callingWindow.MyWindowState = WindowState.Normal;
        }

        private void RestoreCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //FitContent menuCommand
        private void FitContentCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)e.Parameter;
            MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(callingWindow, "slider");
            TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(callingWindow, "txtArea");

            SetTextBoxBindings(txt, SetBindingMode.SetBinding);
            SetContainerCanvasBindings(callingWindow, SetBindingMode.SetBinding);

            double currWidth = callingWindow.ActualWidth;
            double currHeight = callingWindow.ActualHeight;

            DoubleAnimation animHeight = new DoubleAnimation(currHeight, txt.ExtentHeight+50, new TimeSpan(0, 0, 0, 0, 250));
            DoubleAnimation animWidth = new DoubleAnimation(currWidth, 200, new TimeSpan(0, 0, 0, 0, 250));

            Storyboard.SetTargetName(animHeight, callingWindow.Name);
            Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            Storyboard.SetTargetName(animWidth, callingWindow.Name);
            Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            Storyboard storyMin = new Storyboard();
            storyMin.Children.Add(animWidth);
            storyMin.Children.Add(animHeight);

            storyMin.Begin(this);

            callingWindow.MinHeight = 50;
            callingWindow.MinWidth = 100;
            callingWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            callingWindow.MyWindowState = WindowState.Normal;
        }

        private void FitContentCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Minimize menuCommand
        private void MinizmizeCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)e.Parameter;
            MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(callingWindow, "slider");
            Canvas containerCanvas = (Canvas)LogicalTreeHelper.FindLogicalNode(callingWindow, "Container");
            TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(callingWindow, "txtArea");

            SetTextBoxBindings(txt, SetBindingMode.ClearBinding);
            SetContainerCanvasBindings(callingWindow, SetBindingMode.SetBinding);
            callingWindow.MinHeight = 0;
            callingWindow.MinWidth = 0;
            callingWindow.ResizeMode = ResizeMode.NoResize;
            callingWindow.RenderTransform = new RotateTransform(0, 10, 10);
            callingWindow.OriginalSize = new Size(callingWindow.ActualWidth, callingWindow.ActualHeight);
            callingWindow.MyWindowState = WindowState.Minimized;


            double currWidth = callingWindow.ActualWidth;
            double currHeight = callingWindow.ActualHeight;

            //DoubleAnimation animSpin = new DoubleAnimation(0, 360, new TimeSpan(0, 0, 0, 0, 500));
            DoubleAnimation animHeight = new DoubleAnimation(currHeight, 20, new TimeSpan(0, 0, 0, 0, 250));
            DoubleAnimation animWidth = new DoubleAnimation(currWidth, 20, new TimeSpan(0, 0, 0, 0, 250));

            //Storyboard.SetTargetName(animSpin, callingWindow.Name);
            //Storyboard.SetTargetProperty(animSpin, new PropertyPath("(0).(1)", 
            //    new DependencyProperty[] {
            //        UIElement.RenderTransformProperty,
            //        RotateTransform.AngleProperty}));

            Storyboard.SetTargetName(animHeight, callingWindow.Name);
            Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            Storyboard.SetTargetName(animWidth, callingWindow.Name);
            Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            Storyboard storyMin = new Storyboard();
            //storyMin.Children.Add(animSpin);
            storyMin.Children.Add(animWidth);
            storyMin.Children.Add(animHeight);

            storyMin.Begin(this);
        }

        private void MinizmizeCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion



        #region Events

        void sldTxtBox_MouseLeave(object sender, MouseEventArgs e)
        {
            MySlider mySlider = (MySlider)sender;
            AnimateSlider(mySlider, SliderAnimateMode.Hide);
        }


        void sldTxtBox_MouseEnter(object sender, MouseEventArgs e)
        {
            MySlider mySlider = (MySlider)sender;
            AnimateSlider(mySlider, SliderAnimateMode.Show);
        }


        void txtTypeHere_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox rtxtB = (TextBox)sender;
            ActivateTextBox(rtxtB);
        }


        void txtTypeHere_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            DeactivateTextBox(txt);
        }


        void sview_ScrollChanged(object sender, RoutedEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)sender;
            MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(callingWindow, "slider");
            MyScrollView scroller = (MyScrollView)LogicalTreeHelper.FindLogicalNode(callingWindow, "scroller");

            HandleScrollChange(callingWindow, slider, scroller);
        }


        void newWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)sender;
            DragWindow(callingWindow);
        }


        void newWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)sender;
            ResetDragWindow(callingWindow);
        }


        void txtTypeHere_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TextBox txt = sender as TextBox;
                MyScrollView scroller = txt.Parent as MyScrollView;
                Canvas container = scroller.Parent as Canvas;
                UCWindow callingWindow = container.Parent as UCWindow;

                DragWindow(callingWindow);
            }
        }


        void txtTypeHere_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCWindow callingWindow = (UCWindow)sender;
            ResetDragWindow(callingWindow);
        }

        #endregion


        #region TextBox Helpers

        private void SetTextBoxBindings(TextBox txtTypeHere, SetBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case SetBindingMode.SetBinding:
                    txtTypeHere.MouseDoubleClick += new MouseButtonEventHandler(txtTypeHere_MouseDoubleClick);
                    txtTypeHere.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txtTypeHere_LostKeyboardFocus);
                    txtTypeHere.MouseLeftButtonDown += new MouseButtonEventHandler(txtTypeHere_MouseLeftButtonDown);
                    txtTypeHere.MouseLeftButtonUp += new MouseButtonEventHandler(txtTypeHere_MouseLeftButtonUp);
                    break;

                case SetBindingMode.ClearBinding:
                    txtTypeHere.MouseDoubleClick -= new MouseButtonEventHandler(txtTypeHere_MouseDoubleClick);
                    txtTypeHere.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(txtTypeHere_LostKeyboardFocus);
                    txtTypeHere.MouseLeftButtonDown -= new MouseButtonEventHandler(txtTypeHere_MouseLeftButtonDown);
                    txtTypeHere.MouseLeftButtonUp -= new MouseButtonEventHandler(txtTypeHere_MouseLeftButtonUp);
                    break;
            }

        }


        private void ActivateTextBox(TextBox txt)
        {
            MyScrollView scroller = txt.Parent as MyScrollView;
            Canvas container = scroller.Parent as Canvas;
            UCWindow callingWindow = container.Parent as UCWindow;

            if (callingWindow.MyWindowState != WindowState.Minimized)
            {
                txt.Focusable = true;
                txt.SetValue(TextBox.CursorProperty, Cursors.IBeam);
            }

        }


        private void DeactivateTextBox(TextBox txt)
        {
            txt.Focusable = false;
            txt.SetValue(TextBox.CursorProperty, Cursors.Arrow);
        }

        #endregion


        #region Slider Helpers


        private void HandleScrollChange(UCWindow callingWindow, MySlider slider, MyScrollView scroller)
        {
            if (scroller.ExtentHeight > scroller.Height && callingWindow.MyWindowState == WindowState.Normal)
                slider.MyVisibility = Visibility.Visible;
            else
                slider.MyVisibility = Visibility.Hidden;

            slider.Value = scroller.MyVerticalOffset;
            slider.Maximum = scroller.ExtentHeight;

            lblHit.Content = String.Format("{0}, {1}", slider.Maximum, scroller.ExtentHeight);
            lblHit2.Content = String.Format("{0}", slider.Value);
        }


        private void AnimateSlider(MySlider mySlider, SliderAnimateMode sliderAnimateMode)
        {
            double slideFrom = 0;
            double slideTo = 0;

            switch (sliderAnimateMode)
            {
                case SliderAnimateMode.Show:
                    slideFrom = (double)mySlider.GetValue(Canvas.RightProperty);
                    slideTo = 0;
                    break;

                case SliderAnimateMode.Hide:
                    slideFrom = (double)mySlider.GetValue(Canvas.RightProperty);
                    slideTo = -5;
                    break;
            }

            mySlider.BeginAnimation(Canvas.RightProperty,
                new DoubleAnimation(slideFrom, slideTo, new TimeSpan(0, 0, 0, 0, 250)));
        }

        #endregion


        #region Window Helpers

        private void ResetDragWindow(UCWindow callingWindow)
        {
            Canvas containerCanvas = (Canvas)LogicalTreeHelper.FindLogicalNode(callingWindow, "Container");
            TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(callingWindow, "txtArea");

            containerCanvas.BitmapEffect = null;

            //reset window size
            callingWindow.Height -= 10;
            callingWindow.Width -= 10;

            SetContainerCanvasBindings(callingWindow, SetBindingMode.SetBinding);

            //reset textBoxActiveState
            if (callingWindow.PreviousTextBoxState == TextBoxActiveState.Active)
                ActivateTextBox(txt);
            else
                DeactivateTextBox(txt);
        }


        private void DragWindow(UCWindow callingWindow)
        {
            //TODO Fix DragMove() on txtArea
            try
            {
                Canvas containerCanvas = (Canvas)LogicalTreeHelper.FindLogicalNode(callingWindow, "Container");
                SetContainerCanvasBindings(callingWindow, SetBindingMode.ClearBinding);
                
                //TODO move to Custom textBox control
                TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(callingWindow, "txtArea");
                TextBoxActiveState txtBoxState;
                if (txt.Focusable)
                    txtBoxState = TextBoxActiveState.Active;
                else
                    txtBoxState = TextBoxActiveState.Inactive;
                callingWindow.PreviousTextBoxState = txtBoxState;
                
                DeactivateTextBox(txt);

                
                //Enlarge window for dropshadow
                callingWindow.Width += 10;
                callingWindow.Height += 10;

                DropShadowBitmapEffect dps = new DropShadowBitmapEffect();
                dps.Softness = 1;
                dps.ShadowDepth = 5;
                containerCanvas.BitmapEffect = dps;

                callingWindow.DragMove();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }


        private void SetContainerCanvasBindings(UCWindow callingWindow, SetBindingMode bindingMode)
        {
            Canvas containerCanvas = LogicalTreeHelper.FindLogicalNode(callingWindow, "Container") as Canvas;

            switch (bindingMode)
            {
                case SetBindingMode.SetBinding:
                    Binding bindWinH = new Binding("ActualHeight");
                    bindWinH.Source = callingWindow;
                    Binding bindWinW = new Binding("ActualWidth");
                    bindWinW.Source = callingWindow;

                    containerCanvas.SetBinding(Canvas.HeightProperty, bindWinH);
                    containerCanvas.SetBinding(Canvas.WidthProperty, bindWinW);

                    if (callingWindow.MyWindowState != WindowState.Minimized)
                        callingWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                    break;

                case SetBindingMode.ClearBinding:
                    containerCanvas.Height = callingWindow.ActualHeight;
                    containerCanvas.Width = callingWindow.ActualWidth;

                    BindingOperations.ClearBinding(containerCanvas, Canvas.HeightProperty);
                    BindingOperations.ClearBinding(containerCanvas, Canvas.WidthProperty);

                    callingWindow.ResizeMode = ResizeMode.NoResize;
                    break;
            }
        }

        #endregion

    }


    #region Custom Control Classes

    public class UCWindow : Window
    {
        private Size _OriginalSize;
        private WindowState _MyWindowState;
        private TextBoxActiveState _PreviousTextBoxState;

        public Size OriginalSize
        {
            get { return _OriginalSize; }
            set { _OriginalSize = value; }
        }

        public WindowState MyWindowState
        {
            get { return _MyWindowState; }
            set { _MyWindowState = value; }
        }

        public TextBoxActiveState PreviousTextBoxState 
        {
            get { return _PreviousTextBoxState; }
            set { _PreviousTextBoxState = value; }
        }

        protected override Geometry GetLayoutClip(Size ls)
        {
            return null;
        } 
    }


    public class MyScrollView : ScrollViewer
    {
        public double MyVerticalOffset 
        {
            get { return this.VerticalOffset; }
            set { this.ScrollToVerticalOffset(value); }
        }
    }


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
                    case Visibility.Visible :
                        if ((double)this.GetValue(Canvas.RightProperty) < -5)
                            AnimateSliderPeek(this, value);
                        break;
                    case Visibility.Hidden :
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
                case Visibility.Visible :
                    mySlider.Visibility = Visibility.Visible;
                    mySlider.BeginAnimation(Canvas.RightProperty,
                        new DoubleAnimation(-20, -5, new TimeSpan(0, 0, 0, 0, 500)));
                    break;

                case Visibility.Hidden :
                    mySlider.BeginAnimation(Canvas.RightProperty,
                        new DoubleAnimation((double)this.GetValue(Canvas.RightProperty), -20, new TimeSpan(0, 0, 0, 0, 500)));
                    break;
            }
        }
    }

    #endregion


}