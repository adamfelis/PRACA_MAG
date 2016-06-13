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

        public override void OnNext(List<IData> values)
        {
            List<DataPoint> newLongitudinalDataPoints = new List<DataPoint>();
            List<DataPoint> newLateralDataPoints = new List<DataPoint>();
            for (int i = 0; i < values.Count - 1; i+=3)
            {
                //if (values[i].MessageStrategy == MessageStrategy.LongitudinalData)
                //{
                //    newLongitudinalDataPoints.Add(new DataPoint(values[i].Array[0][0] * 0.02, values[i].Array[0][1] * 0.02));
                //}
                //if (values[i + 1].MessageStrategy == MessageStrategy.LateralData)
                //{
                //    newLateralDataPoints.Add(new DataPoint(values[i].Array[0][1] * 0.02, values[i + 1].Array[0][0] * 0.02));
                //}
                if (values[i + 2].MessageStrategy == MessageStrategy.PositionData)
                {
                    newLongitudinalDataPoints.Add(new DataPoint(values[i+2].Array[0][0], values[i+2].Array[0][2]));
                    newLateralDataPoints.Add(new DataPoint(values[i+2].Array[0][2], values[i + 2].Array[0][1]));
                }
            }
            viewModel.AddLongitudinalPoints(newLongitudinalDataPoints);
            viewModel.AddLateralPoints(newLateralDataPoints);
        }
    }
}
