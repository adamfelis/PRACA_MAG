using Common.Containers;

namespace Client.Priveleges
{
    public interface IClientOutputPrivileges
    {
        event ClientJoinHandler ClientJoinAccepted;
        event MathToolDataHandler MathToolDataReceived;

        void Subscribe(IClientPrivileges clientPrivileges);

        void OnClientJoinAccepted(int id);
        void OnMathToolDataReceived(int id, IData data);
    }
}
