using Common.Containers;
using Common.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade
{
    sealed class MathToolFacade
    {
        private Dictionary<ScriptType, Scripts.Script> availableScripts;
        private MLApp.MLApp mlApp;

        internal MathToolFacade()
        {
            InitializeScripts();
            this.mlApp = new MLApp.MLApp();
        }
        
        internal IData RunScript(ScriptType scriptType, Scripts.Parameters.Parameters parameters)
        {
            if (!availableScripts.ContainsKey(scriptType))
                throw new Scripts.InvalidScriptTypeException();
            return availableScripts[scriptType].RunScript(mlApp, parameters);
        }

        private void InitializeScripts()
        {
            this.availableScripts = new Dictionary<ScriptType, Scripts.Script>();

            var scriptTypeValues = Enum.GetValues(typeof(ScriptType)).Cast<ScriptType>();
            foreach(var scriptTypeValue in scriptTypeValues)
            {
                switch(scriptTypeValue)
                {
                    case ScriptType.RungeKutta:
                        availableScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptRungeKutta());
                        break;
                    case ScriptType.LaplaceTransform:
                        availableScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptLaplaceTransform());
                        break;
                    default:
                        throw new Scripts.InvalidScriptTypeException();
                }
            }
        }
    }
}
