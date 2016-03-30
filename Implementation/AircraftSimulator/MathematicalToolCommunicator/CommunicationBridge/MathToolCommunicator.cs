﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;

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

        public virtual Parameters Compute(Parameters parameters)
        {
            return methodImplementor.Compute(parameters);
        }

        protected MathToolCommunicator()
        {
            MathToolCommunicator.mathToolFacade = new ExternalMathToolCommunicationFacade.MathToolFacade();
        }
    }
}
