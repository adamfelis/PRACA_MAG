using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationManager.ToolsManagerCommunication
{
    class ToolsManagerCommunication : IToolsManagerCommunication
    {
        public ToolsManager.ToolsManager ManagerInstance
        {
            get
            {
                return ToolsManager.ToolsManager.Instance;
            }
        }
    }
}
