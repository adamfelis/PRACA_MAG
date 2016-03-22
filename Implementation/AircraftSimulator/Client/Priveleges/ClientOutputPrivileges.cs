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

        public void OnMathToolDataReceived(int id, IData data)
        {
            ServerDataReceived?.Invoke(this, new DataEventArgs {Id = id, Data = data });
        }
    }
}
