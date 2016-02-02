using UnityEngine;
using System.IO;

namespace Augmenta
{
	public static class Vector2Ext
	{
		public static void Multiply(this Vector2 first, Vector2 second)
		{
			first = Product(first, second);
		}

		public static void Divide(this Vector2 first, Vector2 second)
		{
			first = Quotient(first, second);
		}

		public static Vector2 Product(Vector2 first, Vector2 second)
		{
			Vector2 returnValue = new Vector2(first.x * second.x, first.y * second.y);
			return returnValue;
		}

		public static Vector2 Quotient(Vector2 first, Vector2 second)
		{
			Vector2 returnValue = new Vector2(first.x / second.x, first.y / second.y);
			return returnValue;
		}

		public static void Invert(this Vector2 first)
		{
			first = Inverse(first);
		}

		public static Vector2 Inverse(this Vector2 first)
		{
			Vector2 returnValue = new Vector2(1 / first.x, 1 / first.y);
			return returnValue;
		}

		public static Vector2 Uniform(float input)
		{
			return new Vector2(input, input);
		}

		public static bool Approximately(Vector2 a, Vector2 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
		}

		public static bool IsUniform(Vector2 a)
		{
			return Mathf.Approximately(a.x, a.y);
		}

		public static void Clamp(this Vector2 vector, Vector2 min, Vector2 max)
		{
			vector = ClampResult(vector, min, max);
		}

		public static Vector2 ClampResult(this Vector2 vector, Vector2 min, Vector2 max)
		{
			return new Vector2(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y));
		}

		public static Vector2 ReadVector2(this BinaryReader reader)
		{
			Vector2 output;
			output.x = reader.ReadSingle();
			output.y = reader.ReadSingle();
			return output;
		}
		public static void Write(this BinaryWriter writer, Vector2 input)
		{
			writer.Write(input.x);
			writer.Write(input.y);
		}
	}
}
