using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Containers;

namespace MathematicalToolCommunicator.CommunicationBridge
{
    public abstract class MathToolCommunicator
    {
        private static ExternalMathToolCommunicationFacade.MathToolFacade mathToolFacade; 

        internal static ExternalMathToolCommunicationFacade.MathToolFacade MathToolFacade
        {
            get
            {
                return mathToolFacade;
            }
        }

        protected Methods.MethodImplementor methodImplementor;

        public virtual List<IData> Compute(List<IData> parameters)
        {
            return methodImplementor.Compute(Parameters.PrepareParameters(parameters));
        }

        protected MathToolCommunicator()
        {
            MathToolCommunicator.mathToolFacade = new ExternalMathToolCommunicationFacade.MathToolFacade();
        }

        public void SetMethodImplementor(Common.Scripts.ScriptType scriptType)
        {
            this.methodImplementor = Methods.MethodImplementor.PrepareConcreteMethodImplementor(scriptType);
        }
    }
}
