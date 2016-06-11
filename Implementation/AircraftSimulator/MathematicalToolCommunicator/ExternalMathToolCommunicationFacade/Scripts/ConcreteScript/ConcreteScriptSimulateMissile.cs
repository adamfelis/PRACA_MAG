using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.AircraftData;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.ConcreteScript
{
    sealed class ConcreteScriptSimulateMissile : Script
    {
        private new const String scriptName = "SimulateMissile";
        protected override string ScriptName
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

            //var shooterId =((float[]) (parameters.ParametersList[0].Value))[0];
            //var missileId = ((float[])(parameters.ParametersList[1].Value))[0];
            //var targetId = ((float[])(parameters.ParametersList[2].Value))[0];


            int strategiesAmount = res.GetLength(0);
            for (int i = 0; i < strategiesAmount; i++)
            {
                IData deltaPosResult = new Data()
                {
                    Array = new MissileData(res[i, 3], res[i, 4], res[i, 5], res[i, 0], res[i, 1], res[i, 2]).GetData(),
                    InputType = DataType.Vector,
                    MessageType = MessageType.ClientDataResponse,
                    MessageStrategy = MessageStrategy.PositionData,
                    MessageContent = MessageContent.Missile,
                    MessageConcreteType = MessageConcreteType.MissileDataResponse,
                    StrategyNumber = i,
                    //MissileTargetId = (int)targetId,
                    //ShooterId = (int)shooterId,
                    //MissileId = (int)missileId
            };
                result.Add(deltaPosResult);
            }
            return result;
        }
    }
}
