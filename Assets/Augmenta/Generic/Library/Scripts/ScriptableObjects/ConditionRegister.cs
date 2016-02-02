using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public class ConditionRegister : ScriptableObject
	{
		private static ConditionRegister instance;
		private static string[] conditionTypeNames;
		private static string[] returnableConditionTypeNames;

		[SerializeField]
		private List<ConditionType> conditionTypes = new List<ConditionType>();

		public static string[] BuildConditionNameArray()
		{
			for (int i = 0; i < conditionTypeNames.Length; ++i)
				returnableConditionTypeNames[i] = string.Copy(conditionTypeNames[i]);
			return returnableConditionTypeNames;
		}

		public static ConditionType GetCondition(int index)
		{
			return instance.conditionTypes[index];
		}

		private void OnEnable()
		{
			if (instance == null)
				instance = this;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private void LoadInstance()
		{
			instance = Resources.Load<ConditionRegister>("Augmenta/Condition Register.asset");
			conditionTypeNames = new string[instance.conditionTypes.Count];
			returnableConditionTypeNames = new string[instance.conditionTypes.Count];
			int i = 0;
			instance.conditionTypes.ForEach(x => { conditionTypeNames[i++] = x.ConditionName; });
		}
	}
}