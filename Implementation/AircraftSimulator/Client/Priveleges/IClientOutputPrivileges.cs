using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public interface IClientOutputPrivileges
    {
        event ServerDataReceivedHandler ServerDataReceived;
        event ServerDisconnectedHandler ServerDisconnected;

        void Subscribe(IClientPrivileges clientPrivileges);

        void OnServerDataPresented(DataEventArgs eventArgs);
        void OnServerDisconnected(ErrorCode error);
    }
}
