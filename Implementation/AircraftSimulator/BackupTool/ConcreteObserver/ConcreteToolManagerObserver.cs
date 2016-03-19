using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolAdapter.Observing.ConcreteObserving;

namespace BackupTool.ConcreteObserver
{
    sealed class ConcreteToolManagerObserver : ToolAdapter.Observing.Observer
    {
        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(ConcreteObservableSubject value)
        {
            throw new NotImplementedException();
        }
    }
}
