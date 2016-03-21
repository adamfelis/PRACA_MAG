using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolAdapter.Observing
{
    public abstract class Observer : IObserver<IData>
    {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IData value);
    }
}
