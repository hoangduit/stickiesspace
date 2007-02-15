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

        public void MinimizeAnimation(StickyWindowModel stickyWindow)
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


        public void RestoreAnimation(StickyWindowModel stickyWindow)
        {
            //Restore Animation
            Storyboard animationsRestore = stickyWindow.Template.Resources["animationsRestore"] as Storyboard;
            animationsRestore.Begin(stickyWindow);

            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            stickyWindow.MyWindowState = WindowState.Normal;

            MyTextBox txt = stickyWindow.sTextArea;
            txt.ActiveState = txt.PreviousActiveState;
        }


        public void FitContentAnimation(StickyWindowModel stickyWindow)
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


    }

}