using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex1.controls.GraphReg
{
    /// <summary>
    /// Interaction logic for GraphReg.xaml
    /// </summary>
    public partial class GraphRegView : UserControl
    {
        private const double margin = 20;
        private double xmin = margin, xmax, ymax, ymin = 0;
        private const double step = 10;
        private GraphRegViewModel vm;
        private List<Ellipse> ellipses;
        private List<Ellipse> last30Seconds;
        private Brush[] brushes = { Brushes.Orange, Brushes.Blue, Brushes.LightGray, Brushes.Red };
        private const float pointSize = 4;
        private GeometryGroup geometryGroup;
        private List<int> anomalies;
        private double xRange, yRange;
        public GraphRegView()
        {
            InitializeComponent();
            this.geometryGroup = new GeometryGroup();
            this.anomalies = new List<int>();
            xmax = canGraph.Width;
            ymax = canGraph.Height - margin;
            vm = new GraphRegViewModel();
            this.last30Seconds = new List<Ellipse>();
            this.ellipses = new List<Ellipse>();
            this.DataContext = vm;
            this.vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {                       
                    if (vm.VM_CorrolatedFeature != null)
                    {
                        if (e.PropertyName.Equals("VM_DisplayFeature"))
                        {
                            this.xRange = vm.DisplayFeatureMaxValue - vm.DisplayFeatureMinValue;
                            this.yRange = vm.CorrolateFeatureMaxValue - vm.CorrolateFeatureMinValue;
                            setAnomalies();
                            drawNewGraphInThread();
                        }

                        else if (e.PropertyName.Equals("VM_LineNumber"))
                        {
                            if (Application.Current != null)
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    drawLast30Seconds();
                                });
                            }
                        }
                    }
                    else
                    {
                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                canGraph.Children.Clear();
                                drawAxis();
                            });
                        }
                    }
                };
            drawAxis();
        }

        private void drawNewGraphInThread()
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    canGraph.Children.Clear();
                    ellipses.Clear();
                    last30Seconds.Clear();
                    drawAxis();
                    canGraph.Children.Add(this.xAxisLabel);
                    canGraph.Children.Add(this.yAxisLabel);
                    DrawNewGraph();
                });
            }).Start();
        }
        private void drawAxis()
        {
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(xmax, ymax)));
            for (double x = xmin + step; x <= xmax - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 4),
                    new Point(x, ymax + margin / 4)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, ymax + margin), new Point(xmin, 0)));
            for (double y = step; y <= ymax - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 4, y),
                    new Point(xmin + margin / 4, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

        }
        public void setMainViewModel(MainViewModel vm)
        {
            this.vm.setMainViewModel(vm);
        }
        
        private void setAnomalies()
        {
            string features1 = vm.VM_DisplayFeature + "-" + vm.VM_CorrolatedFeature;
            string features2 = vm.VM_CorrolatedFeature + "-" + vm.VM_DisplayFeature;

            foreach (Tuple<string, int> t in vm.Anomalies)
            {
                if (features1.Equals(t.Item1) || features2.Equals(t.Item1))
                    this.anomalies.Add(t.Item2);
            }
        }
        
        private Point scale(Point p)
        {
            double newX = p.X - vm.DisplayFeatureMinValue;
            double newY = p.Y - vm.CorrolateFeatureMinValue;

            if (xRange != 0)
                newX *= (xmax - xmin - pointSize / 2) / xRange;
            if (yRange != 0)
                newY *= (ymax - ymin - pointSize / 2) / yRange;

            newX += margin;
            newY = ymax - newY - pointSize / 2;
            return new Point(newX, newY);
        }
        private void DrawNewGraph()
        {          
            List<float> x = vm.getValuesOfFeature(vm.VM_DisplayFeature);
            List<float> y = vm.getValuesOfFeature(vm.VM_CorrolatedFeature);
            List<Point> points = toPoints(x, y);

            //double yZeroLocation = ymax - (ymax * (Math.Abs(vm.DisplayFeatureMinValue) / yRange)) - margin;
            //Canvas.SetTop(this.yZeroLabel, yZeroLocation);
            //canGraph.Children.Add(this.yZeroLabel);

            //double xZeroLocation = xmax * (Math.Abs(vm.CorrolateFeatureMinValue) / xRange) + margin;
            //Canvas.SetLeft(this.xZeroLabel, xZeroLocation);
            //canGraph.Children.Add(this.xZeroLabel);

            for (int i = 0; i < points.Count; i++)
            {
                Ellipse ellipse = new Ellipse();
                Point newPoint = scale(points[i]);
                Canvas.SetLeft(ellipse, newPoint.X);
                Canvas.SetTop(ellipse, newPoint.Y);
                ellipse.StrokeThickness = 1;
                ellipse.Width = pointSize;
                ellipse.Height = pointSize;
                ellipse.Fill = brushes[2];
                ellipse.Stroke = brushes[2];
                if (anomalies.Contains(i+1))
                {
                    ellipse.Fill = brushes[3];
                    ellipse.Stroke = brushes[3];
                }
                ellipses.Add(ellipse);
                canGraph.Children.Add(ellipse);
            }

            Line regLine = Line.linear_reg(points);

            Point start = new Point(x.Min(), regLine.f(x.Min()));
            Point end = new Point(x.Max(), regLine.f(x.Max()));

            PointCollection pointCol = new PointCollection();
            pointCol.Add(scale(start));
            pointCol.Add(scale(end));

            Polyline l = new Polyline();
            l.Points = pointCol;
            l.StrokeThickness = 1;
            l.Stroke = brushes[1];
            canGraph.Children.Add(l);
        }
        private List<Point> toPoints(List<float> x, List<float> y)
        {
            List<Point> list = new List<Point>();
            for (int i = 0; i < vm.Size; i++)
            {
                list.Add(new Point(x[i], y[i]));
            }
            return list;
        }
        private void drawLast30Seconds()
        {
            if (last30Seconds.Count > 30 * vm.Frequency)
            {
                // change old point's color
                last30Seconds[0].Fill = brushes[2];
                last30Seconds[0].Stroke = brushes[2];
                last30Seconds.RemoveAt(0);
            }
            // add new point with color
            ellipses[vm.LineNumber].Fill = brushes[0];
            ellipses[vm.LineNumber].Stroke = brushes[0];
            last30Seconds.Add(ellipses[vm.LineNumber]);
        }
    }
}
