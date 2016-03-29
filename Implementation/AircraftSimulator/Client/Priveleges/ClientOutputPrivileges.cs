using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public class ClientOutputPrivileges : IClientOutputPrivileges
    {
        public event ServerDataReceivedHandler ServerDataReceived;
        public event ServerDisconnectedHandler ServerDisconnected;
        public void Subscribe(IClientPrivileges clientPrivileges)
        {
            ServerDataReceived += clientPrivileges.ServerDataReceived;
            ServerDisconnected += clientPrivileges.ServerDisconnected;
        }

        public void OnServerDataPresented(DataEventArgs eventArgs)
        {
            ServerDataReceived?.Invoke(this, eventArgs);
        }

        public void OnServerDisconnected()
        {
            ServerDisconnected?.Invoke(this);
        }

    }
}
