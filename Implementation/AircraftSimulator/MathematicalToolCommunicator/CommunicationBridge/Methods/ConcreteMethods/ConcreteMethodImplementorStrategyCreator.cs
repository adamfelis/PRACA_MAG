using Common.Containers;
using Common.Scripts;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods.ConcreteMethods
{
    class ConcreteMethodImplementorStrategyCreator : Methods.MethodImplementor
    {
        private const SpecialScriptType specialScriptType = SpecialScriptType.StrategyCreator;
        internal override List<IData> Compute(Parameters parameters)
        {
            return MathToolCommunicator.MathToolFacade.RunScript(specialScriptType, parameters);
        }
    }
}

