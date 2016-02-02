using UnityEngine;

namespace Augmenta
{
	public static class Mathd
	{
		public const double PI = 3.141592653589793238462643383279502884197169399375105820974944;
		public const double PI2 = 6.283185307179586476925286766559005768394338798750211641949889;

		public const double mτToDeg = 0.36f;
		public const double DegTomτ = 2.777777777777777777777777777777777777777777777777777777777778;

		public const double mτToRad = 0.006283185307179586476925286766559005768394338798750211641949;
		public const double RadTomτ = 159.1549430918953357688837633725143620344596457404564487476673;

		public const double τToDeg = 360.0f;
		public const double DegToτ = 0.0027777777777777777777777777777777777777777777777777777777778;

		public const double τToRad = 6.283185307179586476925286766559005768394338798750211641949889;
		public const double RadToτ = 0.1591549430918953357688837633725143620344596457404564487476673;

		public static double Min(params double[] values)
		{
			double min = values[0];
			for (int i = 1; i < values.Length; ++i)
			{
				if (values[i] < min)
					min = values[i];
			}
			return min;
		}

		public static double Max(params double[] values)
		{
			double max = values[0];
			for (int i = 1; i < values.Length; ++i)
			{
				if (values[i] > max)
					max = values[i];
			}
			return max;
		}

		public static double Floor(double value)
		{
			if (value > 0)
				return (int)value;
			else if (value < 0)
				return ((int)value) + 1;
			else
				return 0;
		}

		public static double Ceil(double value)
		{
			if (value < 0)
				return (int)value;
			else if (value > 0)
				return ((int)value) + 1;
			else
				return 0;
		}

		public static int FloorToInt(double value)
		{
			if (value > 0)
				return (int)value;
			else if (value < 0)
				return ((int)value) + 1;
			else
				return 0;
		}

		public static int CeilToInt(double value)
		{
			if (value < 0)
				return (int)value;
			else if (value > 0)
				return ((int)value) + 1;
			else
				return 0;
		}

		public static double Abs(double value)
		{
			if (value < 0)
				return -value;
			return value;
		}

		public static double Pow(double v, double p)
		{
			return System.Math.Pow(v, p);
		}

		public static Double3 Pow(Double3 v, double p)
		{
			return new Double3(Pow(v.x, p), Pow(v.y, p), Pow(v.z, p));
		}
	}
}