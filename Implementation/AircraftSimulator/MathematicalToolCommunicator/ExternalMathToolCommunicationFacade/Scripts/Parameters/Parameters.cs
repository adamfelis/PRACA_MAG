using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters
{
    public sealed class Parameters
    {
        private List<Parameter> parameters;

        internal List<Parameter> ParametersList
        {
            get
            {
                return parameters;
            }
        }

        public Parameters(List<Parameter> parameters)
        {
            this.parameters = parameters;
        }
        

        public static Parameters PrepareParameters(IData data)
        {
            List<Parameter> parameters = new List<Parameter>();

            switch(data.InputType)
            {
                case DataType.Float:
                    //TODO
                    break;
                case DataType.Matrix:
                    parameters.Add(new ConcreteParameters.Matrix(ParameterType.Matrix, data.Array));
                    break;
                case DataType.Vector:
                    parameters.Add(new ConcreteParameters.Vector(ParameterType.Vector, data.Array));
                    break;
                case DataType.NotSet:

                    break;
                default:
                    break;
            }

            return new Parameters(parameters); ;
        }
    }
}
