using UnityEngine;

namespace Augmenta
{
	public struct Float3x3
	{
		public Float3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
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

		public static Vector3 operator * (Float3x3 m, Vector3 v)
		{
			return new Vector3((m.m00 * v.x) + (m.m01 * v.y) + (m.m02 * v.z), (m.m10 * v.x) + (m.m11 * v.y) + (m.m12 * v.z), (m.m20 * v.x) + (m.m21 * v.y) + (m.m22 * v.z));
		}

		public static explicit operator Float3x3(Double3x3 m)
		{
			return new Float3x3((float)m.m00, (float)m.m01, (float)m.m02, (float)m.m10, (float)m.m11, (float)m.m12, (float)m.m20, (float)m.m21, (float)m.m22);
		}

		public float m00, m01, m02;
		public float m10, m11, m12;
		public float m20, m21, m22;
	}
}
