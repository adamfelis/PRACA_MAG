using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Patterns.Observator.Tool;

namespace DiagramTool
{
    public sealed class DiagramTool : ITool
    {
        private ToolType toolType;
        public ToolType ToolType
        {
            get
            {
                return toolType;
            }
        }

        public DiagramTool()
        {

        }

        public DiagramTool(ToolType toolType)
        {
            this.toolType = toolType;
        }
    }
}
