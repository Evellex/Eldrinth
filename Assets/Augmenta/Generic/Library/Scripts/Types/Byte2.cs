using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Byte2
	{
		public byte x, y;

		public static bool operator ==(Byte2 a, Byte2 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			Byte2 p = (Byte2)obj;
			if ((System.Object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public bool Equals(Byte2 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public override int GetHashCode()
		{
			return x ^ y;
		}

		public static bool operator !=(Byte2 a, Byte2 b)
		{
			return !(a == b);
		}

		public Byte2(byte x, byte y)
		{
			this.x = x;
			this.y = y;
		}

		public Byte2(int x, int y)
		{
			this.x = (byte)x;
			this.y = (byte)y;
		}

		public Byte2(Byte2 t)
		{
			x = t.x;
			y = t.y;
		}

		public static Byte2 operator +(Byte2 a, Byte2 b)
		{
			return new Byte2(a.x + b.x, a.y + b.y);
		}

		public static Byte2 operator /(Byte2 a, float b)
		{
			return new Byte2((byte)(a.x / b), (byte)(a.y / b));
		}

		public static Byte2 operator -(Byte2 a, Byte2 b)
		{
			return new Byte2(a.x - b.x, a.y - b.y);
		}

		public static implicit operator Vector2(Byte2 instance)
		{
			return new Vector2(Mathf.RoundToInt(instance.x), Mathf.RoundToInt(instance.y));
		}

		public static Byte2 zero = new Byte2(0, 0);
		public static Byte2 one = new Byte2(1, 1);
	}
}
