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
using System.Windows.Media.Animation;

namespace StickyWindow
{

    public class MyTextBox : TextBox
    {

        #region Private Properties

        private TextBoxActiveState _PreviousActiveState;

        #endregion


        #region Public Properties

        public TextBoxActiveState ActiveState
        {
            get 
            {
                if (this.Focusable)
                    return TextBoxActiveState.Active;
                else
                    return TextBoxActiveState.Inactive;
            }
            set 
            {
                switch (value)
                {
                    case TextBoxActiveState.Active :
                        ActivateTextBox((StickyWindowModel)this.TemplatedParent);
                        break;

                    case TextBoxActiveState.Inactive :
                        DeactivateTextBox();
                        break;
                }
            }
        }

        public TextBoxActiveState PreviousActiveState
        {
            get { return _PreviousActiveState; }
            set { _PreviousActiveState = value; }
        }

        #endregion


        #region Methods

        protected void ActivateTextBox(StickyWindowModel stickyWindow)
        {
            if (stickyWindow.MyWindowState != WindowState.Minimized)
            {
                this.Focusable = true;
                this.SetValue(TextBox.CursorProperty, Cursors.IBeam);
            }

        }


        protected void DeactivateTextBox()
        {
            this.Focusable = false;
            this.SetValue(TextBox.CursorProperty, Cursors.Arrow);
        }

        #endregion
    }

}