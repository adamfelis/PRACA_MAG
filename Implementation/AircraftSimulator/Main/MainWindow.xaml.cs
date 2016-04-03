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
using System.Collections.ObjectModel;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationDataContext.ApplicationDataContext applicationDataContext;
        private Common.IDispatchable dispatchable;


        public MainWindow()
        {
            this.dispatchable = new Common.Dispatchable(this.Dispatcher);
            this.applicationDataContext = new ApplicationDataContext.ApplicationDataContext(dispatchable);
            this.DataContext = this.applicationDataContext;
            InitializeComponent();
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
            this.applicationDataContext.MainApplication.ServerCommunication.ServerInstance.StopServer();
        }

        private void clientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(clientsListView.SelectedValue != null)
                this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.
                    SetActiveStrategies(int.Parse((clientsListView.SelectedValue as ServerCommunication.ServerCommunication.Client).Name));
            
        }

        private void manageStrategiesButton_Click(object sender, RoutedEventArgs e)
        {
            //this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.AddStrategy()
            ExtraWindows.AircraftsStrategiesManagement aircraftsStrategiesManagementWindow = new ExtraWindows.AircraftsStrategiesManagement();
            aircraftsStrategiesManagementWindow.Show();
            aircraftsStrategiesManagementWindow.DataContext = applicationDataContext;
        }

        private void algorithmsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applicationDataContext.MainApplication.InformAboutScriptTypeForComputations((global::Common.Scripts.ScriptType)this.algorithmsComboBox.SelectedValue);
        }
    }
}
