namespace Augmenta
{
	[System.Serializable]
	public struct Int3
	{
		public static Int3 zero = new Int3(0, 0, 0);
		public static Int3 one = new Int3(1, 1, 1);
		public static Int3 left = new Int3(-1, 0, 0);
		public static Int3 right = new Int3(1, 0, 0);
		public static Int3 up = new Int3(0, 1, 0);
		public static Int3 down = new Int3(0, -1, 0);
		public static Int3 forward = new Int3(0, 0, 1);
		public static Int3 back = new Int3(0, 0, -1);
		public int x, y, z;

		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public int this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else if (i == 2) return z; else { UnityEngine.Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; }
		}

		public static bool operator ==(Int3 a, Int3 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		public static bool operator !=(Int3 a, Int3 b)
		{
			return !(a == b);
		}

		public static Double3 operator /(Int3 v1, double s)
		{
			return new Double3(v1.x / s, v1.y / s, v1.z / s);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			Int3 p = (Int3)obj;
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public bool Equals(Int3 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z);
		}

		public override int GetHashCode()
		{
			return x ^ y ^ z;
		}

		public override string ToString()
		{
			return "X:(" + x + ") X:(" + y + ") Z:(" + z + ")";
		}
	}
}