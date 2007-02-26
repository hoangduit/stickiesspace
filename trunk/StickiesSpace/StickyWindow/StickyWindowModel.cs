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

        static StickyWindowModel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StickyWindowModel), new FrameworkPropertyMetadata(typeof(StickyWindowModel)));
        }


        public StickyWindowModel()
        {
            this.Name = "stickyWindow";
            this.MinHeight = 50;
            this.MinWidth = 100;
            this.Height = 200;
            this.Width = 200;
            this.MyWindowState = WindowState.Normal;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Title = "This is the stickyWindow";

            this.Show();

            Size oSize = new Size(200, 200);
            this.OriginalSize = oSize;

            StickyWindowCommands commands = new StickyWindowCommands(this);
            this.sContextCircle.ContextMenu = commands.GetContextMenu();

            this.SetContainerCanvasBindings(SetBindingMode.SetBinding);

            #region Event Wireup

            this.sSlider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            this.sSlider.MouseLeave += new MouseEventHandler(slider_MouseLeave);
            this.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            this.sContextCircle.MouseLeftButtonDown += new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown);
            this.sTextArea.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            this.sTextArea.MouseDoubleClick += new MouseButtonEventHandler(txt_MouseDoubleClick);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowModel_MouseLeftButtonDown);

            #endregion

            this.color = Colors.LightBlue;
            this.Opacity = 1;
        }


        public StickyWindowModel(string DeSerializeXML)
        {
            this.Name = "stickyWindow";
            this.MinHeight = 50;
            this.MinWidth = 100;
            this.MyWindowState = WindowState.Normal;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Title = "This is the stickyWindow";

            this.Show();

            Deserialize(DeSerializeXML);

            StickyWindowCommands commands = new StickyWindowCommands(this);
            this.sContextCircle.ContextMenu = commands.GetContextMenu();

            this.SetContainerCanvasBindings(SetBindingMode.SetBinding);

            #region Event Wireup

            this.sSlider.MouseEnter += new MouseEventHandler(slider_MouseEnter);
            this.sSlider.MouseLeave += new MouseEventHandler(slider_MouseLeave);
            this.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(scroller_ScrollChanged));
            this.sContextCircle.MouseLeftButtonDown += new MouseButtonEventHandler(contextCircle_MouseLeftButtonDown);
            this.sTextArea.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            this.sTextArea.MouseDoubleClick += new MouseButtonEventHandler(txt_MouseDoubleClick);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowModel_MouseLeftButtonDown);

            #endregion

            this.Opacity = 1;
        }


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
            xmlout.WriteAttributeString("Color", new SolidColorBrush(
                                                    Color.FromArgb(this.color.A, this.color.R, this.color.G, this.color.B)
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
                        this.color = (Color)ColorConverter.ConvertFromString(attribNode.Attributes["Color"].Value);
                        break;
                }
            }
        }

        #endregion


        #region Events

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
                Border contextCircle = sender as Border;
                StickyWindowModel stickyWindow = contextCircle.TemplatedParent as StickyWindowModel;
                StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);

                switch (stickyWindow.MyWindowState)
                {
                    case WindowState.Minimized:
                        animations.RestoreAnimation();
                        break;

                    case WindowState.Normal:
                        animations.MinimizeAnimation();
                        break;
                }
                
                e.Handled = true;
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


 
    }


    
}
