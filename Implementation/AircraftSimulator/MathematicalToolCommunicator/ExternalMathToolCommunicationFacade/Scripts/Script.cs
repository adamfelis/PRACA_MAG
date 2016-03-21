﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts
{
    abstract class Script
    {
        protected ScriptType scriptType;
        protected const String scriptName = "";

        protected abstract String ScriptName
        {
            get;
        }

        // TODO: return type has to be changed
        internal Scripts.Parameters.Parameters RunScript(MLApp.MLApp mlApp, Parameters.Parameters parameters)
        {
            Scripts.Parameters.Parameters result = null;
            //mlApp.Execute()
            return result;
        } 
    }
}
