using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Containers;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Scripts;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods.ConcreteMethods
{
    class ConcreteMethodImplementorInverse : Methods.MethodImplementor
    {
        private const SpecialScriptType specialScriptType = SpecialScriptType.Inverse;
        internal override List<IData> Compute(Parameters parameters)
        {
            return MathToolCommunicator.MathToolFacade.RunScript(specialScriptType, parameters);
        }
    }
}
