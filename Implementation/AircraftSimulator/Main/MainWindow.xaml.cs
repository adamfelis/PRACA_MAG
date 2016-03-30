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
using Common.Containers;

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
            //mainApplication.ToolsManagerCommunication.ManagerInstance.ToString();
            //mainApplication.AircraftsManagerCommunication.ManagerInstance.AddShooter(AircraftsManager.Shooter.ShooterType.F16, 1);
            //List<IData> data = mainApplication.AircraftsManagerCommunication.ManagerInstance.GetShooterData(1);
            //foreach(IData concreteData in data)
            //{
            //    mainApplication.ToolsManagerCommunication.ManagerInstance.Compute(concreteData);
            //}
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            mainApplication.ServerCommunication.ServerInstance.StopServer();
        }
    }
}
