using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.ConcreteScript
{
    sealed class ConcreteScriptGetState : Scripts.Script
    {
        private new const String scriptName = "Get_State";

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
            float[,] res = objRes as float[,];

            IData lateralResult = new Data()
            {
                Array = new float[1][] { new float[] { res[0, 0], res[0, 1], res[0, 2], res[0, 3], res[0, 4] } },
                InputType = DataType.Vector,
                MessageType = MessageType.ClientDataResponse
            };
            IData longitudinalResult = new Data()
            {
                Array = new float[1][] { new float[] { res[0, 5], res[0, 6], res[0, 7], res[0, 8] } },
                InputType = DataType.Vector,
                MessageType = MessageType.ClientDataResponse
            };

            result.Add(longitudinalResult);
            result.Add(lateralResult);

            return result;
        }
    }
}
