using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public delegate void ServerDataReceivedHandler(object sender, DataEventArgs eventHandler);
    public delegate void ServerDisconnectedHandler(object sender, ErrorCode e);
    public interface IClientPrivileges
    {
        ServerDataReceivedHandler ServerDataReceived { get; }
        ServerDisconnectedHandler ServerDisconnected { get; }
    }
}
