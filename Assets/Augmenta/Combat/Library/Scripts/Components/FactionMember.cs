using System.Collections;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("Augmenta/Combat/Faction Member")]
	public class FactionMember : MonoBehaviour
	{
		[SerializeField]
		[EnumMask]
		private Faction currentFactions = (Faction)0;

		public void SetFactions(Faction newFactions)
		{
			currentFactions = newFactions;
		}

		public void AddFactions(Faction newFactions)
		{
			currentFactions |= newFactions;
		}

		public void RemoveFactions(Faction removedFactions)
		{
			currentFactions &= ~removedFactions;
		}

		public Faction GetFactions()
		{
			return currentFactions;
		}

		public bool IsPartOfFaction(Faction queryFaction)
		{
			return (queryFaction & currentFactions) != 0;
		}
	}
}