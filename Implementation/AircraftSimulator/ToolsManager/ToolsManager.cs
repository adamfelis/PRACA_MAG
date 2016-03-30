using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.CommunicationBridge;
using Common.Containers;

namespace ToolsManager
{
    public sealed class ToolsManager
    {
        private static ToolsManager instance;
        private MathToolCommunicator mathToolCommunicator;
        private ToolsManagement.IToolsManagement toolsManagement;

        public static ToolsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ToolsManager();
                return instance;
            }
        }

        public ToolsManagement.IToolsManagement ToolsManagement
        {
            get
            {
                return toolsManagement;
            }
        }

        private ToolsManager()
        {
            mathToolCommunicator = new RefinedMathToolCommunicator();
            toolsManagement = new ToolsManagement.ToolsManagement();
        }
        public IData Compute(IData parameters)
        {
            IData result = null;
            return result;
            //return methodImplementor.Compute(parameters);
        }
    }
}
