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
    
    public class StickyWindowModel : Window
    {

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

        public Ellipse sContextCircle
        {
            get { return this.Template.FindName("contextCircle", this) as Ellipse; }
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

            //Resize Window

            //Show/Animate DropShadow

            this.DragMove();
        }


        public void ReleaseDrag()
        {
            //Hide/Animate DropShadow

            //Resize Window

            //Restore Canvas Bindings
            SetContainerCanvasBindings(SetBindingMode.SetBinding);
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
                        //this.ResizeMode = ResizeMode.CanResizeWithGrip;
                    }
                    break;

                case SetBindingMode.ClearBinding:
                    containerCanvas.Height = this.ActualHeight;
                    containerCanvas.Width = this.ActualWidth;

                    BindingOperations.ClearBinding(containerCanvas, Canvas.HeightProperty);
                    BindingOperations.ClearBinding(containerCanvas, Canvas.WidthProperty);

                    //this.ResizeMode = ResizeMode.NoResize;
                    break;
            }
        }

    }


    
}
