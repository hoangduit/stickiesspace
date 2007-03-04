using System;
using System.IO;
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
using System.Windows.Media.Effects;
using System.Xml;

namespace StickyWindow
{
    
    public class StickyWindowModel : Window
    {

        Uri iconUri = new Uri(@"pack://application:,,,/StickySpaces;component/Resources/StickyIcon.ico");
        

        static StickyWindowModel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StickyWindowModel), new FrameworkPropertyMetadata(typeof(StickyWindowModel)));
        }


        public StickyWindowModel(string DeSerializeXML)
        {
            this.Name = String.Format("stickyWindow_{0}", DateTime.Now.Ticks.ToString());
            this.MinHeight = 50;
            this.MinWidth = 100;
            this.MaxHeight = 600;
            this.MaxWidth = 600;
            this.MyWindowState = WindowState.Normal;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Title = "stickyWindow";
            this.ShowInTaskbar = false;

            IconBitmapDecoder icon = new IconBitmapDecoder(iconUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
            this.Icon = icon.Frames[0];

            this.Show();

            if (DeSerializeXML != null)
                Deserialize(DeSerializeXML);
            else
            {
                this.color = Colors.LightBlue;
                this.textColor = Colors.Black;
                Size oSize = new Size(200, 200);
                this.OriginalSize = oSize;
            }

           
            #region Event/Bindings Wireup

            this.SetContainerCanvasBindings(SetBindingMode.SetBinding);
            this.sSlider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            this.sSlider.MouseLeave += new MouseEventHandler(slider_MouseLeave);
            this.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            this.sContextCircle.MouseLeftButtonDown += new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown);
            //this.sTextArea.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            //this.sTextArea.MouseDoubleClick += new MouseButtonEventHandler(txt_MouseDoubleClick);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowModel_MouseLeftButtonDown);
            SetContextCircleMouseEventBindings(SetBindingMode.SetBinding);
            
            #endregion


            #region CommandBindings

            StickyWindowCommands commands = new StickyWindowCommands(this);
            this.sContextCircle.ContextMenu = commands.GetContextMenu();
            CommandBinding CloseCmdBinding = new CommandBinding(CloseCmd, commands.CloseCmdExecuted, commands.CloseCmdCanExecute);
            CommandBinding MinimizeCmdBinding = new CommandBinding(MinimizeCmd, commands.MinizmizeCmdExecuted, commands.MinizmizeCmdCanExecute);
            CommandBinding FitContentCmdBinding = new CommandBinding(FitContentCmd, commands.FitContentCmdExecuted, commands.FitContentCmdCanExecute);
            CommandBinding RestoreCmdBinding = new CommandBinding(RestoreCmd, commands.RestoreCmdExecuted, commands.RestoreCmdCanExecute);
            CommandBinding PrintCmdBinding = new CommandBinding(PrintCmd, commands.PrintCmdExecuted, commands.PrintCmdCanExecute);
            CommandBinding ColorsCmdBinding = new CommandBinding(ColorsCmd, commands.ColorsCmdExecuted, commands.ColorsCmdCanExecute);

            this.CommandBindings.Add(CloseCmdBinding);
            this.CommandBindings.Add(MinimizeCmdBinding);
            this.CommandBindings.Add(FitContentCmdBinding);
            this.CommandBindings.Add(RestoreCmdBinding);
            this.CommandBindings.Add(PrintCmdBinding);
            this.CommandBindings.Add(ColorsCmdBinding);

            #endregion

            this.Opacity = 1;
        }


        #region Commands

        public static RoutedCommand CloseCmd = new RoutedUICommand("Close", "Close", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.T, ModifierKeys.Control) }));
        public static RoutedCommand MinimizeCmd = new RoutedUICommand("Minimize", "Minimize", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.M, ModifierKeys.Control) }));
        public static RoutedCommand RestoreCmd = new RoutedUICommand("Restore", "Restore", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.R, ModifierKeys.Control) }));
        public static RoutedCommand FitContentCmd = new RoutedUICommand("Fit Content", "Fit Content", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F, ModifierKeys.Control) }));
        public static RoutedCommand PrintCmd = new RoutedUICommand("Print", "Print", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.P, ModifierKeys.Control) }));
        public static RoutedCommand ColorsCmd = new RoutedUICommand("Colors", "Colors", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.K, ModifierKeys.Control) }));

        #endregion


        #region Properties

        #region Custom Properties

        public WindowState MyWindowState
        {
            get { return (WindowState)GetValue(MyWindowStateProperty); }
            set { SetValue(MyWindowStateProperty, value); }
        }

        public Size OriginalSize
        {
            get { return (Size)GetValue(OriginalSizeProperty); }
            set { SetValue(OriginalSizeProperty, value); }
        }

        private Color _color;
        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;

                //update all colored elements colors
                this.sTextArea.Background = new SolidColorBrush(_color);
                this.sBorder.BorderBrush = new SolidColorBrush(_color);
            }
        }

        private Color _textColor;
        public Color textColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                this.sTextArea.Foreground = new SolidColorBrush(_textColor);
            }
        }

        private bool _isColorWindowOpen = false;
        public bool isColorWindowOpen
        {
            get { return _isColorWindowOpen; }
            set { _isColorWindowOpen = value; }
        }

        #endregion

        #region UIElement Properties

        public Canvas sContainer
        {
            get { return this.Template.FindName("container", this) as Canvas; }
        }

        public Border sBorder
        {
            get { return this.Template.FindName("border", this) as Border; }
        }

        public Border sContextCircle
        {
            get { return this.Template.FindName("contextCircle", this) as Border; }
        }

        public MyScrollViewer sScroller 
        {
            get { return this.Template.FindName("scroller", this) as MyScrollViewer; }
        }

        public MyTextBox sTextArea
        {
            get { return this.Template.FindName("txtArea", this) as MyTextBox;  }
        }

        public MySlider sSlider
        {
            get { return this.Template.FindName("slider", this) as MySlider; }
        }

        #endregion

        #endregion


        #region Custom DependencyProperties

        public static DependencyProperty MyWindowStateProperty = DependencyProperty.Register("MyWindowState", typeof(WindowState), typeof(Window), 
            new FrameworkPropertyMetadata(WindowState.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits));

        public static DependencyProperty OriginalSizeProperty = DependencyProperty.Register("OriginalSize", typeof(Size), typeof(Window),
            new FrameworkPropertyMetadata(new Size(0,0), FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits));

        #endregion


        #region AnimateDropshadow  - UNUSED

        public void StartDrag()
        {
            //Break Canvas bindings
            SetContainerCanvasBindings(SetBindingMode.ClearBinding);

            //Create DropShadow
            DropShadowBitmapEffect dps = new DropShadowBitmapEffect();
            dps.SetValue(NameProperty, "dps");
            dps.Softness = 1;
            dps.ShadowDepth = 0;
            this.sContainer.BitmapEffect = dps;
            this.sContainer.RegisterName(dps.GetValue(NameProperty).ToString(), dps);

            //Resize Window
            this.Height += 10;
            this.Width += 10;

            //Create animation
            DoubleAnimation animShowDropShadow = new DoubleAnimation(0, 5, TimeSpan.FromMilliseconds(100));
            Storyboard.SetTargetName(animShowDropShadow, dps.GetValue(NameProperty).ToString());
            Storyboard.SetTargetProperty(animShowDropShadow, new PropertyPath(DropShadowBitmapEffect.ShadowDepthProperty));
            Storyboard storyShowDrop = new Storyboard();
            storyShowDrop.Children.Add(animShowDropShadow);
            storyShowDrop.Begin(this.sContainer);

            this.DragMove();
        }


        public void ReleaseDrag()
        {
            //Hide/Animate DropShadow
            DropShadowBitmapEffect dps = this.sContainer.BitmapEffect as DropShadowBitmapEffect;
            
            DoubleAnimation animHideDropShadow = new DoubleAnimation(0, 5, TimeSpan.FromMilliseconds(100));
            animHideDropShadow.Completed += new EventHandler(animHideDropShadow_Completed);
            Storyboard.SetTargetName(animHideDropShadow, dps.GetValue(NameProperty).ToString());
            Storyboard.SetTargetProperty(animHideDropShadow, new PropertyPath(DropShadowBitmapEffect.ShadowDepthProperty));
            Storyboard storyHideDrop = new Storyboard();
            storyHideDrop.Children.Add(animHideDropShadow);
            storyHideDrop.Begin(this.sContainer);

            //Resize Window
            this.Height -= 10;
            this.Width -= 10;

            //Restore Canvas Bindings
            SetContainerCanvasBindings(SetBindingMode.SetBinding);
        }


        void animHideDropShadow_Completed(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


        #region Methods

        public void SetContainerCanvasBindings(SetBindingMode bindingMode)
        {
            Canvas containerCanvas = this.sContainer;

            switch (bindingMode)
            {
                case SetBindingMode.SetBinding:
                    Binding bindWinH = new Binding("ActualHeight");
                    bindWinH.Source = this;
                    Binding bindWinW = new Binding("ActualWidth");
                    bindWinW.Source = this;

                    containerCanvas.SetBinding(Canvas.HeightProperty, bindWinH);
                    containerCanvas.SetBinding(Canvas.WidthProperty, bindWinW);

                    if (this.MyWindowState != WindowState.Minimized)
                    {
                        this.ResizeMode = ResizeMode.CanResizeWithGrip;
                    }
                    break;

                case SetBindingMode.ClearBinding:
                    containerCanvas.Height = this.ActualHeight;
                    containerCanvas.Width = this.ActualWidth;

                    BindingOperations.ClearBinding(containerCanvas, Canvas.HeightProperty);
                    BindingOperations.ClearBinding(containerCanvas, Canvas.WidthProperty);

                    this.ResizeMode = ResizeMode.NoResize;
                    break;
            }
        }


        internal void SetContextCircleMouseEventBindings(SetBindingMode BindingMode)
        {
            switch (BindingMode)
            {
                case SetBindingMode.SetBinding:
                    this.sContextCircle.MouseEnter += new MouseEventHandler(sContextCircle_MouseEnter);
                    this.sContextCircle.MouseLeave += new MouseEventHandler(sContextCircle_MouseLeave);
                    break;

                case SetBindingMode.ClearBinding:
                    this.sContextCircle.RemoveHandler(Border.MouseEnterEvent, new MouseEventHandler(sContextCircle_MouseEnter));
                    this.sContextCircle.RemoveHandler(Border.MouseLeaveEvent, new MouseEventHandler(sContextCircle_MouseLeave));
                    break;
            }
        }


        public void ShowColorsControl()
        {
            StickyWindowColorControlModel stickyWindowColorControl = new StickyWindowColorControlModel(this);
            stickyWindowColorControl.CreateAndShow(this);
        }


        public String Serialize()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter xmlout = new XmlTextWriter(new StringWriter(sb));

            xmlout.WriteStartElement("StickyWindow");

            //window state
            xmlout.WriteStartElement("WindowState");
            xmlout.WriteAttributeString("State", this.MyWindowState.ToString());
            xmlout.WriteEndElement();

            //x,y position
            xmlout.WriteStartElement("WindowPosition");
            xmlout.WriteAttributeString("X", this.Left.ToString());
            xmlout.WriteAttributeString("Y", this.Top.ToString());
            xmlout.WriteEndElement();

            //h,w size
            xmlout.WriteStartElement("WindowSize");
            if (this.MyWindowState == WindowState.Normal)
            {
                xmlout.WriteAttributeString("H", this.ActualHeight.ToString());
                xmlout.WriteAttributeString("W", this.ActualWidth.ToString());
            }
            else
            {
                xmlout.WriteAttributeString("H", this.OriginalSize.Height.ToString());
                xmlout.WriteAttributeString("W", this.OriginalSize.Width.ToString());
            }
            xmlout.WriteEndElement();

            //text content
            xmlout.WriteElementString("WindowText", this.sTextArea.Text);

            //color
            xmlout.WriteStartElement("WindowColor");
            xmlout.WriteAttributeString("NoteColor", new SolidColorBrush(
                                                    Color.FromArgb(this.color.A, this.color.R, this.color.G, this.color.B)
                                                                      ).Color.ToString());
            xmlout.WriteAttributeString("TextColor", new SolidColorBrush(
                                                    Color.FromArgb(this.textColor.A, this.textColor.R, this.textColor.G, this.textColor.B)
                                                                      ).Color.ToString());
            xmlout.WriteEndElement();

            xmlout.WriteEndElement();
            xmlout.Flush();
            xmlout.Close();
            return sb.ToString();
        }


        public void Deserialize(string windowState)
        {
            XmlDocument windowStateDoc = new XmlDocument();
            windowStateDoc.LoadXml(windowState);

            XmlNodeList windowAttribs = windowStateDoc.ChildNodes[0].ChildNodes;

            foreach (XmlNode attribNode in windowAttribs)
            {
                switch (attribNode.Name)
                {
                    case "WindowPosition":
                        this.Left = double.Parse(attribNode.Attributes["X"].Value);
                        this.Top = double.Parse(attribNode.Attributes["Y"].Value);
                        break;

                    case "WindowState":
                        this.MyWindowState = (WindowState)Enum.Parse(typeof(WindowState), attribNode.Attributes["State"].Value);
                        break;

                    case "WindowSize":
                        Size oSize = new Size(double.Parse(attribNode.Attributes["W"].Value), double.Parse(attribNode.Attributes["H"].Value));
                        if (this.MyWindowState == WindowState.Minimized)
                        {
                            this.MinHeight = 0;
                            this.MinWidth = 0;
                            this.Height = 20;
                            this.Width = 20;
                            this.ResizeMode = ResizeMode.NoResize;
                            this.sTextArea.TextWrapping = TextWrapping.NoWrap;
                            UpdateToolTip();
                        }
                        else
                        {
                            this.Height = oSize.Height;
                            this.Width = oSize.Width;
                        }
                        this.OriginalSize = oSize;
                        break;

                    case "WindowText":
                        this.sTextArea.Text = attribNode.InnerText;
                        break;

                    case "WindowColor":
                        this.color = (Color)ColorConverter.ConvertFromString(attribNode.Attributes["NoteColor"].Value);
                        this.textColor = (Color)ColorConverter.ConvertFromString(attribNode.Attributes["TextColor"].Value);
                        break;
                }
            }
        }


        private void UpdateToolTip()
        {
            ToolTip noteToolTip = new ToolTip();
            noteToolTip.Content = new TextBlock(new Run(this.sTextArea.Text));
            noteToolTip.MaxHeight = 600;
            noteToolTip.MaxWidth = 600;
            noteToolTip.Visibility = Visibility.Visible;
            this.ToolTip = noteToolTip;
        }

        #endregion


        #region Events

        void sContextCircle_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StickyWindowAnimations animations = new StickyWindowAnimations(this);
            animations.ContextGrowAnimation();
        }

        void sContextCircle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StickyWindowAnimations animations = new StickyWindowAnimations(this);
            animations.ContextShrinkAnimation();
        }

        void StickyWindowModel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StickyWindowModel stickyWindow = sender as StickyWindowModel;
            stickyWindow.DragMove();
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
                StickyWindowAnimations animations = new StickyWindowAnimations(this);

                switch (MyWindowState)
                {
                    case WindowState.Minimized:
                        animations.RestoreAnimation();
                        this.ToolTip = new ToolTip();
                        break;

                    case WindowState.Normal:
                        animations.MinimizeAnimation();
                        UpdateToolTip();
                        break;
                }

                e.Handled = true;
            }
        }

        void slider_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MySlider slider = (MySlider)sender;
            slider.AnimateSlider(SliderAnimateMode.Hide);
            e.Handled = true;
        }

        void slider_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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


    }


    
}
