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
        Polyline l;
        const double margin = 20;
        double xmin = margin, xmax, ymax;
        const double step = 10;
        private GraphRegViewModel vm;
        private List<Ellipse> ellipses;
        private List<Ellipse> last30Seconds;
        private Brush[] brushes = { Brushes.Orange, Brushes.Blue, Brushes.LightGray, Brushes.Red };
        public GraphRegView()
        {
            InitializeComponent();
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
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            canGraph.Children.Clear();
                            drawAxis();
                        });
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
        private void DrawNewGraph()
        {
            canGraph.Children.Clear();
            ellipses.Clear();
            last30Seconds.Clear();

            // Display ellipses at the points.
            const float pointSize = 4;

            // scaling
            double xRange = vm.DisplayFeatureMaxValue - vm.DisplayFeatureMinValue;
            double yRange = vm.CorrolateFeatureMaxValue - vm.CorrolateFeatureMinValue;

            //double yZeroLocation = ymax - (ymax * (Math.Abs(vm.DisplayFeatureMinValue) / yRange)) - margin;
            //Canvas.SetTop(this.yZeroLabel, yZeroLocation);
            //canGraph.Children.Add(this.yZeroLabel);

            //double xZeroLocation = xmax * (Math.Abs(vm.CorrolateFeatureMinValue) / xRange) + margin;
            //Canvas.SetLeft(this.xZeroLabel, xZeroLocation);
            //canGraph.Children.Add(this.xZeroLabel);

            List<int> anomalies = new List<int>();
            string features1 = vm.VM_DisplayFeature + "-" + vm.VM_CorrolatedFeature;
            string features2 = vm.VM_CorrolatedFeature + "-" + vm.VM_DisplayFeature;

            foreach (Tuple<string, int> t in vm.Anomalies)
            {
                if (features1.Equals(t.Item1) || features2.Equals(t.Item1))
                    anomalies.Add(t.Item2);
            }

            List<float> x = vm.getValuesOfFeature(vm.VM_DisplayFeature);
            List<float> y = vm.getValuesOfFeature(vm.VM_CorrolatedFeature);
            List<Point> points = toPoints(x, y);
            for (int i = 0; i < points.Count; i++)
            {
                Ellipse ellipse = new Ellipse();
                Canvas.SetLeft(ellipse, margin + (points[i].X / xRange) * (xmax - margin) - pointSize / 2);
                Canvas.SetTop(ellipse, ymax - (points[i].Y / yRange) * ymax - pointSize / 2);
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

            double max = vm.DisplayFeatureMaxValue;
            double x1 = margin + (max / xRange) * (xmax - margin);
            double y1 = ymax - (regLine.f((float)max) / yRange) * ymax;
            Point p1 = new Point(x1 , y1);

            double min = vm.DisplayFeatureMinValue;
            double x2 = margin + (min / xRange) * (xmax - margin);
            double y2 = ymax - (regLine.f((float)min) / yRange) * ymax;
            Point p2 = new Point(x2, y2);

            float startX = (-1) * regLine.b / regLine.a;
            float startY = regLine.f(startX);
            double x3 = margin + (startX / xRange) * (xmax - margin);
            double y3 = ymax - (startY / yRange) * ymax;
            Point p3 = new Point(x3, y3);

            double y4 = 0;
            double endY = yRange;
            double endX = (endY - regLine.b) / regLine.a;
            double x4 = margin + (endX / xRange) * (xmax - margin);
            Point p4 = new Point(x4, y4);

            PointCollection pointCol = new PointCollection();
            Point pLeft = p2, pRight = p1;
            if (p2.Y > ymax)
                pLeft = p3;
            if (p1.Y < 0)
                pRight = p4;

            pointCol.Add(pLeft);
            pointCol.Add(pRight);

            l = new Polyline();
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
