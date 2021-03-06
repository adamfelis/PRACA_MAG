﻿using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.ConcreteScript
{
    sealed class ConcreteScriptLaplaceTransform : Scripts.Script
    {
        private new const String scriptName = "LaplaceTransform";

        protected override String ScriptName
        {
            get
            {
                return scriptName;
            }
        }
        internal override List<IData> RunScript(MLApp.MLApp mlApp, Parameters.Parameters parameters)
        {
            base.RunScript(mlApp, parameters);

            List<IData> result = new List<IData>();
            mlApp.Execute(ScriptName);
            object objRes;
            mlApp.GetWorkspaceData("result_from_matlab", "base", out objRes);
            float[,] res;
            if (objRes.GetType() == typeof(float[,]))
                res = (float[,])objRes;
            else
            {
                double[,] resDouble = (double[,])objRes;
                res = new float[resDouble.GetLength(0), resDouble.GetLength(1)];
                for (int i = 0; i < resDouble.GetLength(0); i++)
                {
                    for (int j = 0; j < resDouble.GetLength(1); j++)
                    {
                        res[i, j] = (float)resDouble[i, j];
                    }
                }
            }

            int strategiesAmount = res.GetLength(0);
            for (int i = 0; i < strategiesAmount; i++)
            {
                float[] lateralData = new float[] { res[i, 0], res[i, 1], res[i, 2], res[i, 3], res[i, 4] };
                float[] longitudinalData = new float[] { res[i, 5], res[i, 6], res[i, 7], res[i, 8] };
                float[] positionData = new float[] { res[i, 9], res[i, 10], res[i, 11] };
                //TODO
                // should be sent as single AircraftData result
                //AircraftData aircraftData = new AircraftData(lateralData, longitudinalData, positionData);
                IData lateralResult = new Data()
                {
                    Array = new float[1][] { lateralData },
                    InputType = DataType.Vector,
                    MessageType = MessageType.ClientDataResponse,
                    MessageStrategy = MessageStrategy.LateralData,
                    MessageContent = MessageContent.Aircraft,
                    StrategyNumber = i
                };
                IData longitudinalResult = new Data()
                {
                    Array = new float[1][] { longitudinalData },
                    InputType = DataType.Vector,
                    MessageType = MessageType.ClientDataResponse,
                    MessageStrategy = MessageStrategy.LongitudinalData,
                    MessageContent = MessageContent.Aircraft,
                    StrategyNumber = i
                };
                IData positionResult = new Data()
                {
                    Array = new float[1][] { positionData },
                    InputType = DataType.Vector,
                    MessageType = MessageType.ClientDataResponse,
                    MessageStrategy = MessageStrategy.PositionData,
                    MessageContent = MessageContent.Aircraft,
                    StrategyNumber = i
                };

                result.Add(longitudinalResult);
                result.Add(lateralResult);
                result.Add(positionResult);
            }
            return result;
        }
    }
}
