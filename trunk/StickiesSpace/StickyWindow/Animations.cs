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

namespace StickyWindow
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
            StoryboardExtender animationsRestore = stickyWindow.Template.Resources["animationsRestore"] as StoryboardExtender;

            //TODO fix handler stacking!!
            animationsRestore.Completed -= new EventHandler(animationsRestore_Completed);
            animationsRestore.Completed += new EventHandler(animationsRestore_Completed);

            animationsRestore.TargetElement = stickyWindow;
            animationsRestore.Begin(stickyWindow);
        }

        void animationsRestore_Completed(object sender, EventArgs e)
        {
            Clock c = sender as Clock;
            StoryboardExtender sbe = c.Timeline as StoryboardExtender;
            StickyWindowModel sw = sbe.TargetElement as StickyWindowModel;

            sw.MinHeight = 50;
            sw.MinWidth = 100;
            sw.ResizeMode = ResizeMode.CanResizeWithGrip;
            sw.MyWindowState = WindowState.Normal;

            MyTextBox txt = sw.sTextArea;
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


        public void ContextGrowAnimation()
        {
            Border contextCircle = stickyWindow.sContextCircle;
            
            DoubleAnimation animationsContextGrowX = new DoubleAnimation(20, TimeSpan.FromMilliseconds(250));
            animationsContextGrowX.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextGrowX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Border.WidthProperty));

            DoubleAnimation animationsContextGrowY = new DoubleAnimation(20, TimeSpan.FromMilliseconds(250));
            animationsContextGrowY.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextGrowY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Border.HeightProperty));

            DoubleAnimation animationsContextMoveX = new DoubleAnimation(0, TimeSpan.FromMilliseconds(250));
            animationsContextMoveX.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextMoveX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.LeftProperty));

            DoubleAnimation animationsContextMoveY = new DoubleAnimation(0, TimeSpan.FromMilliseconds(250));
            animationsContextMoveY.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextMoveY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.TopProperty));

            Storyboard storyContextGrow = new Storyboard();
            storyContextGrow.Children.Add(animationsContextGrowX);
            storyContextGrow.Children.Add(animationsContextGrowY);
            storyContextGrow.Children.Add(animationsContextMoveX);
            storyContextGrow.Children.Add(animationsContextMoveY);

            storyContextGrow.Begin(stickyWindow.sContextCircle);
        }


        public void ContextShrinkAnimation()
        {
            Border contextCircle = stickyWindow.sContextCircle;

            DoubleAnimation animationsContextGrowX = new DoubleAnimation(10, TimeSpan.FromMilliseconds(250));
            animationsContextGrowX.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextGrowX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Border.WidthProperty));

            DoubleAnimation animationsContextGrowY = new DoubleAnimation(10, TimeSpan.FromMilliseconds(250));
            animationsContextGrowY.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextGrowY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Border.HeightProperty));

            DoubleAnimation animationsContextMoveX = new DoubleAnimation(1.5, TimeSpan.FromMilliseconds(250));
            animationsContextMoveX.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextMoveX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.LeftProperty));

            DoubleAnimation animationsContextMoveY = new DoubleAnimation(1.5, TimeSpan.FromMilliseconds(250));
            animationsContextMoveY.SetValue(Storyboard.TargetNameProperty, contextCircle.Name);
            animationsContextMoveY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.TopProperty));

            Storyboard storyContextShrink = new Storyboard();
            storyContextShrink.Children.Add(animationsContextGrowX);
            storyContextShrink.Children.Add(animationsContextGrowY);
            storyContextShrink.Children.Add(animationsContextMoveX);
            storyContextShrink.Children.Add(animationsContextMoveY);

            storyContextShrink.Begin(stickyWindow.sContextCircle);
        }




    }

}