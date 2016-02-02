using UnityEngine;
using System.IO;

namespace Augmenta
{
	public static class Vector4Ext
	{
		public static void Multiply(this Vector4 first, Vector4 second)
		{
			first = Product(first,second);
		}

		public static void Divide(this Vector4 first, Vector4 second)
		{
			first = Quotient(first, second);
		}

		public static Vector4 Product(Vector4 first, Vector4 second)
		{
			Vector4 returnValue = new Vector4(first.x * second.x, first.y * second.y, first.z * second.z, first.w * second.w);
			return returnValue;
		}

		public static Vector4 Quotient(Vector4 first, Vector4 second)
		{
			Vector4 returnValue = new Vector4(first.x / second.x, first.y / second.y, first.z / second.z, first.w / second.w);
			return returnValue;
		}

		public static void Invert(this Vector4 first)
		{
			first = Inverse(first);
		}

		public static Vector4 Inverse(this Vector4 first)
		{
			Vector4 returnValue = new Vector4(1 / first.x, 1 / first.y, 1 / first.z, 1 / first.w);
			return returnValue;
		}		

		public static Vector4 Uniform(float input)
		{
			return new Vector4(input, input, input, input);
		}

		public static bool Approximately(Vector4 a, Vector4 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z) && Mathf.Approximately(a.w, b.w);
		}

		public static bool IsUniform(Vector4 a)
		{
			return Mathf.Approximately(a.x, a.y) && Mathf.Approximately(a.x, a.z) && Mathf.Approximately(a.x, a.w);
		}

		public static void Clamp(this Vector4 vector, Vector4 min, Vector4 max)
		{
			vector = ClampResult(vector, min, max);
		}

		public static Vector4 ClampResult(this Vector4 vector, Vector4 min, Vector4 max)
		{
			return new Vector4(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y), Mathf.Clamp(vector.z, min.z, max.z), Mathf.Clamp(vector.w, min.w, max.w));
		}

		public static Vector4 ReadVector4(this BinaryReader reader)
		{
			Vector4 output;
			output.x = reader.ReadSingle();
			output.y = reader.ReadSingle();
			output.z = reader.ReadSingle();
			output.w = reader.ReadSingle();
			return output;
		}
		public static void Write(this BinaryWriter writer, Vector4 input)
		{
			writer.Write(input.x);
			writer.Write(input.y);
			writer.Write(input.z);
			writer.Write(input.w);
		}
	}
}
