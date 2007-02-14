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

            //Storyboard animationsMinimize = ResourceDictionary

            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri(@";StickyWindow\themes/generic.xaml", UriKind.Relative);


            int i = 0;
            //animationsMinimize.Begin(stickyWindow);



            //SetTextBoxBindings(txt, SetBindingMode.ClearBinding);
            //SetContainerCanvasBindings(stickyWindow, SetBindingMode.SetBinding);
            //stickyWindow.MinHeight = 0;
            //stickyWindow.MinWidth = 0;
            //stickyWindow.ResizeMode = ResizeMode.NoResize;
            //stickyWindow.RenderTransform = new RotateTransform(0, 10, 10);
            //stickyWindow.OriginalSize = new Size(stickyWindow.ActualWidth, stickyWindow.ActualHeight);
            //stickyWindow.MyWindowState = WindowState.Minimized;


            //double currWidth = stickyWindow.ActualWidth;
            //double currHeight = stickyWindow.ActualHeight;

            ////DoubleAnimation animSpin = new DoubleAnimation(0, 360, new TimeSpan(0, 0, 0, 0, 500));
            //DoubleAnimation animHeight = new DoubleAnimation(currHeight, 20, new TimeSpan(0, 0, 0, 0, 250));
            //DoubleAnimation animWidth = new DoubleAnimation(currWidth, 20, new TimeSpan(0, 0, 0, 0, 250));

            ////Storyboard.SetTargetName(animSpin, stickyWindow.Name);
            ////Storyboard.SetTargetProperty(animSpin, new PropertyPath("(0).(1)", 
            ////    new DependencyProperty[] {
            ////        UIElement.RenderTransformProperty,
            ////        RotateTransform.AngleProperty}));

            //Storyboard.SetTargetName(animHeight, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            //Storyboard.SetTargetName(animWidth, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            //Storyboard storyMin = new Storyboard();
            ////storyMin.Children.Add(animSpin);
            //storyMin.Children.Add(animWidth);
            //storyMin.Children.Add(animHeight);

            //storyMin.Begin(this);
        }


        protected void RestoreAnimation(StickyWindowModel stickyWindow)
        {
            //Restore Animation
        }


        protected void FitContentAnimation(StickyWindowModel stickyWindow)
        {
            //FitContent Animation
        }


    }

}