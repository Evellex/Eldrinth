using System.Collections;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Collider Event Receiver")]
	public class ColliderEventReceiver : MonoBehaviour
	{
		public event System.Action<Collision> On_OnCollisionEnter;

		public event System.Action<Collision> On_OnCollisionExit;

		public event System.Action<Collision> On_OnCollisionStay;

		public event System.Action<Collider> On_OnTriggerEnter;

		public event System.Action<Collider> On_OnTriggerExit;

		public event System.Action<Collider> On_OnTriggerStay;

		private void OnCollisionEnter(Collision collision)
		{
			if (On_OnCollisionEnter != null)
				On_OnCollisionEnter.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			if (On_OnCollisionExit != null)
				On_OnCollisionExit.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			if (On_OnCollisionStay != null)
				On_OnCollisionStay.Invoke(collision);
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (On_OnTriggerEnter != null)
				On_OnTriggerEnter.Invoke(collider);
		}

		private void OnTriggerExit(Collider collider)
		{
			if (On_OnTriggerExit != null)
				On_OnTriggerExit.Invoke(collider);
		}

		private void OnTriggerStay(Collider collider)
		{
			if (On_OnTriggerStay != null)
				On_OnTriggerStay.Invoke(collider);
		}
	}
}