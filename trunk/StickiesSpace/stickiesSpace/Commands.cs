using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using StickyWindow;

namespace stickiesSpace
{

    public class StickyWindowCommands
    {        

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

            
            //TODO Fix Keygestures!
            CloseCmd.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));


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
            m1.InputGestureText = "Ctrl-T";

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
            StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.RestoreAnimation();
        }

        protected void RestoreCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //FitContent menuCommand
        protected void FitContentCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.FitContentAnimation();
        }

        protected void FitContentCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Minimize menuCommand
        protected void MinizmizeCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowModel stickyWindow = (StickyWindowModel)e.Parameter;
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.MinimizeAnimation();
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
