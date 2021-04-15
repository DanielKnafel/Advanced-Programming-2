using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ex1.controls
{
    public partial class GraphControl : UserControl
    {
        const double margin = 10;
        private GraphControlViewModel vm;

        public static readonly DependencyProperty Minimum_xProperty =
            DependencyProperty.Register("Minimum_x",
                typeof(double), typeof(GraphControl), new UIPropertyMetadata(0.0, null));

        public double Minimum_x
        {
            set
            {
                SetValue(Minimum_xProperty, value);
            }
            get
            {
                return (double)GetValue(Minimum_xProperty);
            }
        }

        public static readonly DependencyProperty Minimum_yProperty =
            DependencyProperty.Register("Minimum_y",
                typeof(double), typeof(GraphControl), new UIPropertyMetadata(0.0, null));

        public double Minimum_y
        {
            set
            {
                SetValue(Minimum_yProperty, value);
            }
            get
            {
                return (double)GetValue(Minimum_yProperty);
            }
        }

        public static readonly DependencyProperty Maximum_xProperty =
            DependencyProperty.Register("Maximum_x",
                typeof(double), typeof(GraphControl), new UIPropertyMetadata(300.0, null));

        public double Maximum_x
        {
            set
            {
                SetValue(Maximum_xProperty, value);
            }
            get
            {
                return (double)GetValue(Maximum_xProperty);
            }
        }

        public static readonly DependencyProperty Maximum_yProperty =
            DependencyProperty.Register("Maximum_y",
                typeof(double), typeof(GraphControl), new UIPropertyMetadata(200.0, null));

        public double Maximum_y
        {
            set
            {
                SetValue(Maximum_yProperty, value);
            }
            get
            {
                return (double)GetValue(Maximum_yProperty);
            }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points",
                typeof(ObservableCollection<Point>), typeof(GraphControl), new UIPropertyMetadata(new ObservableCollection<Point>(), new PropertyChangedCallback(helper)));

        public ObservableCollection<Point> Points
        {
            set
            {
                SetValue(PointsProperty, value);
            }
            get
            {
                return (ObservableCollection<Point>)GetValue(PointsProperty);
            }
        }

        static void helper(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GraphControl UserControl1Control = d as GraphControl;
            UserControl1Control.helper(e);
        }
        void helper(DependencyPropertyChangedEventArgs e)
        {
            addGraph(new PointCollection(scaleall((ObservableCollection<Point>)e.NewValue)));
        }

        private PointCollection timeLinePos;
        private void addTimeLine()
        {
            double bottomX = margin;
            double bottomY = this.canGraph.Height;
            double topX = bottomX;
            double topY = 0;

            Point bottom = new Point(bottomX, bottomY);
            Point top = new Point(topX, topY);
            this.timeLinePos = new PointCollection();
            this.timeLinePos.Add(top);
            this.timeLinePos.Add(bottom);
            this.timeLine = new Polyline();
            this.timeLine.StrokeThickness = 1;
            this.timeLine.Stroke = Brushes.Red;
            timeLine.Points = timeLinePos;
        }
        public GraphControl()
        {
            InitializeComponent();
            this.vm = new GraphControlViewModel();
            addTimeLine();

            this.vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("VM_LineNumber"))
                    {
                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                if (this.Points.Count != 0)
                                {
                                    moveTimeLine();
                                }
                            });
                        }
                    }
                };
        }

        private Polyline timeLine;
        private void moveTimeLine()
        {
            Point p1 = this.timeLinePos[0];
            p1.X = margin + ((double)this.vm.LineNumber / (double)this.Points.Count) * (this.canGraph.Width - margin);
            this.timeLinePos[0] = p1;

            Point p2 = this.timeLinePos[1];
            p2.X = p1.X;
            this.timeLinePos[1] = p2;
        }

        public void setMainViewModel(MainViewModel vm)
        {
            this.vm.setMainViewModel(vm);
        }
        void addGraph(PointCollection p)
        {
            canGraph.Children.Clear();
            drawGraphAxes();
            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Green;
            polyline.Points = p;
            canGraph.Children.Add(polyline);
            canGraph.Children.Add(this.timeLine);
        }
        private void drawGraphAxes()
        {
            canGraph.Children.Clear();
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymin = margin;
            double ymax = canGraph.Height - margin;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, ymax), new Point(canGraph.Width, ymax)));

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height - ymin)));

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);
        }
        private Point scale(Point tp)
        {
            Point p = new Point(tp.X, tp.Y);
            double graph_min_x = margin, graph_max_x = canGraph.Width,
                graph_min_y = margin, graph_max_y = canGraph.Height - margin;
            double x_width = graph_max_x - graph_min_x;
            double y_width = graph_max_y - graph_min_y;

            p.X -= Minimum_x;
            p.Y -= Minimum_y;
            if (p.X != 0)
                p.X *= x_width / (Maximum_x - Minimum_x);
            if (p.Y != 0)
                p.Y *= y_width / (Maximum_y - Minimum_y);
            p.X += graph_min_x;
            p.Y = graph_max_y - p.Y;
            return p;
        }
        private ObservableCollection<Point> scaleall(ObservableCollection<Point> p)
        {
            ObservableCollection<Point> temp = new ObservableCollection<Point>();
            foreach (Point tempPoint in p)
            {
                temp.Add(scale(tempPoint));
            }
            return temp;
        }
    }
}
