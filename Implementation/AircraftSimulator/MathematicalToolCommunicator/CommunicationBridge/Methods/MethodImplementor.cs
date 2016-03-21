using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods
{
    public abstract class MethodImplementor
    {
        public virtual Parameters TemplateComputeMethod(Parameters parameters)
        {
            Parameters result = null;

            return result; 
        }

        internal abstract Parameters Compute(Parameters parameters);
    }
}
