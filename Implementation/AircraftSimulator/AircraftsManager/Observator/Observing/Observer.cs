using System;
using Common.Containers;

namespace Common.Patterns.Observator.Observing
{
    public abstract class Observer : IObserver<IData>
    {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IData value);
    }
}
