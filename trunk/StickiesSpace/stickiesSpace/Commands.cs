using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using StickyWindow;

namespace stickiesSpace
{

    public class StickyWindowCommands
    {
        StickyWindowAnimations animations = new StickyWindowAnimations();

        //Command Events
        public static RoutedCommand CloseCmd = new RoutedCommand();
        public static RoutedCommand MinimizeCmd = new RoutedCommand();
        public static RoutedCommand RestoreCmd = new RoutedCommand();
        public static RoutedCommand FitContentCmd = new RoutedCommand();
        public static RoutedCommand PrintCmd = new RoutedCommand();


        public ContextMenu GetContextMenu(Window stickyWindow)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem m1, m2, m3, m4, m5;

            //Commands
            CommandBinding CloseCmdBinding = new CommandBinding(CloseCmd, CloseCmdExecuted, CloseCmdCanExecute);
            CommandBinding MinimizeCmdBinding = new CommandBinding(MinimizeCmd, MinizmizeCmdExecuted, MinizmizeCmdCanExecute);
            CommandBinding FitContentCmdBinding = new CommandBinding(FitContentCmd, FitContentCmdExecuted, FitContentCmdCanExecute);
            CommandBinding RestoreCmdBinding = new CommandBinding(RestoreCmd, RestoreCmdExecuted, RestoreCmdCanExecute);
            CommandBinding PrintCmdBinding = new CommandBinding(PrintCmd, PrintCmdExecuted, PrintCmdCanExecute);

            m1 = new MenuItem();
            m1.Header = "Close";
            m1.CommandBindings.Add(CloseCmdBinding);
            m1.Command = CloseCmd;
            m1.CommandParameter = stickyWindow;

            m2 = new MenuItem();
            m2.Header = "Minimize";
            m2.CommandBindings.Add(MinimizeCmdBinding);
            m2.Command = MinimizeCmd;
            m2.CommandParameter = stickyWindow;

            m3 = new MenuItem();
            m3.Header = "Restore";
            m3.CommandBindings.Add(RestoreCmdBinding);
            m3.Command = RestoreCmd;
            m3.CommandParameter = stickyWindow;

            m4 = new MenuItem();
            m4.Header = "Fit Content";
            m4.CommandBindings.Add(FitContentCmdBinding);
            m4.Command = FitContentCmd;
            m4.CommandParameter = stickyWindow;

            m5 = new MenuItem();
            m5.Header = "Print";
            m5.CommandBindings.Add(PrintCmdBinding);
            m5.CommandParameter = stickyWindow;

            menu.Items.Add(m1);
            menu.Items.Add(m2);
            menu.Items.Add(m3);
            menu.Items.Add(m4);
            menu.Items.Add(m5);

            return menu;
        }


        #region Commands

        //Close menuCommand
        protected void CloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            stickyWindow.Close();
        }

        protected void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Restore menuCommand
        protected void RestoreCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            //MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(stickyWindow, "slider");
            //Canvas containerCanvas = (Canvas)LogicalTreeHelper.FindLogicalNode(stickyWindow, "Container");
            //TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(stickyWindow, "txtArea");

            //RestoreAnimation(stickyWindow, slider, containerCanvas, txt);
        }

        protected void RestoreAnimation(StickyWindowModel stickyWindow, MySlider slider, Canvas containerCanvas, TextBox txt)
        {
            //SetTextBoxBindings(txt, SetBindingMode.SetBinding);
            //SetContainerCanvasBindings(stickyWindow, SetBindingMode.SetBinding);

            //double currWidth = stickyWindow.ActualWidth;
            //double currHeight = stickyWindow.ActualHeight;
            //double newWidth = stickyWindow.OriginalSize.Width;
            //double newHeight = stickyWindow.OriginalSize.Height;

            //DoubleAnimation animHeight = new DoubleAnimation(currHeight, newHeight, new TimeSpan(0, 0, 0, 0, 250));
            //DoubleAnimation animWidth = new DoubleAnimation(currWidth, newHeight, new TimeSpan(0, 0, 0, 0, 250));

            //Storyboard.SetTargetName(animHeight, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            //Storyboard.SetTargetName(animWidth, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            //Storyboard storyMin = new Storyboard();
            //storyMin.Children.Add(animWidth);
            //storyMin.Children.Add(animHeight);

            //storyMin.Begin(this);

            //stickyWindow.MinHeight = 50;
            //stickyWindow.MinWidth = 100;
            //stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            //stickyWindow.MyWindowState = WindowState.Normal;
        }

        protected void RestoreCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //FitContent menuCommand
        protected void FitContentCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            //MySlider slider = (MySlider)LogicalTreeHelper.FindLogicalNode(stickyWindow, "slider");
            //TextBox txt = (TextBox)LogicalTreeHelper.FindLogicalNode(stickyWindow, "txtArea");

            //SetTextBoxBindings(txt, SetBindingMode.SetBinding);
            //SetContainerCanvasBindings(stickyWindow, SetBindingMode.SetBinding);

            //double currWidth = stickyWindow.ActualWidth;
            //double currHeight = stickyWindow.ActualHeight;

            //DoubleAnimation animHeight = new DoubleAnimation(currHeight, txt.ExtentHeight + 50, new TimeSpan(0, 0, 0, 0, 250));
            //DoubleAnimation animWidth = new DoubleAnimation(currWidth, 200, new TimeSpan(0, 0, 0, 0, 250));

            //Storyboard.SetTargetName(animHeight, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animHeight, new PropertyPath(Window.HeightProperty));

            //Storyboard.SetTargetName(animWidth, stickyWindow.Name);
            //Storyboard.SetTargetProperty(animWidth, new PropertyPath(Window.WidthProperty));

            //Storyboard storyMin = new Storyboard();
            //storyMin.Children.Add(animWidth);
            //storyMin.Children.Add(animHeight);

            //storyMin.Begin(this);

            //stickyWindow.MinHeight = 50;
            //stickyWindow.MinWidth = 100;
            //stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            //stickyWindow.MyWindowState = WindowState.Normal;
        }

        protected void FitContentCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Minimize menuCommand
        protected void MinizmizeCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            animations.MinimizeAnimation(stickyWindow);
        }



        protected void MinizmizeCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        protected void PrintCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Printstuff
        }

        protected void PrintCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

    }

}
