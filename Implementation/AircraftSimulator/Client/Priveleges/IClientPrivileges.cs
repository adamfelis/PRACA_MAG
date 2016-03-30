using Common.EventArgs;

namespace Client.Priveleges
{
    public delegate void ServerDataReceivedHandler(object sender, DataEventArgs eventHandler);
    public delegate void ServerDisconnectedHandler(object sender);
    public interface IClientPrivileges
    {
        ServerDataReceivedHandler ServerDataReceived { get; }
        ServerDisconnectedHandler ServerDisconnected { get; }
    }
}
