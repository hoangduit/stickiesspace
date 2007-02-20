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
using System.Windows.Media.Effects;

namespace StickyWindow
{
    
    public class StickyWindowModel : Window
    {

        public delegate MouseButtonEventHandler MouseLeftDown();

        static StickyWindowModel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StickyWindowModel), new FrameworkPropertyMetadata(typeof(StickyWindowModel)));
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
            new FrameworkPropertyMetadata(new Size(200,200), FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits));

        #endregion


        

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

    }


    
}
