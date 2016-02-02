using UnityEngine;

namespace Augmenta
{
	public struct Double3x3
	{
		public double m00, m01, m02;

		public double m10, m11, m12;

		public double m20, m21, m22;

		public Double3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
		{
			this.m00 = m00;
			this.m01 = m01;
			this.m02 = m02;
			this.m10 = m10;
			this.m11 = m11;
			this.m12 = m12;
			this.m20 = m20;
			this.m21 = m21;
			this.m22 = m22;
		}

		public static Double3 operator *(Double3x3 m, Double3 v)
		{
			return new Double3((m.m00 * v.x) + (m.m01 * v.y) + (m.m02 * v.z), (m.m10 * v.x) + (m.m11 * v.y) + (m.m12 * v.z), (m.m20 * v.x) + (m.m21 * v.y) + (m.m22 * v.z));
		}

		public static Vector3 operator *(Double3x3 m, Vector3 v)
		{
			return new Vector3((float)((m.m00 * v.x) + (m.m01 * v.y) + (m.m02 * v.z)), (float)((m.m10 * v.x) + (m.m11 * v.y) + (m.m12 * v.z)), (float)((m.m20 * v.x) + (m.m21 * v.y) + (m.m22 * v.z)));
		}

		public static explicit operator Double3x3(Float3x3 m)
		{
			return new Double3x3(m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);
		}

		public static Double3x3 Inverse(Double3x3 m)
		{
			double det = m.m00 * (m.m11 * m.m22 - m.m21 * m.m12) - m.m01 * (m.m10 * m.m22 - m.m12 * m.m20) + m.m02 * (m.m10 * m.m21 - m.m11 * m.m20);

			if (det == 0)
				return m;

			double invdet = 1 / det;

			Double3x3 minv;
			minv.m00 = (m.m11 * m.m22 - m.m21 * m.m12) * invdet;
			minv.m01 = (m.m02 * m.m21 - m.m01 * m.m22) * invdet;
			minv.m02 = (m.m01 * m.m12 - m.m02 * m.m11) * invdet;
			minv.m10 = (m.m12 * m.m20 - m.m10 * m.m22) * invdet;
			minv.m11 = (m.m00 * m.m22 - m.m02 * m.m20) * invdet;
			minv.m12 = (m.m10 * m.m02 - m.m00 * m.m12) * invdet;
			minv.m20 = (m.m10 * m.m21 - m.m20 * m.m11) * invdet;
			minv.m21 = (m.m20 * m.m01 - m.m00 * m.m21) * invdet;
			minv.m22 = (m.m00 * m.m11 - m.m10 * m.m01) * invdet;
			return minv;
		}

		public override string ToString()
		{
			return "(" + m00 + ")(" + m01 + ")(" + m02 + ")(" + m10 + ")(" + m11 + ")(" + m12 + ")(" + m20 + ")(" + m21 + ")(" + m22 + ")";
		}
	}
}