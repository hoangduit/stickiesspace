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

namespace stickiesSpace
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// 
    /// http://www.tutorials-ne.com/avalon/Handling-events/
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        public Window1()
        {
            InitializeComponent();
        }

        public void CreateStickyWindow(object sender, EventArgs e)
        {
            StickyWindowModel stickyWindow = new StickyWindowModel();
            stickyWindow.Height = 200;
            stickyWindow.Width = 200;
            stickyWindow.MinHeight = 50;
            stickyWindow.MinWidth = 100;
            stickyWindow.MyWindowState = WindowState.Normal;
            stickyWindow.Background = Brushes.Transparent;
            stickyWindow.AllowsTransparency = true;
            stickyWindow.WindowStyle = WindowStyle.None;
            stickyWindow.ResizeMode = ResizeMode.CanResizeWithGrip;

            stickyWindow.MouseLeftButtonDown += new MouseButtonEventHandler(stickyWindow_MouseLeftButtonDown);

            stickyWindow.Show();
        }


        #region Events

        public void stickyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StickyWindowModel callingWindow = (StickyWindowModel)sender;
            callingWindow.DragMove();
        }


        #endregion

    }
}