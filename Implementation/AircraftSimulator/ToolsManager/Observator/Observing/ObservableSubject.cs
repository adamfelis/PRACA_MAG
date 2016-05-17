using System;
using System.Collections.Generic;
using Common.Containers;

namespace ToolsManager.Observator.Observing
{
    public abstract class ObservableSubject : IObservable<List<IData>>
    {
        protected Dictionary<int, IObserver<List<IData>>> observers;

        public virtual IDisposable Subscribe(IObserver<List<IData>> observer)
        {
            observers.Add(observers.Count, observer);
            return null;
        }
    }
}
