using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters
{
    sealed class Parameters
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
    }
}
