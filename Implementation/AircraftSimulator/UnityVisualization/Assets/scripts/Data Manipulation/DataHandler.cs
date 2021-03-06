﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.Model;
using Client.Priveleges;
using Common.AircraftData;
using Common.Containers;
using Common.EventArgs;
using UnityEngine;

namespace Assets.scripts.Data_Manipulation
{
    public class DataHandler : UnityNotifier, IDataHandler 
    {
        public event ClientResponseHandler ClientResponseHandler;
        public event ClientResponseHandler ClientDisconnected;

        private int currentStrategy = 0;
        public DataHandler (ICommunication communication, AircraftsController aircraftsController)
        {
            base.aircraftsController = aircraftsController;
            base.unityShellNotifier = communication;
        }

        public void OnClientJoinResponse(IData data)
        {
            string toOutput = "Connected to the server";
            unityShellNotifier.NotifyUnityShell(toOutput);

            aircraftsController.gameObject.GetComponent<Player_ID>().SetupRemotelyAssignedId(data.ServerSideId);

            ClientResponseHandler.Invoke();
        }

        private void discoverMessageContent(IDataList dataList)
        {
            Data firstData = dataList.DataArray.First();
            MessageContent messageContent = firstData.MessageContent;
            switch (messageContent)
            {
                case MessageContent.Aircraft:
                    aircraftsController.Aircraft.aircraftInterpolator.LockInterpolation();
                    processAircraftData(messageContent, dataList);
                    aircraftsController.Aircraft.aircraftInterpolator.UnclockInterpolation();
                    ClientResponseHandler.Invoke();
                    break;
                case MessageContent.Missile:
                    processMissileData(messageContent, dataList);

                    break;
            }
        }

        private void processAircraftData(MessageContent messageContent, IDataList dataList)
        {
            foreach (Data data in dataList.DataArray)
            {
                if (data.StrategyNumber != currentStrategy)
                    continue;
                MessageStrategy messageStrategy = data.MessageStrategy;
                parseDataToString(messageContent, messageStrategy, data);
                if (messageStrategy == MessageStrategy.LongitudinalData)
                    handleLongitudinalData(data);
                if (messageStrategy == MessageStrategy.LateralData)
                    handleLateralData(data);
                if (messageStrategy == MessageStrategy.PositionData)
                    handlePositionData(data);
            }
            aircraftsController.FirstResponseReceived = true;
        }

        private void parseDataToString(MessageContent messageContent, MessageStrategy messageStrategy, Data data)
        {
            string toOutput = "messageContent: " + messageContent + " strategy:" + data.StrategyNumber + ": " +
                  messageStrategy.ToString() + ": ";
            if (data.Array == null)
                return;
            foreach (var f in data.Array)
            {
                toOutput += f.Aggregate(String.Empty, (current, f1) => current + (f1 + " "));
            }
            unityShellNotifier.NotifyUnityShell(toOutput);
        }

        private void processMissileData(MessageContent messageContent, IDataList dataList)
        {
            foreach (Data data in dataList.DataArray)
            {
                if (data.StrategyNumber != currentStrategy)
                    continue;
                MessageConcreteType messageConcreteType = data.MessageConcreteType;
                MessageStrategy messageStrategy = data.MessageStrategy;
                parseDataToString(messageContent, messageStrategy, data);
                switch (messageConcreteType)
                {
                    case MessageConcreteType.MissileAddedResponse:
                        unityShellNotifier.NotifyUnityShell("Missile added response received");
                        aircraftsController.MissileController.BeginFly(data.ShooterId, data.MissileTargetId, data.MissileId);
                        break;
                    case MessageConcreteType.MissileDataResponse:
                        MissileData missileData = new MissileData(data.Array);
                        Vector3 position = new Vector3(missileData.X_0, missileData.Y_0, missileData.Z_0);
                        //Vector3 position = new Vector3(missileData.X_0, missileData.Y_0, missileData.Z_0);
                        aircraftsController.MissileController.UpdateMissilePosition(data.MissileId, data.MissileTargetId,
                            position);
                        break;
                }
            }
        }

        public void OnServerDataResponse(IDataList dataList)
        {
            //return;
            string toOutput = "Response received from the server:";
            unityShellNotifier.NotifyUnityShell(toOutput);
            discoverMessageContent(dataList);
        }

        public void OnServerDisconnected()
        {
            ClientDisconnected.Invoke();
            string toOutput = "Disconnected from the server";
            unityShellNotifier.NotifyUnityShell(toOutput);
        }

        private float prevTheta = 0.0f;
        private void handleLongitudinalData(IData data)
        {
            var aircraft = aircraftsController.Aircraft;
            //velocity in X axis (u)
            var velocityZ = -data.Array[0][0];
            //velocity in Z axis (w)
            var velocityY = data.Array[0][1];
            //rotary velocity in y axis (q)
            aircraft.q = data.Array[0][2];

            ///NOTE MINUS
            var theta = data.Array[0][3];
            //unityShellNotifier.NotifyUnityShell("delta theta: " + (theta-prevTheta).ToString("n2"));
            prevTheta = theta;
            aircraft.RotateInLongitudinal(theta);
            aircraft.SetupVelocityInLongitudinal(velocityZ, velocityY);
        }
        private void handlePositionData(IData data)
        {
            var aircraft = aircraftsController.Aircraft;
            var deltaX = data.Array[0][0];
            var deltaY = data.Array[0][2];
            var deltaZ = data.Array[0][1];

            aircraft.TranslateInLongitudinal(deltaX, deltaY);
            aircraft.TranslateInLateral(deltaZ);
        }

        private void handleLateralData(IData data)
        {
            var aircraft = aircraftsController.Aircraft;
            //velocity in Y axis (v)
            var velocityX = data.Array[0][0];
            //rotary velocity in X axis (p)
            aircraft.p = data.Array[0][1];
            //rotary velocity in Z axis (r)
            aircraft.r = data.Array[0][2];
            //phi
            var phi = data.Array[0][3];
            //psi
            var psi = data.Array[0][4];

            aircraft.RotateInLateral(phi, psi);
            aircraft.SetupVelocityInLateral(velocityX);
        }
    }
}
