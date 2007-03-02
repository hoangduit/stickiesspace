using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Forms;
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


namespace StickySpaces
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : Window
    {

        #region click debugging
        //private int cDownCount = 0;
        //private int cUpCount = 0;

        //private void cDown(object sender)
        //{
        //    cDownCount += 1;
        //    txtCustomWindowDown.Text = cDownCount.ToString();
        //    downType.Text = sender.GetType().ToString();
        //}
        //private void cUp(object sender)
        //{
        //    cUpCount += 1;
        //    txtCustomWindowUp.Text = cUpCount.ToString();
        //    upType.Text = sender.GetType().ToString();
        //}
        //private void ResetCounts(object sender, RoutedEventArgs e)
        //{
        //    ClearTypes();
        //    cDownCount = 0;
        //    txtCustomWindowDown.Text = String.Empty;
        //    cUpCount = 0;
        //    txtCustomWindowUp.Text = String.Empty;
        //}
        //private void ClearTypes()
        //{
        //    upType.Text = String.Empty;
        //    downType.Text = String.Empty;
        //}
        #endregion

        static Uri iconUri = new Uri(@"pack://application:,,,/StickySpaces;component/Resources/StickyIcon.ico");
        static Stream iconStream = System.Windows.Application.GetResourceStream(iconUri).Stream;

        NotifyIcon notifyIcon;

        public Window1()
        {
            InitializeComponent();

            IconBitmapDecoder icon = new IconBitmapDecoder(iconUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
            this.Icon = icon.Frames[0];

            this.Closing += new System.ComponentModel.CancelEventHandler(Window1_Closing);
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
        }


        #region Methods

        public void CreateStickyWindow(object sender, EventArgs e)
        {
            StickyWindowModel stickyWindow = new StickyWindowModel(null);
            stickyWindow.Owner = this;
            this.RegisterName(stickyWindow.Name, stickyWindow);
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
            XmlDocument stateDoc = new XmlDocument();
            try
            {
                stateDoc.Load("stickySpacesState.xml");
                StringReader reader = new StringReader(stateDoc.InnerXml);
                XPathDocument navDoc = new XPathDocument(reader);
                XPathNavigator nav = navDoc.CreateNavigator();
                XPathExpression windowsPath = nav.Compile(@"/StickySpacesWindows/StickyWindow");
                XPathNodeIterator windows = nav.Select(windowsPath);

                if (windows != null)
                    while (windows.MoveNext())
                    {
                        StickyWindowModel newSw = new StickyWindowModel(windows.Current.OuterXml);
                        newSw.Owner = this;
                        this.RegisterName(newSw.Name, newSw);
                    }
            }
            catch (Exception ex) { }
        }

        #endregion


        #region Events

        void notifyIcon_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs args = e as System.Windows.Forms.MouseEventArgs;
            if (args.Button == MouseButtons.Left)    
                CreateStickyWindow(this, null);
        }


        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            LauncherCommands commands = new LauncherCommands(this);

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "Welcome to StickySpaces!";
            this.notifyIcon.Text = "StickySpaces";

            this.notifyIcon.Icon = new Icon(iconStream);
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(500);
            this.notifyIcon.ContextMenu = commands.GetContextMenu();
            this.notifyIcon.Click += new EventHandler(notifyIcon_Click);
            this.DeSerialzeState(this, null);

            this.Hide();
        }


        void Window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.SerialzeState(this, null);
            this.notifyIcon.Visible = false;
        }

        #endregion




    }
}