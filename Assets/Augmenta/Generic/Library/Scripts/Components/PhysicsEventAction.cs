using System;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Physics Event Action")]
	public class PhysicsEventAction : GenericEventAction
	{
		[SerializeField]
		[EnumMask]
		private PhysicsEventFlags triggerOn = (PhysicsEventFlags)0;

		private new Rigidbody rigidbody;

		protected override void Update()
		{
			base.Update();
			if (rigidbody && (triggerOn & PhysicsEventFlags.OnRigidbodySleep) > 0 && rigidbody.IsSleeping())
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
			rigidbody = GetComponent<Rigidbody>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if ((triggerOn & PhysicsEventFlags.OnCollisionEnter) > 0)
				Trigger();
		}

		private void OnCollisionStay(Collision collision)
		{
			if ((triggerOn & PhysicsEventFlags.OnCollisionStay) > 0)
				Trigger();
		}

		private void OnCollisionExit(Collision collision)
		{
			if ((triggerOn & PhysicsEventFlags.OnCollisionExit) > 0)
				Trigger();
		}

		private void OnTriggerEnter(Collider other)
		{
			if ((triggerOn & PhysicsEventFlags.OnTriggerEnter) > 0)
				Trigger();
		}

		private void OnTriggerStay(Collider other)
		{
			if ((triggerOn & PhysicsEventFlags.OnTriggerStay) > 0)
				Trigger();
		}

		private void OnTriggerExit(Collider other)
		{
			if ((triggerOn & PhysicsEventFlags.OnTriggerExit) > 0)
				Trigger();
		}
	}
}