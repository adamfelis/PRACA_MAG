using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public class ClientOutputPrivileges : IClientOutputPrivileges
    {
        public event ServerDataReceivedHandler ServerDataReceived;
        public void Subscribe(IClientPrivileges clientPrivileges)
        {
            ServerDataReceived += clientPrivileges.ServerDataReceived;
        }

        public void OnServerDataPresented(DataEventArgs eventArgs)
        {
            ServerDataReceived?.Invoke(this, eventArgs);
        }
    }
}
