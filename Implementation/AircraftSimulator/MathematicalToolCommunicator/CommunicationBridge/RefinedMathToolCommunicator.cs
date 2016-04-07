using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Containers;

namespace MathematicalToolCommunicator.CommunicationBridge
{
    public sealed class RefinedMathToolCommunicator : MathToolCommunicator
    {
        public override List<IData> Compute(List<IData> parameters)
        {
            List<IData> result = null;
            result = base.Compute(parameters);

            return result;
        }
    }
}
