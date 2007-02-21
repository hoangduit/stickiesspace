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

namespace stickiesSpace
{

    public class StickyWindowAnimations
    {

        public StickyWindowAnimations(StickyWindowModel stickyWindowInstance)
        {
            _stickyWindow = stickyWindowInstance;
        }
        
        private StickyWindowModel _stickyWindow;
        public StickyWindowModel stickyWindow
        {
            get { return _stickyWindow; }
            set { _stickyWindow = value; }
        }


        public void MinimizeAnimation()
        {
            //Minimize Animation
            stickyWindow.MinHeight = 0;
            stickyWindow.MinWidth = 0;
            stickyWindow.ResizeMode = ResizeMode.NoResize;
            stickyWindow.OriginalSize = new Size(stickyWindow.ActualWidth, stickyWindow.ActualHeight);
            stickyWindow.MyWindowState = WindowState.Minimized;

            MyTextBox txt = stickyWindow.sTextArea;
            txt.PreviousActiveState = txt.ActiveState;
            txt.ActiveState = TextBoxActiveState.Inactive;

            Storyboard animationsMinimize = stickyWindow.Template.Resources["animationsMinimize"] as Storyboard;
            animationsMinimize.Begin(stickyWindow);
        }


        public void RestoreAnimation()
        {
            //Restore Animation
            Storyboard animationsRestore = stickyWindow.Template.Resources["animationsRestore"] as Storyboard;
            animationsRestore.Completed += new EventHandler(animationsRestore_Completed);
            animationsRestore.Begin(stickyWindow);
        }

        void animationsRestore_Completed(object sender, EventArgs e)
        {
            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            stickyWindow.MyWindowState = WindowState.Normal;

            MyTextBox txt = stickyWindow.sTextArea;
            txt.ActiveState = txt.PreviousActiveState;
        }


        public void FitContentAnimation()
        {
            //FitContent Animation
            Storyboard animationsFitContent = stickyWindow.Template.Resources["animationsFitContent"] as Storyboard;
            TextBox txt = stickyWindow.sTextArea;

            DoubleAnimation animHeight = animationsFitContent.Children[0] as DoubleAnimation;
            animHeight.To = txt.ExtentHeight + 30;
            animationsFitContent.Begin(stickyWindow);

            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            stickyWindow.MyWindowState = WindowState.Normal;
        }


        public void MoveToFitColorControlsParentAnimation()
        {
            Storyboard animationsMoveToFitColorControlsParent = stickyWindow.Template.Resources["animationsMoveToFitColorControlsParent"] as Storyboard;
            animationsMoveToFitColorControlsParent.Completed += new EventHandler(animationsMoveToFitColorControlsParent_Completed);
            animationsMoveToFitColorControlsParent.Begin(stickyWindow);
        }

        void animationsMoveToFitColorControlsParent_Completed(object sender, EventArgs e)
        {
            stickyWindow.ShowColorsControl();
        }


    }

}