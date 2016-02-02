using UnityEngine;
using System.IO;

namespace Augmenta
{
	public static class BinaryWriterExt
	{
		public static void Write(this BinaryWriter writer, Short3 input)
		{
			writer.Write(input.x);
			writer.Write(input.y);
			writer.Write(input.z);
		}

		public static void Write(this BinaryWriter writer, Short2 input)
		{
			writer.Write(input.x);
			writer.Write(input.y);
		}
	}
}
