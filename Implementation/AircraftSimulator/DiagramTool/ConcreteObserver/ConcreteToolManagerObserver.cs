using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patterns.Observator.Observing;
using DiagramTool.Content;
using OxyPlot;

namespace DiagramTool.ConcreteObserver
{
    sealed class ConcreteToolManagerObserver : Observer
    {
        private ViewModel viewModel;
        public ConcreteToolManagerObserver(ViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void OnCompleted()
        {
            viewModel.ResetState();
        }

        public override void OnError(Exception error)
        {

        }

        public override void OnNext(IData value)
        {
            DataPoint newDataPoint = new DataPoint(value.Array[0][0] * 0.02, value.Array[0][1] * 0.02);
            viewModel.AddPoint(newDataPoint);
        }
    }
}
