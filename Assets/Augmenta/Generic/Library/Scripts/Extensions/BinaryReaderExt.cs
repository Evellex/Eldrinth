using UnityEngine;
using System.IO;

namespace Augmenta
{
	public static class BinaryReaderExt
	{
		public static Short3 ReadShort3(this BinaryReader reader)
		{
			Short3 output = Short3.zero;
			output.x = reader.ReadInt16();
			output.y = reader.ReadInt16();
			output.z = reader.ReadInt16();
			return output;
		}

		public static Short2 ReadShort2(this BinaryReader reader)
		{
			Short2 output = Short2.zero;
			output.x = reader.ReadInt16();
			output.y = reader.ReadInt16();
			return output;
		}
	}
}
