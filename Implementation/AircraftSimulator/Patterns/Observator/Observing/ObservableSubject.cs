using System;
using System.Collections.Generic;
using Common.Containers;

namespace Patterns.Observator.Observing
{
    public abstract class ObservableSubject : IObservable<List<IData>>, IObserverCompute
    {
        protected Dictionary<int, IObserver<List<IData>>> observers;

        public abstract List<IData> Compute(Common.Scripts.SpecialScriptType specialScriptType, List<IData> parameters);

        public virtual IDisposable Subscribe(IObserver<List<IData>> observer)
        {
            observers.Add(observers.Count, observer);
            return null;
        }
    }
}
