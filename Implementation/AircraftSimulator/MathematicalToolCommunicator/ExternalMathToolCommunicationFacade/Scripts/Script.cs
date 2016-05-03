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
        internal virtual List<IData> RunScript(MLApp.MLApp mlApp, Parameters.Parameters parameters)
        {
            foreach(Parameters.Parameter param in parameters.ParametersList)
            {
                mlApp.PutWorkspaceData(param.Name.ToLower(), "base", param.Value);
            }

            return new List<IData>();
        } 
    }
}
