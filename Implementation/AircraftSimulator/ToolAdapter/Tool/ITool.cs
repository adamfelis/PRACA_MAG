using System;
using Common.Containers;
using System.Windows.Controls;

namespace ToolAdapter.Tool
{
    public interface ITool
    {
        IObserver<IData> Observer
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
