using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;

namespace MathematicalToolCommunicator.CommunicationBridge
{
    public sealed class RefinedMathToolCommunicator : MathToolCommunicator
    {
        public override Parameters Compute(Parameters parameters)
        {
            Parameters result = null;
            result = base.Compute(parameters);

            return result;
        }
    }
}
