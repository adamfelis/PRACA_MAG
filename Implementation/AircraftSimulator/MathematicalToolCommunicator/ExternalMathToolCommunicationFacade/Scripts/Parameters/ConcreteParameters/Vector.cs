using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters.ConcreteParameters
{
    sealed class Vector : Parameter
    {
        public Vector(ParameterType parameterType) : base(parameterType)
        {
            value = new float[4] {0,0,0,0};
        }

        public Vector(ParameterType parameterType, object value, string name) : base(parameterType, value, name)
        {

        }
    }
}
