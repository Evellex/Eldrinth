using System.Collections.Generic;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("Augmenta/Combat/Damage Modifier")]
	public class DamageModifier : MonoBehaviour
	{
		[SerializeField]
		[EnumMask]
		private DamageType damageToModify = 0;

		[SerializeField]
		private Modifier values;

		public static float ApplyDamageModifiers(float damageInput, DamageType damageType, List<DamageModifier> damageModifiers)
		{
			List<Modifier> modifiers = new List<Modifier>();
			damageModifiers.ForEach((p) => { modifiers.Add(p.GetModifier()); });
			return Modifier.ApplyModifiers(damageInput, modifiers);
		}

		public void SetModifier(Modifier newModifier)
		{
			values = newModifier;
		}

		public Modifier GetModifier()
		{
			return values;
		}

		public void SetDamageType(DamageType type)
		{
			damageToModify = type;
		}

		public void AddDamageType(DamageType type)
		{
			damageToModify |= type;
		}

		private void Start()
		{
			Health healthScript = GetComponent<Health>();
			if (healthScript != null)
				healthScript.ConnectDamageModifier(this);
		}

		private void OnDestroy()
		{
			Health healthScript = GetComponent<Health>();
			if (healthScript != null)
				healthScript.DisconnectDamageModifier(this);
		}
	}
}