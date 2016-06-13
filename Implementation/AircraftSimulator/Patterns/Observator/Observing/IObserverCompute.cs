using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Observator.Observing
{
    public interface IObserverCompute
    {
        List<IData> Compute(Common.Scripts.SpecialScriptType specialScriptType, List<IData> parameters);
    }
}
