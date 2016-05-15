using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramTool.Content
{
    class ViewModel : INotifyPropertyChanged
    {
        private Random random;

        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }

        public ViewModel()
        {
            PlotModel = new PlotModel();
            dataPoints = new List<DataPoint>();
            random = new Random();
            SetUpModel();
            FillInitialData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetUpModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;


            var XvalueAxis = new LinearAxis(AxisPosition.Bottom, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X" };
            PlotModel.Axes.Add(XvalueAxis);
            var ZvalueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Z" };
            PlotModel.Axes.Add(ZvalueAxis);
        }

        private void FillInitialData()
        {
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                CanTrackerInterpolatePoints = false,
                Smooth = false,
            };
            this.PlotModel.Series.Add(lineSerie);
        }

        private List<DataPoint> dataPoints;

        public void ResetState()
        {
            (this.PlotModel.Series[0] as LineSeries).Points.Clear();
        }

        public void AddPoint(DataPoint newDataPoint)
        {
            dataPoints.Add(newDataPoint);
        }

        public void UpdateModel()
        {
            if(dataPoints.Count > 0)
            {
                DataPoint newPoint = dataPoints[0];
                dataPoints.RemoveAt(0);
                if((this.PlotModel.Series[0] as LineSeries).Points.Count > 0)
                {
                    IDataPoint lastDataPoint = (this.PlotModel.Series[0] as LineSeries).Points[(this.PlotModel.Series[0] as LineSeries).Points.Count - 1];
                    newPoint.X += lastDataPoint.X;
                    newPoint.Y += lastDataPoint.Y;
                }
                (this.PlotModel.Series[0] as LineSeries).Points.Add(newPoint);
                if((this.PlotModel.Series[0] as LineSeries).Points.Count > 50)
                {
                    (this.PlotModel.Series[0] as LineSeries).Points.RemoveAt(0);
                }
            }
        }
    }
}
