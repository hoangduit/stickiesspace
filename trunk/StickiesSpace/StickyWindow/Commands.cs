using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using StickyWindow;

namespace StickyWindow
{

    public class StickyWindowCommands
    {

        public StickyWindowCommands(StickyWindowModel stickyWindowInstance)
        {
            _stickyWindow = stickyWindowInstance;
        }

        private StickyWindowModel _stickyWindow;
        public StickyWindowModel stickyWindow
        {
            get { return _stickyWindow; }
            set { _stickyWindow = value; }
        }


        public ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem m1, m2, m3, m4, m5, m6;

            CommandBinding CloseCmdBinding = new CommandBinding(StickyWindowModel.CloseCmd, CloseCmdExecuted, CloseCmdCanExecute);
            CommandBinding MinimizeCmdBinding = new CommandBinding(StickyWindowModel.MinimizeCmd, MinizmizeCmdExecuted, MinizmizeCmdCanExecute);
            CommandBinding FitContentCmdBinding = new CommandBinding(StickyWindowModel.FitContentCmd, FitContentCmdExecuted, FitContentCmdCanExecute);
            CommandBinding RestoreCmdBinding = new CommandBinding(StickyWindowModel.RestoreCmd, RestoreCmdExecuted, RestoreCmdCanExecute);
            CommandBinding PrintCmdBinding = new CommandBinding(StickyWindowModel.PrintCmd, PrintCmdExecuted, PrintCmdCanExecute);
            CommandBinding ColorsCmdBinding = new CommandBinding(StickyWindowModel.ColorsCmd, ColorsCmdExecuted, ColorsCmdCanExecute);


            m1 = new MenuItem();
            m1.Header = "Close";
            m1.CommandBindings.Add(CloseCmdBinding);
            m1.Command = StickyWindowModel.CloseCmd;
            m1.InputGestureText = "Ctrl-T";

            m2 = new MenuItem();
            m2.Header = "Minimize";
            m2.CommandBindings.Add(MinimizeCmdBinding);
            m2.Command = StickyWindowModel.MinimizeCmd;

            m3 = new MenuItem();
            m3.Header = "Restore";
            m3.CommandBindings.Add(RestoreCmdBinding);
            m3.Command = StickyWindowModel.RestoreCmd;

            m4 = new MenuItem();
            m4.Header = "Fit Content";
            m4.CommandBindings.Add(FitContentCmdBinding);
            m4.Command = StickyWindowModel.FitContentCmd;

            m5 = new MenuItem();
            m5.Header = "Print";
            m5.CommandBindings.Add(PrintCmdBinding);

            m6 = new MenuItem();
            m6.Header = "Colors";
            m6.CommandBindings.Add(ColorsCmdBinding);
            m6.Command = StickyWindowModel.ColorsCmd;

            menu.Items.Add(m1);
            menu.Items.Add(m2);
            menu.Items.Add(m3);
            menu.Items.Add(m4);
            menu.Items.Add(m5);
            menu.Items.Add(m6);

            return menu;
        }


        #region Commands

        //Close menuCommand
        public void CloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            stickyWindow.Close();
        }

        public void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Restore menuCommand
        public void RestoreCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.RestoreAnimation();
        }

        public void RestoreCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //FitContent menuCommand
        public void FitContentCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.FitContentAnimation();
        }

        public void FitContentCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        //Minimize menuCommand
        public void MinizmizeCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StickyWindowAnimations animations = new StickyWindowAnimations(stickyWindow);
            animations.MinimizeAnimation();
        }

        public void MinizmizeCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (bool)(stickyWindow.MyWindowState == WindowState.Normal);
        }


        public void PrintCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Printstuff
        }

        public void PrintCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public void ColorsCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            stickyWindow.ShowColorsControl();
        }

        public void ColorsCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !stickyWindow.isColorWindowOpen;
        }

        #endregion

    }

}
