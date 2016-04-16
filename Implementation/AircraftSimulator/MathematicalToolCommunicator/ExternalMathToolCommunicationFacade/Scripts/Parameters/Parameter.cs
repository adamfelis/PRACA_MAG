using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters
{
    public abstract class Parameter
    {
        protected object value;
        protected string name;
        internal object Value
        {
            get
            {
                return value;
            }
        }
        internal string Name
        {
            get
            {
                return name;
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

        protected Parameter(ParameterType parameterType, object value, string name)
        {
            this.parameterType = parameterType;
            this.name = name;

            float[][] values = value as float[][];
            System.Array array = new float[values.Length * values[0].Length];
            for(int i = 0; i < values.Length * values[0].Length; i++)
            {
                array.SetValue(values[(int)(i / values[0].Length)][i % values[0].Length], i);
            }
            this.value = array;
        }
    }
}
