using System;
using System.Collections.Generic;
using Common.Containers;

namespace ToolsManager.Observator.Observing
{
    public abstract class ObservableSubject : IObservable<IData>
    {
        protected Dictionary<int, IObserver<IData>> observers;

        public virtual IDisposable Subscribe(IObserver<IData> observer)
        {
            observers.Add(observers.Count, observer);
            return null;
        }
    }
}
