using System;
using Common.EventArgs;
using Server;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Generic;
using Main.ToolsManagerCommunication;
using Main.AircraftsManagerCommunication;
using AircraftsManager.Shooter;

namespace Main.ServerCommunication
{
    public partial class ServerCommunication : Server.IServerPrivileges
    {
        public class Client
        {
            public string Name { get; set; }
            public string Aircraft { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private ObservableCollection<Client> clientsCollection;
        private Dictionary<int, Client> clientsDictionary;
        public ObservableCollection<Client> ClientsCollection
        {
            get
            {
                return clientsCollection;
            }
        }

        private Dispatcher dispatcher;
        private IAircraftsManagerCommunicationImplementor aircraftsManagerCommunicationImplementor;

        public ServerCommunication(Common.IDispatchable iDispatchable, IAircraftsManagerCommunicationImplementor toolsManagerComunicationImplementor)
        {
            this.dispatcher = iDispatchable.Dispatcher;
            this.aircraftsManagerCommunicationImplementor = toolsManagerComunicationImplementor;
            this.clientsCollection = new ObservableCollection<Client>();
            this.clientsDictionary = new Dictionary<int, Client>();
        }

        public AddClientHandler OnClientAdded => onClientAdded;

        void onClientAdded(object sender, DataEventArgs dataEventArgs)
        {
            dispatcher.BeginInvoke((Action)(()=>
            {
                Shooters.ShooterType shooterType = Shooters.ShooterType.F16;
                Client addedClient = new ServerCommunication.Client() { Name = dataEventArgs.Id.ToString(), Aircraft = shooterType.ToString() };
                this.clientsCollection.Add(addedClient);
                this.clientsDictionary.Add(dataEventArgs.Id, addedClient);
                aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.AddShooter(shooterType, dataEventArgs.Id);
            }));
        }

        public PresentDataOfTheClientHandler OnClientDataPresented => onClientDataPresented;

        void onClientDataPresented(object sender, DataEventArgs dataEventArgs)
        {
            bool a = true;
        }

        public RemoveClientHandler OnClientRemoved => onClientRemoved;

        void onClientRemoved(object sender, DataEventArgs dataEventArgs)
        {
            dispatcher.BeginInvoke((Action)(() =>
            {
                this.clientsCollection.Remove(clientsDictionary[dataEventArgs.Id]);
                this.clientsDictionary.Remove(dataEventArgs.Id);
            }));
        }
    }
}
