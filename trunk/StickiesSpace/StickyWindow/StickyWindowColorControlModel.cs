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

    public class StickyWindowColorControlModel : Window
    {

        static StickyWindowColorControlModel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StickyWindowColorControlModel), new FrameworkPropertyMetadata(typeof(StickyWindowColorControlModel)));
        }


        public StickyWindowColorControlModel(StickyWindowModel stickyWindow)
        {
            _stickyWindowParent = stickyWindow;

            CommandBinding CloseCmdBinding = new CommandBinding(CloseCmd, CloseCmdExecuted, CloseCmdCanExecute);
            this.CommandBindings.Add(CloseCmdBinding);
        }


        #region Commands

        public static RoutedCommand CloseCmd = new RoutedUICommand("Close", "Close", typeof(System.Windows.Input.ICommand),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        public void CloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            stickyWindowParent.isColorWindowOpen = false;
            this.Close();
        }

        public void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion


        #region Properties

        private StickyWindowModel _stickyWindowParent;
        private Color _currColor;

        public StickyWindowModel stickyWindowParent
        {
            get { return _stickyWindowParent; }
            set { _stickyWindowParent = value; }
        }

        public Color currColor
        {
            get 
            { 
                return Color.FromScRgb(
                        (float)(double)sAlpha.Value,
                        (float)(double)sRed.Value,
                        (float)(double)sGreen.Value,
                        (float)(double)sBlue.Value); 
            }
            set
            {
                _currColor = value;
                BindSliders(value);
            }
        }

        public ComboBox colorList
        {
            get { return this.Template.FindName("colorList", this) as ComboBox; }
        }

        public Button closer
        {
            get { return this.Template.FindName("close", this) as Button; }
        }

        public Slider sAlpha
        {
            get { return this.Template.FindName("alpha", this) as Slider; }
        }

        public Slider sRed
        {
            get { return this.Template.FindName("red", this) as Slider; }
        }

        public Slider sGreen
        {
            get { return this.Template.FindName("green", this) as Slider; }
        }

        public Slider sBlue
        {
            get { return this.Template.FindName("blue", this) as Slider; }
        }

        public Border testRect
        {
            get { return this.Template.FindName("testRect", this) as Border; }
        }


        #endregion


        #region Events & Such

        void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BindColor();
        }


        void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox colorList = sender as ComboBox;
            ColorItem selectedColor = colorList.SelectedValue as ColorItem;
            currColor = selectedColor.Brush.Color;
            BindColor();
        }


        void StickyWindowColorControlModel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        void closer_Click(object sender, RoutedEventArgs e)
        {
            stickyWindowParent.isColorWindowOpen = false;
            this.Close();
        }

        #endregion


        #region Methods

        private void BindColor()
        {
            testRect.Background = new SolidColorBrush(currColor);
            stickyWindowParent.color = currColor;
        }


        private void BindSliders(Color color)
        {
            this.sAlpha.Value = color.ScA;
            this.sRed.Value = color.ScR;
            this.sGreen.Value = color.ScG;
            this.sBlue.Value = color.ScB;
        }


        public void Initialize()
        {
            BindSliders(stickyWindowParent.color);
            BindColor();

            this.sAlpha.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sRed.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sGreen.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sBlue.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.closer.Click += new RoutedEventHandler(closer_Click);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowColorControlModel_MouseLeftButtonDown);
            this.colorList.SelectionChanged += new SelectionChangedEventHandler(colorList_SelectionChanged);
        }


        internal void GetPosition(StickyWindowModel stickyWindow)
        {
            double parentY = stickyWindow.Top;
            double parentX = stickyWindow.Left;
            double parentH = stickyWindow.Height;
            double parentW = stickyWindow.Width;

            WindowPositon colorControlPos;

            //Left or Right
            if (parentX < 220)
                colorControlPos = WindowPositon.Right;
            else
                colorControlPos = WindowPositon.Left;

            //determine actual doubles
            switch (colorControlPos)
            {
                case WindowPositon.Right:
                    this.Left = parentX + this.Height + 10;
                    break;

                case WindowPositon.Left:
                    this.Left = parentX - this.Width - 10;
                    break;
            }

            //TODO - Top or Bottom
            this.Top = parentY;
        }


        internal void CreateAndShow(StickyWindowModel stickyWindow)
        {
            this.Height = 210;
            this.Width = 210;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Title = "stickyWindowColorControl";
            this.Name = "stickyWindowColorControl";
            this.Owner = stickyWindow;
            GetPosition(stickyWindow);

            //fade in window
            Storyboard animationsFadeInColorControl = stickyWindow.Template.Resources["animationsFadeInColorControl"] as Storyboard;
            animationsFadeInColorControl.Begin(this);

            this.Show();

            Initialize();
            stickyWindow.isColorWindowOpen = true;
        }

        #endregion

    }
}
