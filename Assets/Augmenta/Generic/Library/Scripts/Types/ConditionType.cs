using UnityEngine;
using System.Collections.Generic;

namespace Augmenta
{
	[System.Serializable]
	public class ConditionType
	{
		[SerializeField]
		private string conditionName;

		[SerializeField]
		private CombineType selfCombineType;

		[SerializeField]
		private List<ConditionEffect> effectsList;

		public enum CombineType
		{
			LargestEffect,
			Sum,
		}

		public string ConditionName
		{
			get { return conditionName; }
		}
	}
}