using UnityEngine;

namespace Augmenta.Combat
{
	[System.Serializable]
	public class DamageInfo
	{
		public Collider colliderHit;

		public float amount;

		public DamageType type;

		public Health receiver;

		public string damageIdentifier;

		public Agent responsibleAgent;

		public DamageInfo(float amount,
		DamageType type,
		Collider colliderHit,
		string damageIdentifier,
		Agent responsibleAgent = null)
		{
			this.colliderHit = colliderHit;
			this.type = type;
			this.receiver = null;
			this.damageIdentifier = damageIdentifier;
			this.amount = amount;
			this.responsibleAgent = responsibleAgent;
		}
	}
}