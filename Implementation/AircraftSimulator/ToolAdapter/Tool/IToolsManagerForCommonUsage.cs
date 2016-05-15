using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolAdapter.Tool
{
    public interface IToolsManagerForCommonUsage
    {
        List<IData> Compute(List<IData> parameters);
        List<IData> Compute(Common.Scripts.SpecialScriptType specialScriptType, List<IData> parameters);
    }
}
