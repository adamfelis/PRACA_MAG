using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainApplication mainApplication;
        private Server.Server s;
        public MainWindow()
        {
            InitializeComponent();
            var a = MainApplication.Instance;
            a.ToolsManagerCommunication.ManagerInstance.ToString();
            s = new Server.Server();
            //var c = new Client.Client();
            //c.ConnectToServer();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            s.StopServer();
        }
    }
}
