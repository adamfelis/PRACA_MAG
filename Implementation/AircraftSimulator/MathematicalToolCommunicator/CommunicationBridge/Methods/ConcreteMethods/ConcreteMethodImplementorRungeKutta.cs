using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Scripts;
using Common.Containers;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods.ConcreteMethods
{
    sealed class ConcreteMethodImplementorRungeKutta : Methods.MethodImplementor
    {
        private const ScriptType scriptType = ScriptType.RungeKutta;
        internal override IData Compute(Parameters parameters)
        {
            return MathToolCommunicator.MathToolFacade.RunScript(scriptType, parameters);
        }
    }
}
