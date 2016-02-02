using System;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Particle System Event Action")]
	public class ParticleSystemEventAction : GenericEventAction
	{
		[SerializeField]
		[EnumMask]
		private ParticleSystemEventFlags triggerOn = (ParticleSystemEventFlags)0;

		private new ParticleSystem particleSystem;

		public void SetTriggerOn(ParticleSystemEventFlags flags)
		{
			triggerOn = flags;
		}

		public void AddTriggerOn(ParticleSystemEventFlags flags)
		{
			triggerOn |= flags;
		}

		public void RemoveTriggerOn(ParticleSystemEventFlags flags)
		{
			triggerOn &= ~flags;
		}

		protected override void Update()
		{
			base.Update();
			if (!particleSystem.IsAlive() && (triggerOn & ParticleSystemEventFlags.OnFinished) > 0)
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

		private void Start()
		{
			particleSystem = GetComponent<ParticleSystem>();
		}
	}
}