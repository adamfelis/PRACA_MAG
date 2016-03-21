using Common.EventArgs;

namespace Client.Priveleges
{
    public delegate void MathToolDataHandler(object sender, DataEventArgs eventHandler);
    public delegate void ClientJoinHandler(object sender, ClientEventArgs eventHandler);
    public interface IClientPrivileges
    {
        MathToolDataHandler OnMathToolDataReceived { get; set; }
        ClientJoinHandler OnClientJoined { get; set; }
    }
}
