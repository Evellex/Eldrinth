using System.Collections;
using UnityEngine;

namespace Augmenta
{
	public class ConditionApplier : MonoBehaviour
	{
		[SerializeField]
		private Condition condition;

		[SerializeField]
		private DurationType durationType;

		[SerializeField]
		private float duration;

		private enum DurationType
		{
			WhileInTrigger,
			SpecificDuration,
		}

		private static void ApplyCondition(ConditionContainer targetContainer, Condition newCondition)
		{
		}

		private void OnTriggerEnter(Collider other)
		{
			GameObject target = other.GetPhysicsRoot().gameObject;
			ConditionContainer container = target.GetOrAddComponent<ConditionContainer>();
			if (container != null)
				container.AddCondition(condition);

			if (durationType == DurationType.SpecificDuration)
				Invoker.Invoke(() => container.RemoveCondition(condition), new Hourglass(duration, Hourglass.CountdownType.ScaledTime));
		}

		private void OnTriggerExit(Collider other)
		{
			if (durationType == DurationType.WhileInTrigger)
			{
				GameObject target = other.GetPhysicsRoot().gameObject;
				ConditionContainer container = target.GetComponent<ConditionContainer>();
				if (container != null)
					container.RemoveCondition(condition);
			}
		}
	}
}