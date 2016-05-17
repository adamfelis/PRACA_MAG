using System;
using System.Collections.Generic;
using System.Linq;
using Common.Containers;

namespace Patterns.Observator.Observing.ConcreteObserving
{
    public sealed class ConcreteObservableSubject : ObservableSubject
    {
        public ConcreteObservableSubject()
        {
            this.observers = new Dictionary<int, IObserver<List<IData>>>();
        }

        public override IDisposable Subscribe(IObserver<List<IData>> observer)
        {
            return base.Subscribe(observer);
        }

        public void NotifySubscribersOnNext(List<IData> data)
        {
            foreach (IObserver<List<IData>> observer in observers.Values)
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

        public List<IObserver<List<IData>>> GetSubscribers()
        {
            return this.observers.Values.ToList<IObserver<List<IData>>>();
        }
    }
}
