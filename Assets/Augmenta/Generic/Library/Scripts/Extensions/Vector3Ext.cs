using System.IO;
using UnityEngine;

namespace Augmenta
{
	public static class Vector3Ext
	{
		public static readonly Vector3 planeXY = new Vector3(1, 1, 0);
		public static readonly Vector3 planeXZ = new Vector3(1, 0, 1);
		public static readonly Vector3 planeYZ = new Vector3(0, 1, 1);

		public static void Multiply(this Vector3 first, Vector3 second)
		{
			first = Product(first, second);
		}

		public static void Divide(this Vector3 first, Vector3 second)
		{
			first = Quotient(first, second);
		}

		public static Vector3 Product(Vector3 first, Vector3 second)
		{
			Vector3 returnValue = new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
			return returnValue;
		}

		public static Vector3 Quotient(Vector3 first, Vector3 second)
		{
			Vector3 returnValue = new Vector3(first.x / second.x, first.y / second.y, first.z / second.z);
			return returnValue;
		}

		public static void Invert(this Vector3 first)
		{
			first = Inverse(first);
		}

		public static Vector3 Inverse(this Vector3 first)
		{
			Vector3 returnValue = new Vector3(1 / first.x, 1 / first.y, 1 / first.z);
			return returnValue;
		}

		public static Vector3 Uniform(float input)
		{
			return new Vector3(input, input, input);
		}

		public static bool Approximately(Vector3 a, Vector3 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
		}

		public static bool IsUniform(Vector3 a)
		{
			return Mathf.Approximately(a.x, a.y) && Mathf.Approximately(a.x, a.z);
		}

		public static void Clamp(this Vector3 vector, Vector3 min, Vector3 max)
		{
			vector = ClampResult(vector, min, max);
		}

		public static Vector3 ClampResult(this Vector3 vector, Vector3 min, Vector3 max)
		{
			return new Vector3(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y), Mathf.Clamp(vector.z, min.z, max.z));
		}

		public static Vector3 ReadVector3(this BinaryReader reader)
		{
			Vector3 output;
			output.x = reader.ReadSingle();
			output.y = reader.ReadSingle();
			output.z = reader.ReadSingle();
			return output;
		}

		public static void Write(this BinaryWriter writer, Vector3 input)
		{
			writer.Write(input.x);
			writer.Write(input.y);
			writer.Write(input.z);
		}
	}
}