using System;
using Common.Containers;
using System.Windows.Controls;
using System.Collections.Generic;

namespace ToolAdapter.Tool
{
    public interface ITool
    {
        Patterns.Observator.Observing.Observer Observer
        {
            get;
        }
        TabItem TabItem
        {
            get;
        }
        ToolType ToolType
        {
            get;
        }
    }
}
