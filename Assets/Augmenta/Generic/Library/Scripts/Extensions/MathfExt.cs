using UnityEngine;

namespace Augmenta
{
	public static class MathfExt
	{
		public const float PI2 = 6.283185307179586476925286766559005768394338798750211641949889f;

		public const float mτToDeg = 0.36f;
		public const float DegTomτ = 2.777777777777777777777777777777777777777777777777777777777778f;

		public const float mτToRad = 0.006283185307179586476925286766559005768394338798750211641949f;
		public const float RadTomτ = 159.1549430918953357688837633725143620344596457404564487476673f;

		public const float τToDeg = 360.0f;
		public const float DegToτ = 0.0027777777777777777777777777777777777777777777777777777777778f;

		public const float τToRad = 6.283185307179586476925286766559005768394338798750211641949889f;
		public const float RadToτ = 0.1591549430918953357688837633725143620344596457404564487476673f;

		public static Int4 Clamp(Int4 v, int min, int max)
		{
			return new Int4(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max), Mathf.Clamp(v.w, min, max));
		}
	}
}