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
        private List<Ellipse> points;
        private List<Ellipse> last30Seconds;
        private Brush[] brushes = { Brushes.Green, Brushes.Blue, Brushes.Gray };
        public GraphRegView()
        {
            InitializeComponent();
            xmax = canGraph.Width;
            ymax = canGraph.Height - margin;
            vm = new GraphRegViewModel();
            this.last30Seconds = new List<Ellipse>();
            this.points = new List<Ellipse>();
            this.DataContext = vm;
            this.vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {                       
                    if (vm.VM_CorrolatedFeature != null)
                    {
                        if (e.PropertyName.Equals("VM_DisplayFeature"))
                        {
                            runDrawNewGraphInThread();
                        }

                        else if (e.PropertyName.Equals("VM_LineNumber"))
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                drawLast30Seconds();
                            });
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

        private void runDrawNewGraphInThread()
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
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
            points.Clear();
            last30Seconds.Clear();
            drawAxis();
            canGraph.Children.Add(this.xAxisLabel);
            canGraph.Children.Add(this.yAxisLabel);

            // Display ellipses at the points.
            const float pointSize = 4;
            List<Point> p = vm.getPoints();

            // scaling
            double xRange = vm.DisplayFeatureMaxValue - vm.DisplayFeatureMinValue;
            double yRange = vm.CorrolateFeatureMaxValue - vm.CorrolateFeatureMinValue;

            for (int i = 0; i < vm.Size; i++)
            {
                Ellipse ellipse = new Ellipse();
                Canvas.SetLeft(ellipse, margin + (p[i].X / xRange) * (xmax - margin) - pointSize / 2);
                Canvas.SetTop(ellipse, ymax - (p[i].Y / yRange) * ymax - pointSize / 2);
                ellipse.StrokeThickness = 1;
                ellipse.Width = pointSize;
                ellipse.Height = pointSize;
                ellipse.Fill = brushes[2];
                ellipse.Stroke = brushes[2];

                points.Add(ellipse);
                canGraph.Children.Add(ellipse);
            }

            l = new Polyline();
            Line regLine = vm.RegLine;
            double max = vm.DisplayFeatureMaxValue;
            double x1 = margin + (max / xRange) * (xmax - margin);
            double y1 = ymax - (regLine.f((float)max) / yRange) * ymax;
            Point p1 = new Point(x1 , y1);

            double min = vm.DisplayFeatureMinValue;
            double x2 = margin + (min / xRange) * (xmax - margin);
            double y2 = ymax - (regLine.f((float)min) / yRange) * ymax;
            Point p2 = new Point(x2, y2);

            PointCollection pointCol = new PointCollection();
            pointCol.Add(p1);
            pointCol.Add(p2);

            l.Points = pointCol;
            l.StrokeThickness = 1;
            l.Stroke = brushes[1];
            canGraph.Children.Add(l);
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
            points[vm.LineNumber].Fill = brushes[0];
            points[vm.LineNumber].Stroke = brushes[0];
            last30Seconds.Add(points[vm.LineNumber]);
        }
    }
}
