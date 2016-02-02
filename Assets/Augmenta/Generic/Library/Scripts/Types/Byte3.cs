using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Byte3
	{
		public byte x, y, z;

		public static bool operator ==(Byte3 a, Byte3 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			Byte3 p = (Byte3)obj;
			if ((System.Object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public bool Equals(Byte3 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public override int GetHashCode()
		{
			return x ^ y ^ z;
		}

		public static bool operator !=(Byte3 a, Byte3 b)
		{
			return !(a == b);
		}

		public Byte3(byte x, byte y, byte z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Byte3(int x, int y, int z)
		{
			this.x = (byte)x;
			this.y = (byte)y;
			this.z = (byte)z;
		}

		public Byte3(Byte3 t)
		{
			x = t.x;
			y = t.y;
			z = t.z;
		}

		public static Byte3 operator +(Byte3 a, Byte3 b)
		{
			return new Byte3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Byte3 operator /(Byte3 a, float b)
		{
			return new Byte3((byte)(a.x / b), (byte)(a.y / b), (byte)(a.z / b));
		}

		public static Byte3 operator -(Byte3 a, Byte3 b)
		{
			return new Byte3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static implicit operator Vector3 (Byte3 instance)
		{
			return new Vector3(Mathf.RoundToInt(instance.x), Mathf.RoundToInt(instance.y), Mathf.RoundToInt(instance.z));
		}

		public static Byte3 zero = new Byte3(0, 0, 0);
		public static Byte3 one = new Byte3(1, 1, 1);
	}
}
