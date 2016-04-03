using System.Collections.ObjectModel;

namespace Main.ServerCommunication
{
    interface IServerCommunication
    {
        Server.IServer ServerInstance
        {
            get;
        }
        ObservableCollection<ServerCommunication.Client> ClientsCollection
        {
            get;
        }
    }
}
