using System;
using System.Windows.Threading;

namespace Server.Executors
{
    public interface IClientAddedExecutor
    {
        void SetupAndRun(Dispatcher dispatcher, Delegate action);
    }
}
