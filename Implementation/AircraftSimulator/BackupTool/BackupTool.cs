using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Patterns.Observator.Tool;

namespace BackupTool
{
    public sealed class BackupTool : ITool
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
