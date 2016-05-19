using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Patterns.Executors;
using System.Windows.Threading;
using Common.Containers;
using Common.EventArgs;

namespace Server.Executors
{
    public class ClientDataPresentedExecutor : Executor, IClientDataPresentedExecutor
    {
        private Dispatcher dispatcher;
        private Func<IList<IData>> action;
        private DataEventArgs dataEventArgs;
        private IServerInputPriveleges serverInputPriveleges;
        private IList<IData> solutions;
        private bool doneJob = false;

        public ClientDataPresentedExecutor(IServerInputPriveleges serverInputPriveleges,
            ref DataEventArgs dataEventArgs)
        {
            this.serverInputPriveleges = serverInputPriveleges;
            this.dataEventArgs = dataEventArgs;
        }

        public void SetupAndRun(Dispatcher dispatcher, Func<IList<IData>> action )
        {
            this.dispatcher = dispatcher;
            this.action = action;
            Execute();
        }

        protected override void InExecute()
        {
            doneJob = false;
            //Thread t = new Thread(new ThreadStart(() =>
            //{
            //    Thread.Sleep(3000);
            //    if (!doneJob)
            //    {
            //        bool a = false;
            //        // Request that oThread be stopped
            //    }
            //}));
            //t.Start();
            //if necessary may be turned to async and awaited / depends on ui blocking
            solutions = dispatcher.Invoke(action);
            doneJob = true;
           // t.Join();
            base.InExecute();
        }

        protected override void PostExecute()
        {
            serverInputPriveleges.RespondToClient(new DataEventArgs()
            {
                Id = dataEventArgs.Id,
                DataList = new DataList()
                {
                    DataArray = solutions.Select(x => x as Data).ToArray()
                }
            });
        }
    }
}
