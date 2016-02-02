using System;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("Augmenta/Combat/Damage Event Action")]
	public class DamageEventAction : GenericEventAction, IDamageEvent
	{
		[SerializeField]
		[EnumMask]
		private DamageEventFlags triggerOn = (DamageEventFlags)0;

		void IDamageEvent.OnDamageSent(DamageInfo info)
		{
			if ((triggerOn & DamageEventFlags.OnDamageSent) > 0)
				Trigger();
		}

		void IDamageEvent.OnHealSent(DamageInfo info)
		{
			if ((triggerOn & DamageEventFlags.OnHealSent) > 0)
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