using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;

namespace ToolAdapter.Observing
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
