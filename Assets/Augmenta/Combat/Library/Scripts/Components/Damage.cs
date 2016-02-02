using System.Collections.Generic;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("Augmenta/Combat/Damage")]
	public class Damage : MonoBehaviour
	{
		[SerializeField]
		[EnumMask]
		private DamageType damageType = (DamageType)0;

		[SerializeField]
		private float damageToApply = 0;

		[SerializeField]
		private FriendlyFire friendlyFire = FriendlyFire.Default;

		[SerializeField]
		private FactionMember sourceFaction = null;

		[SerializeField]
		private string damageIdentifier;

		[SerializeField]
		private Agent responsibleAgent;

		[SerializeField]
		private List<DamageModifier> damageModifiers = new List<DamageModifier>();

		[SerializeField]
		private bool physical = false;

		[SerializeField]
		private bool continuous = false;

		[SerializeField]
		private float repetitionDelay = 0.5f;

		[SerializeField]
		[EnumMask]
		private PhysicsTypeFlags triggerType = (PhysicsTypeFlags)0;

		private DamageInfo interimDamageInfo;

		private float lastDamageTime = 0;
		public float DamageToApply { get { return damageToApply; } set { damageToApply = value; } }

		public string DamageIdentifier
		{
			get { return damageIdentifier; }
		}

		public void DoDamage(Collider target)
		{
			AttemptDamage(target);
		}

		public void SetSourceFaction(FactionMember sourceFaction)
		{
			this.sourceFaction = sourceFaction;
		}

		public void SetDamageType(DamageType newDamageType)
		{
			this.damageType = newDamageType;
		}

		public void SetFriendlyFire(FriendlyFire friendlyFire)
		{
			this.friendlyFire = friendlyFire;
		}

		public void ConnectDamageModifier(DamageModifier newModifier)
		{
			damageModifiers.Add(newModifier);
		}

		public void DisconnectDamageModifier(DamageModifier existingModifier)
		{
			damageModifiers.Remove(existingModifier);
		}

		public void AcknowledgeHandshake(Health receiver)
		{
			interimDamageInfo.receiver = receiver;
			//Here we sort out damage
			bool friendlyFireOn = false;
			switch (friendlyFire)
			{
				case FriendlyFire.OverrideOff:
					friendlyFireOn = false;
					break;

				case FriendlyFire.OverrideOn:
					friendlyFireOn = true;
					break;

				case FriendlyFire.Default:
				default:
					friendlyFireOn = CombatSettings.friendlyFire;
					break;
			}
			bool friendlyFaction = sourceFaction != null && ((sourceFaction.GetFactions() & receiver.GetFactions()) != 0);
			if (friendlyFaction && !friendlyFireOn)
				return;

			IDamageEvent[] list = this.GetInterfaces<IDamageEvent>();
			if (interimDamageInfo.amount > 0)
				foreach (IDamageEvent c in list) { c.OnDamageSent(interimDamageInfo); }
			else
				foreach (IDamageEvent c in list) { c.OnHealSent(interimDamageInfo); }
			receiver.ApplyDamage(interimDamageInfo);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (physical && !continuous && IsType(PhysicsTypeFlags.Trigger))
				AttemptDamage(other);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (physical && !continuous && IsType(PhysicsTypeFlags.Collision))
				AttemptDamage(collision.collider);
		}

		private void OnTriggerStay(Collider other)
		{
			if (physical && continuous && Time.time > lastDamageTime + repetitionDelay && IsType(PhysicsTypeFlags.Trigger))
			{
				lastDamageTime = Time.time;
				AttemptDamage(other);
			}
		}

		private void OnCollisionStay(Collision collision)
		{
			if (physical && continuous && Time.time > lastDamageTime + repetitionDelay && IsType(PhysicsTypeFlags.Collision))
			{
				lastDamageTime = Time.time;
				AttemptDamage(collision.collider);
			}
		}

		private bool IsType(PhysicsTypeFlags flags)
		{
			return (triggerType & flags) > 0;
		}

		private void AttemptDamage(Collider target)
		{
			interimDamageInfo = BuildDamageInfo(target);
			IEnumerable<DamageReceiver> list = target.GetPhysicsRoot().GetComponents<DamageReceiver>();
			foreach (DamageReceiver c in list) { c.RequestHandshake(this); }
		}

		private DamageInfo BuildDamageInfo(Collider target)
		{
			damageModifiers.RemoveAll((p) => { return p == null; });
			float filteredDamageToApply = DamageModifier.ApplyDamageModifiers(damageToApply, damageType, damageModifiers);
			return new DamageInfo(filteredDamageToApply, damageType, target, damageIdentifier, responsibleAgent);
		}
	}
}