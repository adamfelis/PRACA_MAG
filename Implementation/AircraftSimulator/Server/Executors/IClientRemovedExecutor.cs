using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patterns.Executors;
using System.Windows.Threading;

namespace Server.Executors
{
    public interface IClientRemovedExecutor
    {
        void SetupAndRun(Dispatcher dispatcher, Delegate action);
    }
}
