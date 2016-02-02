using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct Modifier
	{
		[SerializeField]
		private Type type;

		[SerializeField]
		private float value;

		public Modifier(Type type, float value)
		{
			this.type = type;
			this.value = value;
		}

		public enum Type
		{
			Multiply,
			Add,
			OverrideMax,
			OverrideMin,
		}

		public static float ApplyModifiers(float originalValue, List<Modifier> modifiers)
		{
			bool negative = false;
			float toAdd = 0;
			float toSubtract = 0;
			float toMult = 1;
			float overrideMax = Mathf.Infinity;
			float overrideMin = Mathf.NegativeInfinity;
			modifiers.ForEach(x =>
			{
				if (x.type == Type.Add)
				{
					if (x.value > 0)
						toAdd += x.value;
					else
						toSubtract -= x.value;
				}
				if (x.type == Type.Multiply)
				{
					toMult *= Mathf.Abs(x.value);
					negative |= x.value < 0;
				}
				if (x.type == Type.OverrideMax)
					overrideMax = Mathf.Min(overrideMax, x.value);
				if (x.type == Type.OverrideMax)
					overrideMin = Mathf.Max(overrideMin, x.value);
			});
			originalValue += toAdd;
			originalValue *= toMult;
			originalValue = Mathf.Max(0, originalValue - toSubtract);
			if (negative)
				originalValue *= -1;
			originalValue = Mathf.Clamp(originalValue, overrideMin, overrideMax);
			return originalValue;
		}
	}
}