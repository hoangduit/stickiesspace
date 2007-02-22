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
        }


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
                BindSliders();
            }
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

        private void BindColor()
        {
            testRect.Background = new SolidColorBrush(currColor);

            //change parent stickyWindow colors
            stickyWindowParent.color = currColor;
        }

        private void BindSliders()
        {
            this.sAlpha.Value = stickyWindowParent.color.ScA;
            this.sRed.Value = stickyWindowParent.color.ScR;
            this.sGreen.Value = stickyWindowParent.color.ScG;
            this.sBlue.Value = stickyWindowParent.color.ScB;
        }

        public void Initialize()
        {
            BindSliders();
            BindColor();

            this.sAlpha.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sRed.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sGreen.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.sBlue.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSlider_ValueChanged);
            this.closer.Click += new RoutedEventHandler(closer_Click);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowColorControlModel_MouseLeftButtonDown);
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


    }
}
