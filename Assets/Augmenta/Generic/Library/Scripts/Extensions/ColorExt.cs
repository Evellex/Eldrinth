using UnityEngine;
using System.IO;

namespace Augmenta
{
	public static class ColorExt
	{
		static Color[] temperatureColours;

		static ColorExt()
		{
			Texture2D temperatureLookup = Resources.Load("Augmenta/TemperatureLookup") as Texture2D;
			temperatureColours = temperatureLookup.GetPixels(0, 0, temperatureLookup.width, 1);
		}

		public static Color ReadColor(this BinaryReader reader)
		{
			Color output;
			output.r = reader.ReadSingle();
			output.g = reader.ReadSingle();
			output.b = reader.ReadSingle();
			output.a = reader.ReadSingle();
			return output;
		}

		public static void Write(this BinaryWriter writer, Color input)
		{
			writer.Write(input.r);
			writer.Write(input.g);
			writer.Write(input.b);
			writer.Write(input.a);
		}

		public static Color ColourTemperature(float kelvinTemperature)
		{
			kelvinTemperature = Mathf.Clamp(kelvinTemperature, 0, 10000);
			int index = Mathf.FloorToInt(kelvinTemperature / (10000.0f / 2047.0f));
			Vector4 colorTemp = temperatureColours[index];			
			colorTemp = new Color(colorTemp.x, colorTemp.y, colorTemp.z, 1);
			return colorTemp;
		}
	}
}