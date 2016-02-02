using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Int2
	{
		public static Int2 zero = new Int2(0, 0);
		public static Int2 one = new Int2(1, 1);
		public static Int2 left = new Int2(-1, 0);
		public static Int2 right = new Int2(1, 0);
		public static Int2 up = new Int2(0, 1);
		public static Int2 down = new Int2(0, -1);
		public int x, y;

		public Int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else { Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else { Debug.LogException(new System.IndexOutOfRangeException()); } }
		}

		public static bool operator ==(Int2 a, Int2 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Int2 a, Int2 b)
		{
			return !(a == b);
		}

		public static Int2 operator +(Int2 a, Int2 b)
		{
			return new Int2(a.x + b.x, a.y + b.y);
		}

		public static Int2 operator -(Int2 a, Int2 b)
		{
			return new Int2(a.x - b.x, a.y - b.y);
		}

		public static explicit operator Int2(Vector2 vec)
		{
			return new Int2((int)vec.x, (int)vec.y);
		}

		public static explicit operator Vector2(Int2 vec)
		{
			return new Vector2(vec.x, vec.y);
		}

		public override bool Equals(System.Object obj)
		{
			if (obj == null)
				return false;

			Int2 p = (Int2)obj;
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public bool Equals(Int2 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y);
		}

		public override int GetHashCode()
		{
			return x ^ y;
		}

		public override string ToString()
		{
			return "X:(" + x + ") X:(" + y + ")";
		}
	}
}