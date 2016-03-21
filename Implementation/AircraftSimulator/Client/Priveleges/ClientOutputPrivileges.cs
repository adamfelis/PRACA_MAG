using Common.Containers;
using Common.EventArgs;

namespace Client.Priveleges
{
    public class ClientOutputPrivileges : IClientOutputPrivileges
    {
        public event ClientJoinHandler ClientJoinAccepted;
        public event MathToolDataHandler MathToolDataReceived;
        public void Subscribe(IClientPrivileges clientPrivileges)
        {
            ClientJoinAccepted += clientPrivileges.OnClientJoined;
            MathToolDataReceived += clientPrivileges.OnMathToolDataReceived;
        }

        public void OnClientJoinAccepted(int id)
        {
            ClientJoinAccepted?.Invoke(this, new ClientEventArgs {Id = id});
        }

        public void OnMathToolDataReceived(int id, IData data)
        {
            MathToolDataReceived?.Invoke(this, new DataEventArgs {Id = id, Data = data });
        }
    }
}
