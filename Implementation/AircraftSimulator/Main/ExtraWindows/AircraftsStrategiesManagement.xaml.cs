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
using System.Windows.Shapes;

namespace Main.ExtraWindows
{
    /// <summary>
    /// Interaction logic for AircraftsStrategiesManagement.xaml
    /// </summary>
    public partial class AircraftsStrategiesManagement : Window
    {
        public AircraftsStrategiesManagement()
        {
            InitializeComponent();
        }

        private void enableStrategy_Click(object sender, RoutedEventArgs e)
        {
            object valueToEnable = this.listBoxWithDisabledStrategies.SelectedValue;
            if(valueToEnable != null)
            {
                MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.AddStrategy((Shooters.ShooterType)valueToEnable, 0);
            }
        }

        private void disableStrategy_Click(object sender, RoutedEventArgs e)
        {
            object valueToEnable = this.listBoxWithEnabledStrategies.SelectedValue;
            if (valueToEnable != null)
            {
                MainApplication.Instance.AircraftsManagerCommunication.ManagerInstance.RemoveStrategy((Shooters.ShooterType)valueToEnable, 0);
            }
        }
    }
}
