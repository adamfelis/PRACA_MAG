using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Common.Containers;

namespace Server.Executors
{
    public interface IClientDataPresentedExecutor
    {
        void SetupAndRun(Dispatcher dispatcher, Func<IList<IData>> action);
    }
}
