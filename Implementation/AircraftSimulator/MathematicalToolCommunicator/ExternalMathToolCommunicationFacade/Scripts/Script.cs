using Common.Containers;
using Common.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts
{
    abstract class Script
    {
        protected ScriptType scriptType;
        protected const String scriptName = "";

        protected abstract String ScriptName
        {
            get;
        }

        // TODO: return type has to be changed
        internal List<IData> RunScript(MLApp.MLApp mlApp, Parameters.Parameters parameters)
        {
            List<IData> result = new List<IData>();
            foreach(Parameters.Parameter param in parameters.ParametersList)
            {
                mlApp.PutWorkspaceData(param.Name.ToLower(), "base", param.Value);
            }
            
            mlApp.Execute(ScriptName);
            object objRes;
            mlApp.GetWorkspaceData("result", "base", out objRes);
            float[,] res = objRes as float[,];
            IData longitudinalResult = new Data()
            {
                Array = new float[1][] { new float[] { res[0, 0], res[0, 1], res[0, 2], res[0, 3] } },
                InputType = DataType.Vector
            };
            result.Add(longitudinalResult);
            return result;
        } 
    }
}
