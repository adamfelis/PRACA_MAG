using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ToolsManagerCommunication
{
    public interface IToolsManagerCommunicationImplementor
    {
        IToolsManagerCommunication ToolsManagerCommunication
        {
            get;
        }
    }
}
