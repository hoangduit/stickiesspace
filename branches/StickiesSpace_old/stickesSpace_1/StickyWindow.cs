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

namespace stickesSpace_1
{
    public class StickyWindow : Window
    {
        static StickyWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StickyWindow), new FrameworkPropertyMetadata(typeof(StickyWindow)));
        }

    }
}
