using Common.EventArgs;

namespace Client.Priveleges
{
    public delegate void ServerDataReceivedHandler(object sender, DataEventArgs eventHandler);
    public interface IClientPrivileges
    {
        ServerDataReceivedHandler ServerDataReceived { get; }
    }
}
