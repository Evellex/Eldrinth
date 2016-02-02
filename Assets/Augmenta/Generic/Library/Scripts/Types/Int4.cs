namespace Augmenta
{
	[System.Serializable]
	public struct Int4
	{
		public static Int4 zero = new Int4(0, 0, 0, 0);
		public static Int4 one = new Int4(1, 1, 1, 1);
		public static Int4 left = new Int4(-1, 0, 0, 0);
		public static Int4 right = new Int4(1, 0, 0, 0);
		public static Int4 up = new Int4(0, 1, 0, 0);
		public static Int4 down = new Int4(0, -1, 0, 0);
		public static Int4 forward = new Int4(0, 0, 1, 0);
		public static Int4 back = new Int4(0, 0, -1, 0);
		public static Int4 ana = new Int4(0, 0, 0, 1);
		public static Int4 kata = new Int4(0, 0, 0, -1);

		public int x, y, z, w;

		public Int4(int x, int y, int z, int w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public int this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else if (i == 2) return z; else if (i == 3) return w; else { UnityEngine.Debug.LogException(new System.IndexOutOfRangeException()); return 0; } }
			set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else if (i == 3) w = value; else UnityEngine.Debug.LogException(new System.IndexOutOfRangeException()); }
		}

		public static bool operator ==(Int4 a, Int4 b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
		}

		public static bool operator !=(Int4 a, Int4 b)
		{
			return !(a == b);
		}

		public static Double4 operator /(Int4 v1, double s)
		{
			return new Double4(v1.x / s, v1.y / s, v1.z / s, v1.w / s);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			Int4 p = (Int4)obj;
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z) && (w == p.w);
		}

		public bool Equals(Int4 p)
		{
			if ((object)p == null)
				return false;

			return (x == p.x) && (y == p.y) && (z == p.z) && (w == p.w);
		}

		public override int GetHashCode()
		{
			return x ^ y ^ z ^ w;
		}

		public override string ToString()
		{
			return "X:(" + x + ") X:(" + y + ") Z:(" + z + ") W:(" + w + ")";
		}
	}
}