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
using MahApps.Metro.Controls;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ApplicationDataContext.ApplicationDataContext applicationDataContext;
        private Common.IDispatchable dispatchable;


        public MainWindow()
        {
            this.dispatchable = new Common.Dispatchable(this.Dispatcher);
            InitializeComponent();
            this.applicationDataContext = new ApplicationDataContext.ApplicationDataContext(dispatchable,
                new Action<ToolAdapter.Tool.ITool>((ToolAdapter.Tool.ITool tool) =>
                {
                    this.tabControl.Items.Add(tool.TabItem);
                }));
            this.DataContext = this.applicationDataContext;
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
            if (clientsListView.SelectedValue != null)
            {
                this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.
                    SetActiveShooter(int.Parse((clientsListView.SelectedValue as ServerCommunication.ServerCommunication.Client).Name));
                //this.availableStrategies_textblock.Visibility = Visibility.Visible;
                this.connected_strategies_groupbox.Visibility = Visibility.Visible;
                this.AvailableStrategies.Visibility = Visibility.Visible;
                this.manageStrategiesButton.Visibility = Visibility.Visible;
                this.manageStrategiesHelpButton.Visibility = Visibility.Visible;
                this.manageStrategiesPropertiesButton.Visibility = Visibility.Visible;
                this.applicationDataContext.MainApplication.ToolsManagerCommunication.ManagerInstance.ToolsManagement.ConcreteObservableSubject.NotifySubscribersOnCompleted();
            }            
        }

        private void manageStrategiesButton_Click(object sender, RoutedEventArgs e)
        {
            this.shooter_strategies_flyout.IsOpen = !this.shooter_strategies_flyout.IsOpen;
            this.shooter_properties_flyout.IsOpen = false;
        }

        private void algorithmsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applicationDataContext.MainApplication.InformAboutScriptTypeForComputations((global::Common.Scripts.ScriptType)this.algorithmsComboBox.SelectedValue);
        }
        

        private void AvailableStrategies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Shooters.ShooterType? activeShooterType = null;
            
            if (e.AddedItems.Count > 0)
            {
                activeShooterType = (Shooters.ShooterType)(e.AddedItems[0] as Image).Tag;
                //this.A_lateral_textblock.Visibility = Visibility.Visible;
                this.A_lateral_groupbox.Visibility = Visibility.Visible;
                //this.A_longitudinal_textblock.Visibility = Visibility.Visible;
                this.A_longitudinal_groupbox.Visibility = Visibility.Visible;
                //this.B_lateral_textblock.Visibility = Visibility.Visible;
                this.B_lateral_groupbox.Visibility = Visibility.Visible;
                //this.B_longitudinal_textblock.Visibility = Visibility.Visible;
                this.B_longitudinal_groupbox.Visibility = Visibility.Visible;
                this.dataGrid_A_lateral.Visibility = Visibility.Visible;
                this.dataGrid_A_longitudinal.Visibility = Visibility.Visible;
                this.dataGrid_B_lateral.Visibility = Visibility.Visible;
                this.dataGrid_B_longitudinal.Visibility = Visibility.Visible;

                this.manageStrategiesPropertiesButton.IsEnabled = true;

                if (activeShooterType.HasValue)
                {
                    this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.ActiveShooterType = activeShooterType.Value;
                    this.applicationDataContext.UpdateStrategiesParameters(this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.GetAircraftParameters());
                }
            }

            

            applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.SetActiveStrategy(activeShooterType);
        }

        private void manageStrategiesPropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            this.shooter_properties_flyout.IsOpen = !this.shooter_properties_flyout.IsOpen;
            this.shooter_strategies_flyout.IsOpen = false;
        }

        private void manageStrategiesHelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addStrategiesButton_Click(object sender, RoutedEventArgs e)
        {
            //this.applicationDataContext.MainApplication.AircraftsManagerCommunication.ManagerInstance.AddStrategy()
            //ExtraWindows.AircraftsStrategiesManagement aircraftsStrategiesManagementWindow = new ExtraWindows.AircraftsStrategiesManagement();
            //aircraftsStrategiesManagementWindow.Show();
            //aircraftsStrategiesManagementWindow.DataContext = applicationDataContext;
            object valueToEnable = this.optional_strategies.SelectedValue;
            if (valueToEnable != null)
            {
                int client_id = MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.ActiveShooter;
                MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.AddStrategy((Shooters.ShooterType)((valueToEnable as Image).Tag), client_id);

                IData additionalInformation = MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.GetInitialAdditionalInformation(client_id);

                MainApplication.Instance.ToolsManagerCommunication.ManagerInstance.Compute(
                    global::Common.Scripts.SpecialScriptType.StrategyCreator,
                    MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.GetShooterInitalData(client_id,
                    additionalInformation));
            }
        }
    }
}
