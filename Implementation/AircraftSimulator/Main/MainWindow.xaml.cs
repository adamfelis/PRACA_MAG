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
        public MainWindow()
        {
            InitializeComponent();
            mainApplication = MainApplication.Instance;
            mainApplication.ToolsManagerCommunication.ManagerInstance.ToString();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            mainApplication.ServerCommunication.ServerInstance.StopServer();
        }
    }
}
