using System;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("Augmenta/Combat/Health Event Action")]
	public class HealthEventAction : GenericEventAction, IHealthEvent
	{
		[SerializeField]
		[EnumMask]
		private HealthEventFlags triggerOn = (HealthEventFlags)0;

		void IHealthEvent.OnDamageReceived(DamageInfo info)
		{
			if ((triggerOn & HealthEventFlags.OnDamageReceived) > 0)
				Trigger();
		}

		void IHealthEvent.OnHealReceived(DamageInfo info)
		{
			if ((triggerOn & HealthEventFlags.OnHealReceived) > 0)
				Trigger();
		}

		void IHealthEvent.OnDeath(DamageInfo info)
		{
			if ((triggerOn & HealthEventFlags.OnDeath) > 0)
				Trigger();
		}

		protected override void OnEventActionReset()
		{
		}

		protected override void OnEventActionPause()
		{
		}

		protected override void OnEventActionResume()
		{
		}
	}
}