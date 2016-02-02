using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public class ConditionContainer : MonoBehaviour
	{
		private List<Condition> conditionList = new List<Condition>();

		public void AddCondition(Condition newCondition)
		{
			conditionList.Add(newCondition);
		}

		public void RemoveCondition(Condition newCondition)
		{
			conditionList.Remove(newCondition);
		}
	}
}