using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace stickiesSpace
{
    class LauncherCommands
    {

        public LauncherCommands(Window1 windowInstance)
        {
            _window = windowInstance;
        }

        private Window1 _window;
        public Window1 window
        {
            get { return _window; }
        }


        public ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem m1, m2;

            m1 = new MenuItem();
            m1.Text = "Exit";
            m1.Click += new EventHandler(m1_Click);

            m2 = new MenuItem();
            m2.Text = "New Sticky";
            m2.Click += new EventHandler(m2_Click);

            menu.MenuItems.Add(m1);
            menu.MenuItems.Add(m2);

            return menu;
        }


        #region Commands

        //NewSticky
        void m2_Click(object sender, EventArgs e)
        {
            window.CreateStickyWindow(window, null);
        }


        //Close
        void m1_Click(object sender, EventArgs e)
        {
            window.Close();
        }  

        #endregion

    }
}
