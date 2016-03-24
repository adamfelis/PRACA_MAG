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

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainApplication mainApplication;
        public MainWindow()
        {
            InitializeComponent();
            var a = MainApplication.Instance;
            a.ToolsManagerCommunication.ManagerInstance.ToString();
            var s = new Server.Server();
            //var c = new Client.Client();
            //c.ConnectToServer();
        }
    }
}
