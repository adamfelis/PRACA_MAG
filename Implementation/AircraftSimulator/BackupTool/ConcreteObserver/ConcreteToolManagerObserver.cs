﻿using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Patterns.Observator.Observing;

namespace BackupTool.ConcreteObserver
{
    sealed class ConcreteToolManagerObserver : Observer
    {
        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(IData value)
        {
            throw new NotImplementedException();
        }
    }
}
