using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;

namespace MathematicalToolCommunicator.CommunicationBridge
{
    sealed class RefinedMathToolCommunicator : MathToolCommunicator
    {
        protected override Parameters Compute(Parameters parameters)
        {
            Parameters result = null;
            result = base.Compute(parameters);

            return result;
        }
    }
}
