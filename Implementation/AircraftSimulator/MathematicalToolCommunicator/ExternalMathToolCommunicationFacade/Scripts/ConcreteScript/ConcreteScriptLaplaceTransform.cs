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
    }
}
