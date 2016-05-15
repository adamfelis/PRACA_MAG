using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patterns.Observator.Observing;

namespace BackupTool.ConcreteObserver
{
    sealed class ConcreteToolManagerObserver : Observer
    {
        public override void OnCompleted()
        {

        }

        public override void OnError(Exception error)
        {

        }

        public override void OnNext(IData value)
        {

        }
    }
}
