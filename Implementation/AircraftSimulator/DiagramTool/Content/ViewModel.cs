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


        private PlotModel lateralPlotModel;
        public PlotModel LateralPlotModel
        {
            get { return lateralPlotModel; }
            set { lateralPlotModel = value; OnPropertyChanged("LateralPlotModel"); }
        }

        private PlotModel longitudinalPlotModel;
        public PlotModel LongitudinalPlotModel
        {
            get { return longitudinalPlotModel; }
            set { longitudinalPlotModel = value; OnPropertyChanged("LongitudinalPlotModel"); }
        }

        public ViewModel()
        {
            LongitudinalPlotModel = new PlotModel();
            LateralPlotModel = new PlotModel();
            longitudinalDataPointsForEachStrategy = new List<List<DataPoint>>();
            lateralDataPointsForEachStrategy = new List<List<DataPoint>>();
            random = new Random();
            SetUpModels();
            InitNewDataForTheShooter(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetUpModels()
        {
            LongitudinalPlotModel.LegendTitle = "Legend";
            LongitudinalPlotModel.LegendOrientation = LegendOrientation.Horizontal;
            LongitudinalPlotModel.LegendPlacement = LegendPlacement.Outside;
            LongitudinalPlotModel.LegendPosition = LegendPosition.TopRight;
            LongitudinalPlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            LongitudinalPlotModel.LegendBorder = OxyColors.Black;
            LongitudinalPlotModel.AutoAdjustPlotMargins = true;
            
            var XvalueAxisLongitudinal = new LinearAxis(AxisPosition.Bottom, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X" };
            LongitudinalPlotModel.Axes.Add(XvalueAxisLongitudinal);
            var ZvalueAxisLongitudinal = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Z" };
            LongitudinalPlotModel.Axes.Add(ZvalueAxisLongitudinal);


            LateralPlotModel.LegendTitle = "Legend";
            LateralPlotModel.LegendOrientation = LegendOrientation.Horizontal;
            LateralPlotModel.LegendPlacement = LegendPlacement.Outside;
            LateralPlotModel.LegendPosition = LegendPosition.TopRight;
            LateralPlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            LateralPlotModel.LegendBorder = OxyColors.Black;
            LateralPlotModel.AutoAdjustPlotMargins = true;

            var YvalueAxisLateral = new LinearAxis(AxisPosition.Bottom, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y" };
            LateralPlotModel.Axes.Add(YvalueAxisLateral);
            var XvalueAxisLateral = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X" };
            LateralPlotModel.Axes.Add(XvalueAxisLateral);
        }

        private void InitNewDataForTheShooter(int strategiesAmount)
        {
            for(int i = 0; i < strategiesAmount; i++)
            {
                var lineSerieLongitudinal = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    CanTrackerInterpolatePoints = false,
                    Smooth = false,
                };
                var lineSerieLateral = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    CanTrackerInterpolatePoints = false,
                    Smooth = false,
                };
                this.LongitudinalPlotModel.Series.Add(lineSerieLongitudinal);
                this.LateralPlotModel.Series.Add(lineSerieLateral);
            }
        }

        private List<List<DataPoint>> longitudinalDataPointsForEachStrategy;
        private List<List<DataPoint>> lateralDataPointsForEachStrategy;

        //called every time the shooter changed
        public void ResetState()
        {
            this.LongitudinalPlotModel.Series.Clear();
            this.LateralPlotModel.Series.Clear();
        }

        public void AddLongitudinalPoints(List<DataPoint> newDataPoints)
        {
            for (int i = 0; i < newDataPoints.Count; i++)
            {
                if (i >= longitudinalDataPointsForEachStrategy.Count)
                    longitudinalDataPointsForEachStrategy.Add(new List<DataPoint>());
                longitudinalDataPointsForEachStrategy[i].Add(newDataPoints[i]);
            }
            for (int i = longitudinalDataPointsForEachStrategy.Count; i > newDataPoints.Count; i--)
            {
                longitudinalDataPointsForEachStrategy.RemoveAt(i - 1);
            }
        }

        public void AddLateralPoints(List<DataPoint> newDataPoints)
        {
            for (int i = 0; i < newDataPoints.Count; i++)
            {
                if (i >= lateralDataPointsForEachStrategy.Count)
                    lateralDataPointsForEachStrategy.Add(new List<DataPoint>());
                lateralDataPointsForEachStrategy[i].Add(newDataPoints[i]);
            }
            for (int i = lateralDataPointsForEachStrategy.Count; i > newDataPoints.Count; i--)
            {
                lateralDataPointsForEachStrategy.RemoveAt(i - 1);
            }
        }

        public void UpdateModels()
        {
            for (int i = 0; i < longitudinalDataPointsForEachStrategy.Count; i++)
            {
                if (longitudinalDataPointsForEachStrategy[i].Count > 0)
                {
                    DataPoint newPoint = longitudinalDataPointsForEachStrategy[i][0];
                    longitudinalDataPointsForEachStrategy[i].RemoveAt(0);
                    if (this.LongitudinalPlotModel.Series.Count <= i)
                    {
                        InitNewDataForTheShooter(1);
                        if (i > 0)
                        {
                            IDataPoint lastDataPointFromTheMainStartegy = (this.LongitudinalPlotModel.Series[0] as LineSeries).Points[(this.LongitudinalPlotModel.Series[0] as LineSeries).Points.Count - 1];
                            newPoint.X += lastDataPointFromTheMainStartegy.X;
                            newPoint.Y += lastDataPointFromTheMainStartegy.Y;
                        }
                    }
                    else if ((this.LongitudinalPlotModel.Series[i] as LineSeries).Points.Count > 0)
                    {
                        IDataPoint lastDataPoint = (this.LongitudinalPlotModel.Series[i] as LineSeries).Points[(this.LongitudinalPlotModel.Series[i] as LineSeries).Points.Count - 1];
                        newPoint.X += lastDataPoint.X;
                        newPoint.Y += lastDataPoint.Y;
                    }
                    (this.LongitudinalPlotModel.Series[i] as LineSeries).Points.Add(newPoint);
                    if ((this.LongitudinalPlotModel.Series[i] as LineSeries).Points.Count > 100)
                    {
                        (this.LongitudinalPlotModel.Series[i] as LineSeries).Points.RemoveAt(0);
                    }
                }
            }
            for (int i = 0; i < lateralDataPointsForEachStrategy.Count; i++)
            {
                if (lateralDataPointsForEachStrategy[i].Count > 0)
                {
                    DataPoint newPoint = lateralDataPointsForEachStrategy[i][0];
                    lateralDataPointsForEachStrategy[i].RemoveAt(0);
                    if (this.LateralPlotModel.Series.Count <= i)
                    {
                        InitNewDataForTheShooter(1);
                        if (i > 0)
                        {
                            IDataPoint lastDataPointFromTheMainStartegy = (this.LateralPlotModel.Series[0] as LineSeries).Points[(this.LateralPlotModel.Series[0] as LineSeries).Points.Count - 1];
                            newPoint.X += lastDataPointFromTheMainStartegy.X;
                            newPoint.Y += lastDataPointFromTheMainStartegy.Y;
                        }
                    }
                    else if ((this.LateralPlotModel.Series[i] as LineSeries).Points.Count > 0)
                    {
                        IDataPoint lastDataPoint = (this.LateralPlotModel.Series[i] as LineSeries).Points[(this.LateralPlotModel.Series[i] as LineSeries).Points.Count - 1];
                        newPoint.X += lastDataPoint.X;
                        newPoint.Y += lastDataPoint.Y;
                    }
                    (this.LateralPlotModel.Series[i] as LineSeries).Points.Add(newPoint);
                    if ((this.LateralPlotModel.Series[i] as LineSeries).Points.Count > 100)
                    {
                        (this.LateralPlotModel.Series[i] as LineSeries).Points.RemoveAt(0);
                    }
                }
            }
        }
    }
}
