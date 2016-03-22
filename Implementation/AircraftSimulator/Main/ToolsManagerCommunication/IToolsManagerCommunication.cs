using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationManager.ToolsManagerCommunication
{
    interface IToolsManagerCommunication
    {
        ToolsManager.ToolsManager ManagerInstance
        {
            get;
        }
    }
}
