using UnityEngine;

namespace Augmenta
{
	public struct Double2
	{
		public static readonly Double2 zero = new Double2(0, 0);
		public static readonly Double2 one = new Double2(1, 1);
		public static readonly Double2 left = new Double2(-1, 0);
		public static readonly Double2 right = new Double2(1, 0);
		public static readonly Double2 up = new Double2(0, 1);
		public static readonly Double2 down = new Double2(0, -1);

		public double x, y;

		public Double2(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public Double2(Double2 v)
		{
			x = v.x;
			y = v.y;
		}

		public double MaxVal
		{
			get { return Mathd.Max(x, y); }
		}

		public double MinVal
		{
			get { return Mathd.Min(x, y); }
		}

		public double this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else { Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else { Debug.LogException(new System.IndexOutOfRangeException()); } }
		}

		public static Double2 Max(params Double2[] values)
		{
			Double2 max = Uniform(double.NegativeInfinity);
			for (int i = 0; i < values.Length; ++i)
				max = Max(values[i], max);
			return max;
		}

		public static Double2 Max(Double2 first, Double2 second)
		{
			return new Double2(Mathd.Max(first.x, second.x), Mathd.Max(first.y, second.y));
		}

		public static explicit operator Double2(Vector2 v)
		{
			return new Double2(v.x, v.y);
		}

		public static Double2 operator +(Double2 v1, Double2 v2)
		{
			return new Double2(v1.x + v2.x, v1.y + v2.y);
		}

		public static Double2 operator -(Double2 v1, Double2 v2)
		{
			return new Double2(v1.x - v2.x, v1.y - v2.y);
		}

		public static Double2 operator *(Double2 v1, Double2 v2)
		{
			return new Double2(v1.x * v2.x, v1.y * v2.y);
		}

		public static Double2 operator *(Double2 v1, double s)
		{
			return new Double2(v1.x * s, v1.y * s);
		}

		public static Double2 Uniform(double v)
		{
			return new Double2(v, v);
		}

		public override string ToString()
		{
			return "X:(" + x + ") X:(" + y + ")";
		}
	}
}