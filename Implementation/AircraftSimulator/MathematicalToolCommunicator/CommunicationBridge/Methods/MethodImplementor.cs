using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Containers;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods
{
    public abstract class MethodImplementor
    {
        public virtual Parameters TemplateComputeMethod(Parameters parameters)
        {
            Parameters result = null;

            return result; 
        }

        internal abstract IData Compute(Parameters parameters);

        public static MethodImplementor PrepareConcreteMethodImplementor(Common.Scripts.ScriptType scriptType)
        {
            MethodImplementor result = null;
            switch(scriptType)
            {
                case Common.Scripts.ScriptType.LaplaceTransform:
                    result = new ConcreteMethods.ConcreteMethodImplementorLaplaceTransform();
                    break;
                case Common.Scripts.ScriptType.RungeKutta:
                    result = new ConcreteMethods.ConcreteMethodImplementorRungeKutta();
                    break;
                default:
                    throw new ExternalMathToolCommunicationFacade.Scripts.InvalidScriptTypeException();
            }
            return result;
        }
    }
}
