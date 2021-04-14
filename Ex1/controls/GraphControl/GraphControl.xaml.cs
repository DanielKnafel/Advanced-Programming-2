using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex1.controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        const double margin = 10;

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

        static void redrawHelper(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GraphControl myGraph = d as GraphControl;
            myGraph.addGraph(new PointCollection(myGraph.scaleall(myGraph.Points)));
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

        public GraphControl()
        {
            InitializeComponent();
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
        }

        private void drawGraphAxes()
        {
            canGraph.Children.Clear();
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymin = margin;
            double ymax = canGraph.Height - margin;
            //const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, ymax), new Point(canGraph.Width, ymax)));
            //for (double x = xmin + step;
            //    x <= canGraph.Width - step; x += step)
            //{
            //    xaxis_geom.Children.Add(new LineGeometry(
            //        new Point(x, ymax - margin / 2),
            //        new Point(x, ymax + margin / 2)));
            //}

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height - ymin)));
            //for (double y = step; y <= canGraph.Height - step; y += step)
            //{
            //    yaxis_geom.Children.Add(new LineGeometry(
            //        new Point(xmin - margin / 2, y),
            //        new Point(xmin + margin / 2, y)));
            //}

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
