using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Short2
	{
		public static Short2 zero = new Short2(0, 0);
		public static Short2 left = new Short2(-1, 0);
		public static Short2 right = new Short2(1, 0);
		public static Short2 up = new Short2(0, 1);
		public static Short2 down = new Short2(0, -1);
		public short x, y;

		public Short2(short x, short y)
		{
			this.x = x;
			this.y = y;
		}

		public Short2(Short2 t)
		{
			this.x = t.x;
			this.y = t.y;
		}

		public static bool operator ==(Short2 a, Short2 b)
		{
			if (System.Object.ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Short2 a, Short2 b)
		{
			return !(a == b);
		}

		public static explicit operator Short3(Short2 original)
		{
			return new Short3(original.x, original.y, 0);
		}

		public static explicit operator Vector2(Short2 original)
		{
			return new Vector2(original.x, original.y);
		}

		public static explicit operator Vector3(Short2 original)
		{
			return new Vector3(original.x, original.y, 0);
		}

		public override bool Equals(System.Object obj)
		{
			if (obj == null)
				return false;

			Short2 p = (Short2)obj;
			if ((System.Object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public bool Equals(Short2 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public override int GetHashCode()
		{
			return x ^ y;
		}
	}
}