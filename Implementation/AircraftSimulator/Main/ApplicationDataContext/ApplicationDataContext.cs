using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Main.ApplicationDataContext
{
    class ApplicationDataContext
    {
        public MainApplication MainApplication
        {
            get
            {
                return MainApplication.Instance;
            }
        }

        public ObservableCollection<ServerCommunication.ServerCommunication.Client> ClientsCollection
        {
            get
            {
                return MainApplication.ServerCommunication.ClientsCollection;
            }
        }

        public Collection<global::Common.Scripts.ScriptType> AlgorithmsCollection
        {
            get
            {
                return new Collection<global::Common.Scripts.ScriptType>(Enum.GetValues(typeof(global::Common.Scripts.ScriptType)).
                    OfType<global::Common.Scripts.ScriptType>().ToList());
            }
        }
        

        public ObservableCollection<Shooters.ShooterType> OptionalStrategiesCollection
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.OptionalStrategies;
            }
        }

        public ObservableCollection<Shooters.ShooterType> StrategiesCollection
        {
            get
            {
                //int selectedItem = int.Parse((listView.SelectedValue as ServerCommunication.ServerCommunication.Client).Name);
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.ActiveStrategies;
            }
        }



        public ApplicationDataContext(Common.IDispatchable iDispatchable)
        {
            MainApplication.InformAboutDispatcher(iDispatchable);
        }

    }
}
