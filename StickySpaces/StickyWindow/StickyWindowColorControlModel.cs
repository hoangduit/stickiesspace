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
            this.ShowInTaskbar = false;
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

        public StickyWindowModel stickyWindowParent
        {
            get { return _stickyWindowParent; }
            set { _stickyWindowParent = value; }
        }

        private Color _currNoteColor;
        public Color currNoteColor
        {
            get 
            { 
                return Color.FromScRgb(
                        (float)(double)sAlphaNote.Value,
                        (float)(double)sRedNote.Value,
                        (float)(double)sGreenNote.Value,
                        (float)(double)sBlueNote.Value); 
            }
            set
            {
                _currNoteColor = value;
                BindSliders(value, ColorControlSliders.Note);
            }
        }

        private Color _currTextColor;
        public Color currTextColor
        {
            get
            {
                return Color.FromScRgb(
                        (float)(double)sAlphaText.Value,
                        (float)(double)sRedText.Value,
                        (float)(double)sGreenText.Value,
                        (float)(double)sBlueText.Value);
            }
            set
            {
                _currTextColor = value;
                BindSliders(value, ColorControlSliders.Text);
            }
        }

        public ComboBox colorListNote
        {
            get { return this.Template.FindName("colorListN", this) as ComboBox; }
        }

        public Button closer
        {
            get { return this.Template.FindName("close", this) as Button; }
        }

        public Button applier
        {
            get { return this.Template.FindName("apply", this) as Button; }
        }

        public Slider sAlphaNote
        {
            get { return this.Template.FindName("alphaN", this) as Slider; }
        }

        public Slider sRedNote
        {
            get { return this.Template.FindName("redN", this) as Slider; }
        }

        public Slider sGreenNote
        {
            get { return this.Template.FindName("greenN", this) as Slider; }
        }

        public Slider sBlueNote
        {
            get { return this.Template.FindName("blueN", this) as Slider; }
        }

        public ComboBox colorListText
        {
            get { return this.Template.FindName("colorListT", this) as ComboBox; }
        }

        public Slider sAlphaText
        {
            get { return this.Template.FindName("alphaT", this) as Slider; }
        }

        public Slider sRedText
        {
            get { return this.Template.FindName("redT", this) as Slider; }
        }

        public Slider sGreenText
        {
            get { return this.Template.FindName("greenT", this) as Slider; }
        }

        public Slider sBlueText
        {
            get { return this.Template.FindName("blueT", this) as Slider; }
        }

        public Border testRect
        {
            get { return this.Template.FindName("testRect", this) as Border; }
        }

        public TextBlock testText
        {
            get { return this.Template.FindName("testText", this) as TextBlock; }
        }

        #endregion


        #region Events & Such

        void ColorSliderNote_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BindColorTest(ColorControlSliders.Note);
        }

        void colorListNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox colorList = sender as ComboBox;
            ColorItem selectedColor = colorListNote.SelectedValue as ColorItem;
            currNoteColor = selectedColor.Brush.Color;
            BindColorTest(ColorControlSliders.Note);
        }


        void ColorSliderText_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BindColorTest(ColorControlSliders.Text);
        }

        void colorListText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox colorList = sender as ComboBox;
            ColorItem selectedColor = colorListText.SelectedValue as ColorItem;
            currTextColor = selectedColor.Brush.Color;
            BindColorTest(ColorControlSliders.Text);
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

        void applier_Click(object sender, RoutedEventArgs e)
        {
            BindColorApply();
        }

        #endregion


        #region Methods

        private void BindColorTest(ColorControlSliders which)
        {
            switch (which)
            {
                case ColorControlSliders.Note:
                    testRect.Background = new SolidColorBrush(currNoteColor);
                    break;

                case ColorControlSliders.Text:
                    testText.Foreground = new SolidColorBrush(currTextColor);
                    break;
            }            
        }


        private void BindColorApply()
        {
            stickyWindowParent.color = currNoteColor;
            stickyWindowParent.textColor = currTextColor;
        }


        private void BindSliders(Color color, ColorControlSliders which)
        {
            switch (which)
            {
                case ColorControlSliders.Note:
                    this.sAlphaNote.Value = color.ScA;
                    this.sRedNote.Value = color.ScR;
                    this.sGreenNote.Value = color.ScG;
                    this.sBlueNote.Value = color.ScB;
                    break;

                case ColorControlSliders.Text:
                    this.sAlphaText.Value = color.ScA;
                    this.sRedText.Value = color.ScR;
                    this.sGreenText.Value = color.ScG;
                    this.sBlueText.Value = color.ScB;
                    break;
            }
        }


        public void Initialize()
        {
            BindSliders(stickyWindowParent.color, ColorControlSliders.Note);
            BindSliders(stickyWindowParent.textColor, ColorControlSliders.Text);
            BindColorTest(ColorControlSliders.Note);
            BindColorTest(ColorControlSliders.Text);

            this.sAlphaNote.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderNote_ValueChanged);
            this.sRedNote.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderNote_ValueChanged);
            this.sGreenNote.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderNote_ValueChanged);
            this.sBlueNote.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderNote_ValueChanged);
            this.colorListNote.SelectionChanged += new SelectionChangedEventHandler(colorListNote_SelectionChanged);

            this.sAlphaText.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderText_ValueChanged);
            this.sRedText.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderText_ValueChanged);
            this.sGreenText.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderText_ValueChanged);
            this.sBlueText.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ColorSliderText_ValueChanged);
            this.colorListText.SelectionChanged += new SelectionChangedEventHandler(colorListText_SelectionChanged);

            this.closer.Click += new RoutedEventHandler(closer_Click);
            this.applier.Click += new RoutedEventHandler(applier_Click);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StickyWindowColorControlModel_MouseLeftButtonDown);
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
