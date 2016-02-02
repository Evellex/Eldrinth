using UnityEngine;

namespace Augmenta
{
	public struct Double4
	{
		public static readonly Double4 zero = new Double4(0, 0, 0, 0);
		public static readonly Double4 one = new Double4(1, 1, 1, 1);
		public static readonly Double4 left = new Double4(-1, 0, 0, 0);
		public static readonly Double4 right = new Double4(1, 0, 0, 0);
		public static readonly Double4 up = new Double4(0, 1, 0, 0);
		public static readonly Double4 down = new Double4(0, -1, 0, 0);
		public static readonly Double4 forward = new Double4(0, 0, 1, 0);
		public static readonly Double4 back = new Double4(0, 0, -1, 0);
		public static readonly Double4 ana = new Double4(0, 0, 0, 1);
		public static readonly Double4 kata = new Double4(0, 0, 0, -1);

		public double x, y, z, w;

		public Double4(double x, double y, double z, double w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Double4(Double4 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}

		public double MaxVal
		{
			get { return Mathd.Max(x, y, z, w); }
		}

		public double MinVal
		{
			get { return Mathd.Min(x, y, z, w); }
		}

		public double this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else if (i == 2) return z; else if (i == 3) return w; else { Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else if (i == 3) w = value; else { Debug.LogException(new System.IndexOutOfRangeException()); } }
		}

		public static Double4 Max(params Double4[] values)
		{
			Double4 max = Uniform(double.NegativeInfinity);
			for (int i = 0; i < values.Length; ++i)
				max = Max(values[i], max);
			return max;
		}

		public static Double3 Max(Double3 first, Double3 second)
		{
			return new Double3(Mathd.Max(first.x, second.x), Mathd.Max(first.y, second.y), Mathd.Max(first.z, second.z));
		}

		public static explicit operator Double4(Vector4 v)
		{
			return new Double4(v.x, v.y, v.z, v.w);
		}

		public static explicit operator Color(Double4 v)
		{
			return new Color((float)v.x, (float)v.y, (float)v.z, (float)v.w);
		}

		public static explicit operator Double3(Double4 v)
		{
			return new Double3(v.x, v.y, v.z);
		}

		public static explicit operator Double2(Double4 v)
		{
			return new Double2(v.x, v.y);
		}

		public static Double4 operator +(Double4 v1, Double4 v2)
		{
			return new Double4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
		}

		public static Double4 operator -(Double4 v1, Double4 v2)
		{
			return new Double4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
		}

		public static Double4 operator *(Double4 v1, Double4 v2)
		{
			return new Double4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
		}

		public static Double4 operator *(Double4 v1, double s)
		{
			return new Double4(v1.x * s, v1.y * s, v1.z * s, v1.w * s);
		}

		public static Double4 Uniform(double v)
		{
			return new Double4(v, v, v, v);
		}

		public override string ToString()
		{
			return "X:(" + x + ") Y:(" + y + ") Z:(" + z + ") W:(" + w + ")";
		}
	}
}