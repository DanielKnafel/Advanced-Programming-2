using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ex1
{
    public class Line
    {
		public float a { get; set; }
		public float b { get; set; }
		public Line(float a, float b)
		{
			this.a = a;
			this.b = b;
		}
		public Line() : this(0, 0) {	}
		public float f(float x)
		{
			return a * x + b;
		}

		// returns the variance of X and Y
		static float var(List<float> x)
		{
			float av = x.Average();
			float sum = 0;
			foreach (float f in x)
			{
				sum += f*f;
			}
			return sum / x.Count - av * av;
		}

		// returns the covariance of X and Y
		static float cov(List<float> x, List<float> y)
		{
			float sum = 0;
			for (int i = 0; i < x.Count; i++)
			{
				sum += x[i] * y[i];
			}
			sum /= x.Count;

			return sum - x.Average() * y.Average();
		}

		// returns the Pearson correlation coefficient of X and Y
		static float pearson(List<float> x, List<float> y)
		{
			return cov(x, y) / (float)(Math.Sqrt(var(x)) * Math.Sqrt(var(y)));
		}

		// performs a linear regression and returns the line equation
		public static Line linear_reg(List<Point> points)
		{
			List<float> x = new List<float>();
			List<float> y = new List<float>();

			for (int i = 0; i < points.Count; i++)
			{
				x.Add((float)points[i].X);
				y.Add((float)points[i].Y);
			}
			float a = cov(x, y) / var(x);
			float b = y.Average() - a * x.Average();

			return new Line(a, b);
		}
	}
}
