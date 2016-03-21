using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolAdapter.Observing.ConcreteObserving;

namespace ToolsManager.ToolsManagement
{
    public interface IToolsManagement
    {
        ConcreteObservableSubject ConcreteObservableSubject
        {
            get;
        }
    }
}
