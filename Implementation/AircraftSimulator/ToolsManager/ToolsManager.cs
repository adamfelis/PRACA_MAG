using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.CommunicationBridge;

namespace ToolsManager
{
    public class ToolsManager
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
            //mathToolCommunicator = new RefinedMathToolCommunicator();
            toolsManagement = new ToolsManagement.ToolsManagement();
        }
    }
}
