using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters
{
    abstract class Parameter
    {
        protected object value;
        internal object Value
        {
            get
            {
                return value;
            }
        }

        protected ParameterType parameterType;

        internal ParameterType ParameterType
        {
            get
            {
                return parameterType;
            }
        }

        protected Parameter(ParameterType parameterType)
        {
            this.parameterType = parameterType;
        }

        protected Parameter(ParameterType parameterType, object value)
        {
            this.parameterType = parameterType;
            this.value = value;
        }
    }
}
