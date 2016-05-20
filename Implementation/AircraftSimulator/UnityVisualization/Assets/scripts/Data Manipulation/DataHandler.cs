using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Priveleges;
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

        public void OnClientJoinResponse()
        {
            string toOutput = "Connected to the server";
            unityShellNotifier.NotifyUnityShell(toOutput);
            ClientResponseHandler.Invoke();
        }

        public void OnServerDataResponse(IDataList dataList)
        {
            string toOutput = "Response received from the server:";
            unityShellNotifier.NotifyUnityShell(toOutput);
            aircraftsController.aircraft.aircraftInterpolator.LockInterpolation();
            foreach (Data data in dataList.DataArray)
            {
                if (data.StrategyNumber != currentStrategy)
                    continue;
                MessageContent messageContent = data.MessageContent;
                toOutput = "strategy" + data.StrategyNumber+": " + messageContent.ToString() + ": ";
                foreach (var f in data.Array)
                {
                    toOutput += f.Aggregate(String.Empty, (current, f1) => current + (f1 + " "));
                    unityShellNotifier.NotifyUnityShell(toOutput);
                }
                if (messageContent == MessageContent.LongitudinalData)
                    handleLongitudinalData(data);
                if (messageContent == MessageContent.LateralData)
                    handleLateralData(data);
            }
            aircraftsController.aircraft.aircraftInterpolator.UnclockInterpolation();
            ClientResponseHandler.Invoke();
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
            var aircraft = aircraftsController.aircraft;
            //velocity in X axis (u)
            //aircraft.Velocity.x = data.Array[0][0];
            var velocityX = data.Array[0][0];
            //velocity in Z axis (w)
            //aircraft.Velocity.y = data.Array[0][1];
            var velocityY = data.Array[0][1];
            //rotary velocity in y axis (q)
            var q = data.Array[0][2];
            aircraft.q = q;

            aircraft.TranslateInLongitudinal(velocityX, velocityY);
            var theta = data.Array[0][3];
            unityShellNotifier.NotifyUnityShell("delta theta: " + (theta-prevTheta).ToString("n2"));
            prevTheta = theta;
            aircraft.RotateInLongitudinal(theta);
        }

        private void handleLateralData(IData data)
        {
            var aircraft = aircraftsController.aircraft;
            //velocity in Y axis (v)
            //aircraft.Velocity.z = data.Array[0][0];
            var velocityZ = data.Array[0][0];
            //rotary velocity in X axis (p)
            aircraft.p = data.Array[0][1];
            //rotary velocity in Z axis (r)
            aircraft.r = data.Array[0][2];
            //phi
            var phi = data.Array[0][3];
            //psi
            var psi = data.Array[0][4];

            aircraft.TranslateInLateral(velocityZ);
            aircraft.RotateInLateral(phi, psi);
        }
    }
}
