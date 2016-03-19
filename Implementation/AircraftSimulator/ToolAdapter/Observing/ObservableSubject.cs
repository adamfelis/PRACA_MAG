using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolAdapter.Observing
{
    public abstract class ObservableSubject : IObservable<Observer>
    {
        public abstract IDisposable Subscribe(IObserver<Observer> observer);
    }
}
