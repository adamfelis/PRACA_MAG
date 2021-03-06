﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.Data_Manipulation;
using Client;
using Client.Priveleges;
using Common.AircraftData;
using Common.Containers;
using UnityEngine;

namespace Assets.scripts
{
    public class DataWriter : UnityNotifier, IDataWriter
    {
        private IClient communicator;
        public DataWriter(IClientPrivileges clientPrivileges, ICommunication communication, AircraftsController aircraftsController)
        {
            this.communicator = new Client.Client(clientPrivileges);
            base.aircraftsController = aircraftsController;
            base.unityShellNotifier = communication;
        }

        public void SendJoinMessage(string localIPAddress)
        {
            var aircraft = aircraftsController.Aircraft;
            string toOutput = communicator.ConnectToServer(new AircraftData(
            aircraft.V_0,
            aircraft.Theta,
            Time.fixedDeltaTime,
            1.0f //????????????????????????????????
            ), localIPAddress);
            unityShellNotifier.NotifyUnityShell(toOutput);
        }

        public void DisconnectFromServer()
        {
            communicator.DisconnectFromServer();
        }

        public void SendAircraftRequestToServer()
        {
            var aircraft = aircraftsController.Aircraft;
            //Debug.Log("rotacja przed wysłaniem" + aircraft.Body.transform.rotation.eulerAngles.x);
            string toOutput = "theta: " + aircraft.Theta + " eta: " + aircraft.Eta + ", xi: " + aircraft.Xi + ", zeta: " + aircraft.Zeta; 
            unityShellNotifier.NotifyUnityShell(toOutput);
            communicator.ClientInputPriveleges.SendDataRequest(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data()
                    {
                        Array = new AircraftData(
                            aircraft.V_0,
                            aircraft.V_e,
                            aircraft.Eta, //NI
                            aircraft.Xi,
                            aircraft.Zeta,
                            aircraft.Tau,
                            aircraft.Theta,
                            aircraft.Phi,
                            aircraft.Psi,
                            aircraft.p,
                            aircraft.q,
                            aircraft.r).GetData(),
                        MessageType = MessageType.ClientDataRequest,
                        MessageContent = MessageContent.Aircraft,
                        InputType = DataType.Matrix,
                        OutputType = DataType.Vector,//???????????????????????????
                        Response = ActionType.ResponseRequired
                    }
                }
            });
        }

        public void SendMissileRequestToServer(MissileData missileData)
        {
            var targetGameObject = aircraftsController.GetComponent<Player_ID>().GetPlayerByRemoteAssignedId(missileData.TargetId);
            var globalTargetVelocity = targetGameObject.GetComponent<Player_SyncPosition>().Velocity;

            communicator.ClientInputPriveleges.SendDataRequest(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data()
                    {
                        MissileTargetId = missileData.TargetId,
                        MissileId = missileData.MissileId,
                        ShooterId = missileData.ShooterId,
                        MessageType = MessageType.ClientDataRequest,
                        MessageContent = MessageContent.Missile,
                        MessageConcreteType = MessageConcreteType.MissileDataRequest,
                        InputType = DataType.Matrix,
                        OutputType = DataType.Vector,//???????????????????????????
                        Response = ActionType.ResponseRequired,
                        Array = missileData.GetData(),
                        Velocity = new []{ globalTargetVelocity.x, globalTargetVelocity.y, globalTargetVelocity.z},
                    }
                }
            });
        }

        public void SendMissileFiredToServer(MissileData missileData)
        {
            //var aircraft = aircraftsController.Aircraft;
            //var localVelocity = aircraft.Velocity;
            //var globalVelocity = aircraft.Body.transform.TransformDirection(localVelocity);

            var targetGameObject = aircraftsController.GetComponent<Player_ID>().GetPlayerByRemoteAssignedId(missileData.TargetId);
            var globalTargetVelocity = targetGameObject.GetComponent<Player_SyncPosition>().Velocity;

            communicator.ClientInputPriveleges.SendDataRequest(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data()
                    {
                        //Array = new MissileData(
                        //    aircraft.Velocity.x,
                        //    aircraft.Velocity.z,
                        //    aircraft.Velocity.y,
                        //    aircraft.Position.x,
                        //    aircraft.Position.y,
                        //    aircraft.Position.z).GetData(),
                        MissileTargetId = missileData.TargetId,
                        MissileId = missileData.MissileId,
                        ShooterId = missileData.ShooterId,
                        MessageType = MessageType.ClientDataRequest,
                        MessageContent = MessageContent.Missile,
                        MessageConcreteType = MessageConcreteType.MissileAddedRequest,
                        InputType = DataType.Matrix,
                        OutputType = DataType.Vector,//???????????????????????????
                        Response = ActionType.ResponseRequired,
                        Array = missileData.GetData(),
                        Velocity = new []{ globalTargetVelocity.x, globalTargetVelocity.y, globalTargetVelocity.z},
                    }
                }
            });
        }

    }
}
