using Common.Containers;

namespace Client.Priveleges
{
    public interface IClientOutputPrivileges
    {
        event ServerDataReceivedHandler ServerDataReceived;

        void Subscribe(IClientPrivileges clientPrivileges);

        void OnMathToolDataReceived(int id, IData data);
    }
}
