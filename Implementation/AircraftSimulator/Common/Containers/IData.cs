using System;

namespace Common.Containers
{
    public enum DataType
    {
        NotSet,
        Float,
        Vector,
        Matrix
    }

    public enum MessageType
    {
        NotSet,
        ClientJoinRequest,
        ClientJoinResponse,
        ClientDisconnected,
        ClientDataRequest,
        ClientDataResponse,
        ServerDisconnected
    }

    public enum ActionType
    {
        NotSet,
        ResponseRequired,
        NoResponse
    }

    public interface IData
    {
        string Sender { get; set; }
        MessageType MessageType { get; set; }
        ActionType Response { get; set; }
        DataType InputType { get; set; }
        DataType OutputType { get; set; }
        float[][] Array { get; set; }
        float [,] Get2DimArray();
        void Set2DimArray(float [,] value);
        int N { get; }
        int M { get; }
    }
}
