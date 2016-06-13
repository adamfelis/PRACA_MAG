using System;
using System.Collections.Generic;
using System.Linq;
using Common.Containers;
using Common.Scripts;
using Patterns.Observator.Observing;

namespace ToolsManager.ConcreteObserving
{
    public sealed class ConcreteObservableSubject : ObservableSubject
    {
        private Func<SpecialScriptType, List<IData>, List<IData>> computeMethod;

        public ConcreteObservableSubject(Func<SpecialScriptType, List<IData>, List<IData>> computeMethod)
        {
            this.computeMethod = computeMethod;
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

        public override List<IData> Compute(SpecialScriptType specialScriptType, List<IData> parameters)
        {
            return computeMethod(specialScriptType, parameters);
        }
    }
}
