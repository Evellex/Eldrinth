using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Augmenta.Combat
{
	using IHE = IHealthEvent;

	[AddComponentMenu("Augmenta/Combat/Health")]
	public class Health : MonoBehaviour
	{
		[SerializeField]
		private float maximumHealth = 100.0f;

		[SerializeField]
		private float initialHealth = 100.0f;

		[SerializeField]
		[HideInInspector]
		private float currentHealth = 100.0f;

		[SerializeField]
		private Agent responsibleAgent = null;

		[SerializeField]
		private List<Rigidbody> damageReceivers = new List<Rigidbody>();

		private List<DamageReceiver> damageReceiverConnections = new List<DamageReceiver>();

		[SerializeField]
		private List<DamageModifier> damageModifiers = new List<DamageModifier>();

		[SerializeField]
		private FactionMember factionController;

		private List<DamageInfo> damageHistory = new List<DamageInfo>();
		private List<DamageInfo> healHistory = new List<DamageInfo>();
		private List<DamageInfo> changeHistory = new List<DamageInfo>();

		private void OnEnable()
		{
			ResetHealth();
		}

		public void Reinitialise(float newMaximumHealth, float newInitialHealth)
		{
			maximumHealth = newMaximumHealth;
			currentHealth = initialHealth = newInitialHealth;
			damageHistory.Clear();
			healHistory.Clear();
			changeHistory.Clear();
		}

		public void ResetHealth()
		{
			Reinitialise(maximumHealth, initialHealth);
		}

		private void Start()
		{
			foreach (Rigidbody rigidbody in damageReceivers)
			{
				DamageReceiver script = rigidbody.gameObject.GetComponent<DamageReceiver>();
				if (script == null)
					script = rigidbody.gameObject.AddComponent<DamageReceiver>();
				script.ConnectHealth(this);
				damageReceiverConnections.Add(script);
			}
		}

		private void OnDestroy()
		{
			foreach (Rigidbody rigidbody in damageReceivers)
			{
				DamageReceiver script = rigidbody.gameObject.GetComponent<DamageReceiver>();
				script.DisconnectHealth(this);
				damageReceiverConnections.Remove(script);
			}
		}

		public float GetHealth()
		{
			return currentHealth;
		}

		public void ConnectDamageModifier(DamageModifier newModifier)
		{
			damageModifiers.Add(newModifier);
		}

		public void DisconnectDamageModifier(DamageModifier existingModifier)
		{
			damageModifiers.Remove(existingModifier);
		}

		public void ReceiveHandshakeRequest(Damage source, DamageReceiver recevier)
		{
			if (damageReceiverConnections.Contains(recevier))
			{
				if (gameObject.activeInHierarchy)
					source.AcknowledgeHandshake(this);
			}
			else
				recevier.DisconnectHealth(this);
		}

		public Faction GetFactions()
		{
			if (factionController != null)
				return factionController.GetFactions();
			return (Faction)0;
		}

		public void ApplyDamage(DamageInfo info)
		{
			damageModifiers.RemoveAll((p) => { return p == null; });
			info.amount = DamageModifier.ApplyDamageModifiers(info.amount, info.type, damageModifiers);

			currentHealth = Mathf.Clamp(currentHealth - info.amount, 0, maximumHealth);

			if (info.amount > 0)
			{
				changeHistory.Add(info);
				damageHistory.Add(info);
				InvokeOnDamage(info);
			}
			if (info.amount < 0)
			{
				changeHistory.Add(info);
				healHistory.Add(info);
				InvokeOnHeal(info);
			}
			if (currentHealth == 0)
			{
				InvokeOnDeath(info);
				if (info.responsibleAgent != null && responsibleAgent != null)
					Console.PrintText(info.responsibleAgent.GetName() + "(ID: " + info.responsibleAgent.GetID() + ") killed " + responsibleAgent.GetName() + "(ID: " + responsibleAgent.GetID() + ") with " + info.damageIdentifier);
			}
		}

		private ReadOnlyCollection<DamageInfo> GetDamageHistory()
		{
			return damageHistory.AsReadOnly();
		}

		private ReadOnlyCollection<DamageInfo> GetHealHistory()
		{
			return healHistory.AsReadOnly();
		}

		private ReadOnlyCollection<DamageInfo> GetChangeHistory()
		{
			return changeHistory.AsReadOnly();
		}

		//Event Callers
		private IEnumerable<IHE> list;

		private void Refresh()
		{ list = this.GetInterfaces<IHE>(); }

		private void InvokeOnDamage(DamageInfo info)
		{ Refresh(); if (list != null) foreach (IHE t in list) { t.OnDamageReceived(info); } }

		private void InvokeOnDeath(DamageInfo info)
		{ Refresh(); if (list != null) foreach (IHE t in list) { t.OnDeath(info); } }

		private void InvokeOnHeal(DamageInfo info)
		{ Refresh(); if (list != null) foreach (IHE t in list) { t.OnHealReceived(info); } }

#if UNITY_EDITOR

		private void OnValidate()
		{
			maximumHealth = Mathf.Max(maximumHealth, 0);
			initialHealth = Mathf.Clamp(initialHealth, 0, maximumHealth);
			currentHealth = initialHealth;
		}

#endif
	}
}