using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Main.Common
{
    class Dispatchable : IDispatchable
    {
        private Dispatcher dispatcher;
        public Dispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }

        public Dispatchable(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
    }
}
