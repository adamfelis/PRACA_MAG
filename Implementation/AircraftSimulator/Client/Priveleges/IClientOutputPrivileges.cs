using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public interface IClientOutputPrivileges
    {
        event ServerDataReceivedHandler ServerDataReceived;

        void Subscribe(IClientPrivileges clientPrivileges);

        void OnServerDataPresented(DataEventArgs eventArgs);
    }
}
