using UnityEngine;

namespace Augmenta
{
	public struct Double3
	{
		public static readonly Double3 zero = new Double3(0, 0, 0);
		public static readonly Double3 one = new Double3(1, 1, 1);
		public static readonly Double3 left = new Double3(-1, 0, 0);
		public static readonly Double3 right = new Double3(1, 0, 0);
		public static readonly Double3 up = new Double3(0, 1, 0);
		public static readonly Double3 down = new Double3(0, -1, 0);
		public static readonly Double3 forward = new Double3(0, 0, 1);
		public static readonly Double3 back = new Double3(0, 0, -1);
		public double x, y, z;

		public Double3(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Double3(Double3 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
		}

		public double MaxVal
		{
			get { return Mathd.Max(x, y, z); }
		}

		public double MinVal
		{
			get { return Mathd.Min(x, y, z); }
		}

		public double this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else if (i == 2) return z; else { Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else { Debug.LogException(new System.IndexOutOfRangeException()); } }
		}

		public static Double3 Max(params Double3[] values)
		{
			Double3 max = Uniform(double.NegativeInfinity);
			for (int i = 0; i < values.Length; ++i)
				max = Max(values[i], max);
			return max;
		}

		public static Double3 Max(Double3 first, Double3 second)
		{
			return new Double3(Mathd.Max(first.x, second.x), Mathd.Max(first.y, second.y), Mathd.Max(first.z, second.z));
		}

		public static explicit operator Double3(Vector3 v)
		{
			return new Double3(v.x, v.y, v.z);
		}

		public static explicit operator Double2(Double3 v)
		{
			return new Double2(v.x, v.y);
		}

		public static explicit operator Color(Double3 v)
		{
			return new Color((float)v.x, (float)v.y, (float)v.z);
		}

		public static Double3 operator +(Double3 v1, Double3 v2)
		{
			return new Double3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
		}

		public static Double3 operator -(Double3 v1, Double3 v2)
		{
			return new Double3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
		}

		public static Double3 operator *(Double3 v1, Double3 v2)
		{
			return new Double3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		public static Double3 operator *(Double3 v1, double s)
		{
			return new Double3(v1.x * s, v1.y * s, v1.z * s);
		}

		public static Double3 Uniform(double v)
		{
			return new Double3(v, v, v);
		}

		public override string ToString()
		{
			return "X:(" + x + ") Y:(" + y + ") Z:(" + z + ")";
		}
	}
}