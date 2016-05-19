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
        ClientAcceptDisconnection,
        ServerDisconnected
    }

    public enum MessageContent
    {
        NotSet,
        LateralData,
        LongitudinalData,
        PositionData
    }

    public enum ActionType
    {
        NotSet,
        ResponseRequired,
        NoResponse
    }

    public enum ErrorCode
    {
        None,
        MainApplicationException,
        WriteOperation,
        ReadOperation,
        StreamClosed,
        ServerException,
        ClientException
    }

    public interface IData
    {
        string Sender { get; set; }
        ErrorCode Error { get; set; }
        MessageType MessageType { get; set; }
        MessageContent MessageContent { get; set; }
        int StrategyNumber { get; set; }
        ActionType Response { get; set; }
        DataType InputType { get; set; }
        Shooters.ShooterType ShooterType { get; set; }
        DataType OutputType { get; set; }
        float[][] Array { get; set; }
        float [,] Get2DimArray();
        void Set2DimArray(float [,] value);
        int N { get; }
        int M { get; }
    }
}
