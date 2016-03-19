using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolAdapter.Observing.ConcreteObserving
{
    public sealed class ConcreteObservableSubject : Observing.ObservableSubject
    {
        public override IDisposable Subscribe(IObserver<Observer> observer)
        {
            throw new NotImplementedException();
        }
    }
}
