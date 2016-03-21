using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters.ConcreteParameters
{
    sealed class Matrix : Parameter
    {
        public Matrix(ParameterType parameterType) : base(parameterType)
        {
            value = new float[4, 4] { { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 } };
        }

        public Matrix(ParameterType parameterType, object value) : base(parameterType, value)
        {

        }
    }
}
