using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	[System.Serializable]
	public class Condition
	{
		[SerializeField]
		private ConditionType type;

		[SerializeField]
		private float magnitude = 1;
	}
}