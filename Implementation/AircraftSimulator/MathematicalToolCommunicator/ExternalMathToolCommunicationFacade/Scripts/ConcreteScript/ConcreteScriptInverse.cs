using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.ConcreteScript
{
    sealed class ConcreteScriptInverse : Script
    {
        private new const String scriptName = "Inverse";
        protected override string ScriptName
        {
            get
            {
                return scriptName;
            }
        }
    }
}
