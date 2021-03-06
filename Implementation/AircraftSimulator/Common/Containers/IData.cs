﻿using System;

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

    public enum MessageConcreteType
    {
        NotSet,
        MissileAddedRequest,
        MissileAddedResponse,
        MissileDataRequest,
        MissileDataResponse
    }

    public enum MessageContent
    {
        NotSet,
        Aircraft,
        Missile
    }

    public enum MessageStrategy
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
        int ServerSideId { get; set; }
        ErrorCode Error { get; set; }
        MessageType MessageType { get; set; }
        MessageContent MessageContent { get; set; }
        MessageStrategy MessageStrategy { get; set; }
        MessageConcreteType MessageConcreteType { get; set; }
        int StrategyNumber { get; set; }
        int MissileTargetId { get; set; }
        int ShooterId { get; set; }
        int MissileId { get; set; }
        /// <summary>
        /// it may be shooter or target, depending on context
        /// </summary>
        float[] Velocity { get; set; }
        ActionType Response { get; set; }
        DataType InputType { get; set; }
        Shooters.ShooterType ShooterType { get; set; }
        DataType OutputType { get; set; }
        float[][] Array { get; set; }
        float[,] Get2DimArray();
        void Set2DimArray(float[,] value);
        int N { get; }
        int M { get; }
    }
}
