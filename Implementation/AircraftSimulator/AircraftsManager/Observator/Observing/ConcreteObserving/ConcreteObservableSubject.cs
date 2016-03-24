using System;
using System.Collections.Generic;
using System.Linq;
using Common.Containers;

namespace Common.Patterns.Observator.Observing.ConcreteObserving
{
    public sealed class ConcreteObservableSubject : ObservableSubject
    {
        public ConcreteObservableSubject()
        {
            this.observers = new Dictionary<int, IObserver<IData>>();
        }

        public override IDisposable Subscribe(IObserver<IData> observer)
        {
            return base.Subscribe(observer);
        }

        public void NotifySubscribersOnNext(IData data)
        {
            foreach (IObserver<IData> observer in observers.Values)
            {
                observer.OnNext(data);
            }
        }
        public void NotifySubscribersOnCompleted()
        {
            foreach (IObserver<Observer> observer in observers.Values)
            {
                (observer as Observer).OnCompleted();
            }
        }
        public void NotifySubscribersOnError()
        {
            foreach (IObserver<Observer> observer in observers.Values)
            {
                (observer as Observer).OnError(new Exception());
            }
        }

        public List<IObserver<IData>> GetSubscribers()
        {
            return this.observers.Values.ToList<IObserver<IData>>();
        }
    }
}
