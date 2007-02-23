using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using StickyWindow;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace stickiesSpace
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        #region click debugging
        private int cDownCount = 0;
        private int cUpCount = 0;

        private void cDown(object sender)
        {
            cDownCount += 1;
            txtCustomWindowDown.Text = cDownCount.ToString();
            downType.Text = sender.GetType().ToString();
        }
        private void cUp(object sender)
        {
            cUpCount += 1;
            txtCustomWindowUp.Text = cUpCount.ToString();
            upType.Text = sender.GetType().ToString();
        }
        private void ResetCounts(object sender, RoutedEventArgs e)
        {
            ClearTypes();
            cDownCount = 0;
            txtCustomWindowDown.Text = String.Empty;
            cUpCount = 0;
            txtCustomWindowUp.Text = String.Empty;
        }
        private void ClearTypes()
        {
            upType.Text = String.Empty;
            downType.Text = String.Empty;
        }
        #endregion


        public Window1()
        {
            InitializeComponent();
        }


        public void CreateStickyWindow(object sender, EventArgs e)
        {
            StickyWindowModel stickyWindow = new StickyWindowModel();
            stickyWindow.Owner = this;
            stickyWindow.Show();
            stickyWindow.Initialize();
        }


        public void SerialzeState(object sender, EventArgs e)
        {
            XmlTextWriter serializeWriter = new XmlTextWriter("stickySpacesState.xml", Encoding.UTF8);

            serializeWriter.WriteStartDocument();
            serializeWriter.WriteStartElement("StickySpacesWindows");

            foreach (StickyWindowModel sw in this.OwnedWindows)
                serializeWriter.WriteRaw(sw.Serialize());

            serializeWriter.WriteEndDocument();
            serializeWriter.Flush();
            serializeWriter.Close();
        }


        public void DeSerialzeState(object sender, EventArgs e)
        {
            XmlReader reader = new XmlTextReader("stickySpacesState.xml");
            reader.MoveToContent();

            while (reader.MoveToContent() == XmlNodeType.Element)
            {
                StickyWindowModel newSw = new StickyWindowModel();
                newSw.Owner = this;
                newSw.Show();
                newSw.Deserialize(reader.ReadInnerXml());
                newSw.Initialize();
            }

        }


    }
}