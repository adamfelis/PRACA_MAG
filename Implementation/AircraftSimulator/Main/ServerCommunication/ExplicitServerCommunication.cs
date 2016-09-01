using System;
using Common.EventArgs;
using Server;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using Main.ToolsManagerCommunication;
using Main.AircraftsManagerCommunication;
using AircraftsManager.Shooter;
using Common.AircraftData;
using Common.Containers;
using Patterns.Executors;
using Server.Executors;

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
        private IToolsManagerCommunicationImplementor toolsManagerCommunicationImplementor;

        public ServerCommunication(Common.IDispatchable iDispatchable, IAircraftsManagerCommunicationImplementor aircraftsManagerCommunicationImplementor,
            IToolsManagerCommunicationImplementor toolsManagerCommunicationImplementor)
        {
            this.dispatcher = iDispatchable.Dispatcher;
            this.aircraftsManagerCommunicationImplementor = aircraftsManagerCommunicationImplementor;
            this.toolsManagerCommunicationImplementor = toolsManagerCommunicationImplementor;
            this.clientsCollection = new ObservableCollection<Client>();
            this.clientsDictionary = new Dictionary<int, Client>();
        }

        public AddClientHandler OnClientAdded => onClientAdded;

        void onClientAdded(object sender, DataEventArgs dataEventArgs, IClientAddedExecutor clientAddedExecutor)
        {
            clientAddedExecutor.SetupAndRun(dispatcher, new Action(() =>
            {
                Shooters.ShooterType shooterType = dataEventArgs.DataList.DataArray.First().ShooterType;
                Client addedClient = new ServerCommunication.Client() { Name = dataEventArgs.Id.ToString(), Aircraft = shooterType.ToString() };
                this.clientsCollection.Add(addedClient);
                this.clientsDictionary.Add(dataEventArgs.Id, addedClient);
                this.aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.AddShooter(shooterType, dataEventArgs.Id);
                this.toolsManagerCommunicationImplementor.ToolsManagerCommunication.ManagerInstance.Compute(
                    global::Common.Scripts.SpecialScriptType.WorkspaceInitializator,
                    aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.GetShooterInitalData(dataEventArgs.Id, dataEventArgs.DataList.DataArray.First()));

            }));
        }

        public PresentDataOfTheClientHandler OnClientDataPresented => onClientDataPresented;

        void onClientDataPresented(object sender, DataEventArgs dataEventArgs, IClientDataPresentedExecutor clientDataPresentedExecutor)
        {
            clientDataPresentedExecutor.SetupAndRun(dispatcher, (() =>
            {
                if (dataEventArgs.DataList.DataArray.First().MessageContent == MessageContent.Missile)
                {
                    if (dataEventArgs.DataList.DataArray.First().MessageConcreteType == MessageConcreteType.MissileDataRequest)
                    {
                        List<IData> parameters = aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication
                            .ManagerInstance.GetMissileData(
                                dataEventArgs.Id, dataEventArgs.DataList.DataArray.First().MissileId, dataEventArgs.DataList.DataArray.First().Velocity);
                        MissileData missileData = new MissileData(dataEventArgs.DataList.DataArray.First().Array);
                        parameters.Add(new Data() { InputType = DataType.Vector, Array = new float[1][] { new float[3] { missileData.ShooterX, missileData.ShooterY, missileData.ShooterZ } }, Sender = "shooter_position" });
                        parameters.Add(new Data() { InputType = DataType.Vector, Array = new float[1][] { new float[3] { missileData.TargetX, missileData.TargetY, missileData.TargetZ } }, Sender = "target_position" });



                        List<IData> result = this.toolsManagerCommunicationImplementor.ToolsManagerCommunication.ManagerInstance.Compute(
                            global::Common.Scripts.SpecialScriptType.SimulateMissile, parameters
                            );
                        foreach (var data in result)
                        {
                            data.MissileTargetId = dataEventArgs.DataList.DataArray.First().MissileTargetId;
                            data.ShooterId = dataEventArgs.DataList.DataArray.First().ShooterId;
                            data.MissileId = dataEventArgs.DataList.DataArray.First().MissileId;
                        }
                        return result;
                    }

                    if (dataEventArgs.DataList.DataArray.First().MessageConcreteType == MessageConcreteType.MissileAddedRequest)
                    {
                        this.aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.AddMissile(
                            AircraftsManager.Missile.MissileType.M1, dataEventArgs.Id,
                            dataEventArgs.DataList.DataArray.First().MissileId,
                            dataEventArgs.DataList.DataArray.First().MissileTargetId
                            );
                        List<IData> parameters = aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance
                            .GetMissileData(dataEventArgs.Id, dataEventArgs.DataList.DataArray.First().MissileId, dataEventArgs.DataList.DataArray.First().Velocity);

                        MissileData missileData = new MissileData(dataEventArgs.DataList.DataArray.First().Array);
                        parameters.Add(new Data() { InputType = DataType.Vector, Array = new float[1][] { new float[3] { missileData.ShooterX, missileData.ShooterY, missileData.ShooterZ } }, Sender = "shooter_position" });
                        parameters.Add(new Data() { InputType = DataType.Vector, Array = new float[1][] { new float[3] { missileData.TargetX, missileData.TargetY, missileData.TargetZ } }, Sender = "target_position" });

                        this.toolsManagerCommunicationImplementor.ToolsManagerCommunication.ManagerInstance.Compute(
                            global::Common.Scripts.SpecialScriptType.MissileAdder, parameters
                            );
                        return new List<IData>()
                            {
                                new Data()
                                {
                                   MessageContent = MessageContent.Missile,
                                   MessageConcreteType = MessageConcreteType.MissileAddedResponse,
                                   MessageType = MessageType.ClientDataResponse,
                                   MissileTargetId = dataEventArgs.DataList.DataArray.First().MissileTargetId,
                                   ShooterId = dataEventArgs.DataList.DataArray.First().ShooterId,
                                   MissileId = dataEventArgs.DataList.DataArray.First().MissileId
                                }
                            };
                    }
                    return null;
                }

                else if (dataEventArgs.DataList.DataArray.First().MessageContent == MessageContent.Aircraft)
                {
                    List<IData> result = this.toolsManagerCommunicationImplementor.ToolsManagerCommunication.ManagerInstance.Compute(
                   aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.GetShooterData(dataEventArgs.Id, dataEventArgs.DataList.DataArray.First())
                   );
                    if (dataEventArgs.Id == this.aircraftsManagerCommunicationImplementor.AircraftsManagerCommunication.ManagerInstance.ActiveShooter)
                        this.toolsManagerCommunicationImplementor.ToolsManagerCommunication.ManagerInstance.ToolsManagement.ConcreteObservableSubject.NotifySubscribersOnNext(result);
                    return result;
                }
                else
                    return null;
            }));

        }

        public RemoveClientHandler OnClientRemoved => onClientRemoved;

        void onClientRemoved(object sender, DataEventArgs dataEventArgs, IClientRemovedExecutor clientRemovedExecutor)
        {
            clientRemovedExecutor.SetupAndRun(dispatcher, new Action(() =>
            {
                this.clientsCollection.Remove(clientsDictionary[dataEventArgs.Id]);
                this.clientsDictionary.Remove(dataEventArgs.Id);
                //if needed it could be removed from aircraftsManager
            }));
        }
    }
}
