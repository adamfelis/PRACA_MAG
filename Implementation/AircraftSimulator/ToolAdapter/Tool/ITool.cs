using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolAdapter.Tool
{
    public interface ITool
    {
        ToolType ToolType
        {
            get;
        }
    }
}
