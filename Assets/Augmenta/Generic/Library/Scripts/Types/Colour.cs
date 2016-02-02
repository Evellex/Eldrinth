using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public class Colour
	{
		private static Dictionary<RGBSpace, RGBDefinition> rgbSpaces = new Dictionary<RGBSpace, RGBDefinition>(2);

		private Double3 val;

		static Colour()
		{
			RGBDefinition sRGB = new RGBDefinition();
			sRGB.r = new Double2(0.64, 0.33);
			sRGB.g = new Double2(0.3, 0.6);
			sRGB.b = new Double2(0.15, 0.06);
			sRGB.w = new Double3(0.3127, 0.329, 1);
			sRGB.gamma = 2.2;
			BuildConversionMatrices(sRGB);
			rgbSpaces.Add(RGBSpace.sRGB, sRGB);

			RGBDefinition AdobeRGB = new RGBDefinition();
			AdobeRGB.r = new Double2(0.64, 0.33);
			AdobeRGB.g = new Double2(0.21, 0.71);
			AdobeRGB.b = new Double2(0.15, 0.06);
			AdobeRGB.w = new Double3(0.3127, 0.329, 1);
			AdobeRGB.gamma = 563.0 / 256.0;
			BuildConversionMatrices(AdobeRGB);
			rgbSpaces.Add(RGBSpace.AdobeRGB, AdobeRGB);
		}

		public Colour(Double3 coords)
		{
			val = coords;
		}

		public Colour()
		{
			val = new Double3();
		}

		public enum Model
		{
			RGB,
			HSV,
			HSL,

			CMYK,

			XYZ,
			xyY,
			Lab,
			LCH,
		}

		public enum RGBSpace
		{
			sRGB,
			AdobeRGB,
		}

		public double X
		{
			get { return val.x; }
			set { val.x = value; }
		}

		public double Y
		{
			get { return val.y; }
			set { val.y = value; }
		}

		public double Z
		{
			get { return val.z; }
			set { val.z = value; }
		}

		public static Colour From_RGB(Double3 rgb, RGBSpace space)
		{
			RGBDefinition d = rgbSpaces[space];
			Double3 linear = Mathd.Pow(rgb, d.gamma);
			return new Colour(d.toXYZ * linear);
		}

		public static Colour From_Lab(Double3 lab)
		{
			lab.x = (lab.x * 30) - 15;
			lab.y = (lab.y * 60) - 30;
			lab.z = (lab.z * 60) - 30;
			Colour col = new Colour();
			Double3 w = new Double3(95.047, 100.000, 108.883);
			System.Func<double, double> f = new System.Func<double, double>((x) =>
			{
				if (x > Mathd.Pow(6.0 / 29.0, 3.0))
					return Mathd.Pow(x, 3.0);
				else
					return 3.0 * Mathd.Pow(6.0 / 29.0, 2.0) * (x - (4.0 / 29.0));
			});
			col.X = w.x * f(((1.0 / 116.0) * (lab.x + 16)) + ((1.0 / 500.0) * lab.y));
			col.Y = w.y * f((1.0 / 116.0) * (lab.x + 16));
			col.Z = w.z * f(((1.0 / 116.0) * (lab.x + 16)) - ((1.0 / 200.0) * lab.z));

			return col;
		}

		public static bool IsInsideGamut(Colour currentColour, RGBSpace limitingSpace)
		{
			Double3 limitingGamutColour = To_RGB(currentColour, limitingSpace);
			if (limitingGamutColour.MaxVal > 1.0 || limitingGamutColour.MinVal < 0.0 || double.IsNaN(limitingGamutColour.x) || double.IsNaN(limitingGamutColour.y) || double.IsNaN(limitingGamutColour.z))
				return false;
			return true;
		}

		public static Double3 To_Lab(Colour col)
		{
			Double3 lab = new Double3();
			Double3 w = new Double3(95.047, 100.000, 108.883);
			System.Func<double, double> f = new System.Func<double, double>((x) =>
			{
				if (x > Mathd.Pow(6.0 / 29.0, 3))
					return Mathd.Pow(x, (1.0 / 3.0));
				else
					return ((1.0 / 3.0) * Mathd.Pow(29.0 / 6.0, 2.0) * x) + (4.0 / 29.0);
			});

			lab.x = (166.0 * f(col.Y / w.y)) - 16;
			lab.y = 500.0 * (f(col.X / w.x) - f(col.Y / w.y));
			lab.z = 200.0 * (f(col.Y / w.y) - f(col.Z / w.z));

			lab.x = (lab.x + 15) * (1.0 / 30.0);
			lab.y = (lab.y + 30) * (1.0 / 50.0);
			lab.z = (lab.z + 30) * (1.0 / 50.0);
			return lab;
		}

		public static Colour From_HSV(Double3 hsv, RGBSpace space)
		{
			double c = hsv.z * hsv.y;
			double x = c * (1 - Mathd.Abs(((hsv.x * 6.0) % 2) - 1));
			double m = hsv.z - c;
			double h = hsv.x * 6;
			int i = Mathd.FloorToInt(h);
			switch (i)
			{
				case 0:
					return From_RGB(new Double3(c + m, x + m, m), space);

				case 1:
					return From_RGB(new Double3(x + m, c + m, m), space);

				case 2:
					return From_RGB(new Double3(m, c + m, x + m), space);

				case 3:
					return From_RGB(new Double3(m, x + m, c + m), space);

				case 4:
					return From_RGB(new Double3(x + m, m, c + m), space);

				default:
					return From_RGB(new Double3(c + m, m, x + m), space);
			}
		}

		public static Colour From_HSL(Double3 hsl, RGBSpace space)
		{
			double c = (1 - Mathd.Abs((2.0 * hsl.z) - 1)) * hsl.y;
			double x = c * (1 - Mathd.Abs(((hsl.x * 6.0) % 2) - 1));
			double m = hsl.z - (c / 2.0);
			double h = hsl.x * 6;
			int i = Mathd.FloorToInt(h);
			switch (i)
			{
				case 0:
					return From_RGB(new Double3(c + m, x + m, m), space);

				case 1:
					return From_RGB(new Double3(x + m, c + m, m), space);

				case 2:
					return From_RGB(new Double3(m, c + m, x + m), space);

				case 3:
					return From_RGB(new Double3(m, x + m, c + m), space);

				case 4:
					return From_RGB(new Double3(x + m, m, c + m), space);

				default:
					return From_RGB(new Double3(c + m, m, x + m), space);
			}
		}

		public static Double3 To_RGB(Colour col, RGBSpace space)
		{
			RGBDefinition d = rgbSpaces[space];
			Double3 linear = d.fromXYZ * col.val;
			return Mathd.Pow(linear, 1.0 / d.gamma);
		}

		public static Double3 To_HSV(Colour col, RGBSpace space)
		{
			Double3 hsv;
			Double3 rgb = To_RGB(col, space);
			double min = Mathd.Min(rgb.x, rgb.y, rgb.z);
			double max = Mathd.Max(rgb.x, rgb.y, rgb.z);
			double delta = max - min;
			if (max == 0)
			{
				return new Double3(0, 0, 0);
			}
			hsv.y = delta / max;
			hsv.z = max;

			if (rgb.x == max)
				hsv.x = (rgb.y - rgb.z) / delta;
			else if (rgb.y == max)
				hsv.x = 2 + (rgb.z - rgb.x) / delta;
			else
				hsv.x = 4 + (rgb.x - rgb.y) / delta;

			hsv.x *= 60;
			if (hsv.x < 0)
				hsv.x += 360;
			return hsv;
		}

		public static Double3 To_HSL(Colour col, RGBSpace space)
		{
			Double3 hsl;
			Double3 rgb = To_RGB(col, space);
			double min = Mathd.Min(rgb.x, rgb.y, rgb.z);
			double max = Mathd.Max(rgb.x, rgb.y, rgb.z);
			double delta = max - min;
			hsl.z = max - min / 2.0;
			if (delta == 0)
			{
				hsl.x = 0;
				hsl.y = 0;
				return hsl;
			}

			if (rgb.x == max)
				hsl.x = (rgb.y - rgb.z) / delta;
			else if (rgb.y == max)
				hsl.x = 2 + (rgb.z - rgb.x) / delta;
			else
				hsl.x = 4 + (rgb.x - rgb.y) / delta;

			hsl.x *= 60;
			if (hsl.x < 0)
				hsl.x += 360;

			hsl.y = delta / (1 - (Mathd.Abs((2.0 * hsl.z) - 1)));

			return hsl;
		}

		public static Colour From_xyY(Double3 xyY)
		{
			if (xyY.y == 0)
				return new Colour(Double3.zero);
			else
				return new Colour(new Double3((xyY.x * xyY.z) / xyY.y, xyY.z, ((1 + (-xyY.x) + (-xyY.y)) * xyY.z) / xyY.y));
		}

		public static Double3 To_xyY(Colour col)
		{
			return new Double3(col.X / (col.X + col.Y + col.Z), col.Y / (col.X + col.Y + col.Z), col.Y);
		}

		public override string ToString()
		{
			return val.ToString();
		}

		private static void BuildConversionMatrices(RGBDefinition definition)
		{
			RGBDefinition d = definition;
			Double3 R = new Double3(d.r.x / d.r.y, 1.0, (1.0 - d.r.x - d.r.y) / d.r.y);
			Double3 G = new Double3(d.g.x / d.g.y, 1.0, (1.0 - d.g.x - d.g.y) / d.g.y);
			Double3 B = new Double3(d.b.x / d.b.y, 1.0, (1.0 - d.b.x - d.b.y) / d.b.y);
			Double3 W = new Double3(d.w.x / d.w.y, 1.0, (1.0 - d.w.x - d.w.y) / d.w.y);
			Double3x3 altMat = new Double3x3(R.x, G.x, B.x, R.y, G.y, B.y, R.z, G.z, B.z);
			Double3 s = Double3x3.Inverse(altMat) * W;
			d.toXYZ = new Double3x3(s.x * R.x, s.y * G.x, s.z * B.x, s.x * R.y, s.y * G.y, s.z * B.y, s.x * R.z, s.y * G.z, s.z * B.z);
			d.fromXYZ = Double3x3.Inverse(d.toXYZ);
		}

		private class RGBDefinition
		{
			public Double2 r;
			public Double2 g;
			public Double2 b;
			public Double3 w;
			public double gamma;
			public Double3x3 toXYZ;
			public Double3x3 fromXYZ;
		}
	}
}