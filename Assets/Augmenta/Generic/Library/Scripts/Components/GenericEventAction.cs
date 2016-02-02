using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("")]
	public abstract class GenericEventAction : MonoBehaviour, IPoolEvent
	{
		protected List<Hourglass> delayedActionTimes = new List<Hourglass>();

		protected bool triggered = false;
		protected bool actioned = false;

		[SerializeField]
		[EnumMask]
		protected EventActionSettingFlags settingsFlags = (EventActionSettingFlags)0;

		[SerializeField]
		protected float delayToAction = 0;

		[SerializeField]
		[HideInInspector]
		protected bool resetOnDisable = true;

		[SerializeField]
		[HideInInspector]
		//This is used purely to manage the migration of resetOnDisable to actionOnDisable
		private bool disableActionMigrated = false;

		[SerializeField]
		private EventActionDisableAction actionOnDisable = EventActionDisableAction.Reset;

		[SerializeField]
		private UnityEngine.Events.UnityEvent onTrigger;

		public void ActionImmediate()
		{
			if ((settingsFlags & EventActionSettingFlags.ActionOnlyOnce) == 0 || !actioned)
			{
				actioned = true;
				onTrigger.Invoke();
			}
		}

		public void ActionDelayed(float delay)
		{
			if ((settingsFlags & EventActionSettingFlags.ActionOnlyOnce) == 0 || !actioned)
			{
				Hourglass newHoruglass = new Hourglass(delay, Hourglass.CountdownType.ScaledTime);
				newHoruglass.Paused = !isActiveAndEnabled;
				delayedActionTimes.Add(newHoruglass);
			}
		}

		public void InstantiateObject(GameObject prefab)
		{
			ObjectPooler.Withdraw(prefab, transform.position, transform.rotation);
		}

		public new void DestroyObject(Object target)
		{
			if (target.GetType() == typeof(Transform))
				ObjectPooler.Deposit(((Transform)target).gameObject);
			else if (target.GetType() == typeof(GameObject))
				ObjectPooler.Deposit((GameObject)target);
			else
				Destroy(target);
		}

		public void OrphanTransform(Transform target)
		{
			target.SetParent(null);
		}

		void IPoolEvent.OnDeposit()
		{
			ResetState();
		}

		void IPoolEvent.OnWithdraw()
		{
		}

		protected virtual void Update()
		{
			delayedActionTimes.ForEach(x => { if (x.Expired) ActionImmediate(); });
			delayedActionTimes.RemoveAll(x => x.Expired);
		}

		protected void Trigger()
		{
			if ((settingsFlags & EventActionSettingFlags.TriggerOnlyOnce) == 0 || !triggered)
			{
				triggered = true;
				if (delayToAction <= 0)
					ActionImmediate();
				else
					ActionDelayed(delayToAction);
			}
		}

		protected abstract void OnEventActionReset();

		protected abstract void OnEventActionPause();

		protected abstract void OnEventActionResume();

		protected virtual void OnEnable()
		{
			ResumeState();
		}

		protected virtual void OnDisable()
		{
			if (actionOnDisable == EventActionDisableAction.Reset)
				ResetState();
			else if (actionOnDisable == EventActionDisableAction.Pause)
				PauseState();
		}

		private void ResumeState()
		{
			delayedActionTimes.ForEach(x => x.Paused = false);
			OnEventActionResume();
		}

		private void PauseState()
		{
			delayedActionTimes.ForEach(x => x.Paused = true);
			OnEventActionPause();
		}

		private void ResetState()
		{
			triggered = false;
			actioned = false;
			delayedActionTimes.Clear();
			OnEventActionReset();
		}

		private void OnValidate()
		{
			delayToAction = Mathf.Max(0, delayToAction);
			if (disableActionMigrated == false)
			{
				if (resetOnDisable == true)
					actionOnDisable = EventActionDisableAction.Reset;
				else
					actionOnDisable = EventActionDisableAction.Pause;
				disableActionMigrated = true;
			}
		}
	}
}