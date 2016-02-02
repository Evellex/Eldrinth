namespace Augmenta
{
	public static class Math
	{
		public const double PI2 = 6.283185307179586476925286766559005768394338798750211641949889;

		public const double mτToDeg = 0.36f;
		public const double DegTomτ = 2.777777777777777777777777777777777777777777777777777777777778;

		public const double mτToRad = 0.006283185307179586476925286766559005768394338798750211641949;
		public const double RadTomτ = 159.1549430918953357688837633725143620344596457404564487476673;

		public const double τToDeg = 360.0f;
		public const double DegToτ = 0.0027777777777777777777777777777777777777777777777777777777778;

		public const double τToRad = 6.283185307179586476925286766559005768394338798750211641949889;
		public const double RadToτ = 0.1591549430918953357688837633725143620344596457404564487476673;

		public static Int4 Clamp(Int4 v, int min, int max)
		{
			return new Int4(Clamp(v.x, min, max), Clamp(v.y, min, max), Clamp(v.z, min, max), Clamp(v.w, min, max));
		}

		public static int Clamp(int s, int min, int max)
		{
			if (s < min)
				return min;
			if (s > max)
				return max;
			return s;
		}

		public static double Clamp(double s, double min, double max)
		{
			if (s < min)
				return min;
			if (s > max)
				return max;
			return s;
		}

		public static int FloorToInt(double value)
		{
			if (value > 0)
				return (int)value;
			else if (value < 0)
				return ((int)value) - 1;
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

		public static double Max(params double[] values)
		{
			double max = values[0];
			for (int i = 1; i < values.Length; ++i)
				max = System.Math.Max(max, values[i]);
			return max;
		}

		public static double Min(params double[] values)
		{
			double min = values[0];
			for (int i = 1; i < values.Length; ++i)
				min = System.Math.Min(values[i], min);
			return min;
		}

		public static Double3 Pow(Double3 v, double p)
		{
			return new Double3(System.Math.Pow(v.x, p), System.Math.Pow(v.y, p), System.Math.Pow(v.z, p));
		}
	}
}