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

        public TextBoxActiveState PreviousTextBoxState
        {
            get { return (TextBoxActiveState)GetValue(PreviousTextBoxStateProperty); }
            set { SetValue(PreviousTextBoxStateProperty, value); }
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

        public TextBox sTextArea
        {
            get { return this.Template.FindName("txtArea", this) as TextBox;  }
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
            new FrameworkPropertyMetadata(new Size(100,50), FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits));

        public static DependencyProperty PreviousTextBoxStateProperty = DependencyProperty.Register("PreviousTextBoxState", typeof(TextBoxActiveState), typeof(Window),
            new FrameworkPropertyMetadata(TextBoxActiveState.Active, FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits));

        #endregion


    }


    
}
