using UnityEngine;

namespace Augmenta
{
	public static class ColliderExt
	{
		public static Component GetPhysicsRoot(this Collider collider)
		{
			Rigidbody rigidbodyTarget = null;
			rigidbodyTarget = collider.attachedRigidbody;
			if(rigidbodyTarget != null)
				return rigidbodyTarget;
			else
				return collider;
		}
	}
}
