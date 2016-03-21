using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolAdapter.Tool;

namespace BackupTool
{
    public sealed class BackupTool : ToolAdapter.Tool.ITool
    {
        public ToolType ToolType
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
