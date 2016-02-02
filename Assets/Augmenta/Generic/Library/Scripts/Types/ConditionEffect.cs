using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public struct ConditionEffect
	{
		[SerializeField]
		private Type type;

		[SerializeField]
		private Modifier.Type modifierType;

		[SerializeField]
		private float value;

		[SerializeField]
		private ConditionScale conditionScaling;

		public enum ConditionScale
		{
			Linear,
			Square,
			Inverse,
			SquareInverse,
		}

		public enum Type
		{
			MaxSpeed,
			Acceleration,
			TurnSpeed,
			Transparency,
			Size,
			AirControl,
			Timescale,
		}
	}
}