using System;
using Common.Containers;
using System.Collections.Generic;

namespace Patterns.Observator.Observing
{
    public abstract class Observer : IObserver<List<IData>>
    {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(List<IData> value);
    }
}
