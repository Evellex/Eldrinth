using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Short3
	{
		public static Short3 zero = new Short3(0, 0, 0);
		public static Short3 left = new Short3(-1, 0, 0);
		public static Short3 right = new Short3(1, 0, 0);
		public static Short3 up = new Short3(0, 1, 0);
		public static Short3 down = new Short3(0, -1, 0);
		public static Short3 forward = new Short3(0, 0, 1);
		public static Short3 back = new Short3(0, 0, -1);
		public short x, y, z;

		public Short3(short x, short y, short z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Short3(Short3 t)
		{
			this.x = t.x;
			this.y = t.y;
			this.z = t.z;
		}

		public static bool operator ==(Short3 a, Short3 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		public static bool operator !=(Short3 a, Short3 b)
		{
			return !(a == b);
		}

		public static explicit operator Short2(Short3 original)
		{
			return new Short2(original.x, original.y);
		}

		public static explicit operator Vector3(Short3 original)
		{
			return new Vector3(original.x, original.y, original.z);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			Short3 p = (Short3)obj;
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public bool Equals(Short3 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public override int GetHashCode()
		{
			return x ^ y ^ z;
		}
	}
}