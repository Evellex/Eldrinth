using UnityEngine;
using System.Collections.Generic;

namespace Augmenta
{
	using IIE = IInventoryEvent;
	[AddComponentMenu("Augmenta/Inventory")]
	public class Inventory : MonoBehaviour 
	{
		[SerializeField]
		List<Rigidbody> pickupAreas;
		[SerializeField]
		Transform inventoryStorer;
		[SerializeField]
		Transform inventoryDropPosition;
		[SerializeField]
		bool automaticPickup = false;
		[SerializeField]
		bool inventoryLimit = false;
		[SerializeField]
		int maximumUnits = 5;

		List<InventoryItem> itemList = new List<InventoryItem>();

		void Start()
		{
			if (inventoryStorer == null)
				inventoryStorer = transform;
			foreach (Rigidbody rigidbody in pickupAreas)
			{
				rigidbody.gameObject.AddComponent<InventoryPickupArea>().ConnectInventory(this);
			}
		}

		void OnDestroy()
		{
			foreach (Rigidbody rigidbody in pickupAreas)
			{
				rigidbody.gameObject.GetComponent<InventoryPickupArea>().DisconnectInventory();
			}
		}

		public void ItemEnterPickupArea(InventoryItem target)
		{
			if(automaticPickup)			
				AttemptPickup(target);			
			InvokeOnItemEnterPickupArea(target);			
		}

		public void AttemptPickup(InventoryItem target)
		{
			if (VerifyPickup(target))			
				Pickup(target);			
		}

		public bool VerifyPickup(InventoryItem target)
		{
			if(inventoryLimit)
			{
				if (target.GetUnits() + GetCurrentUnits() > maximumUnits)
					return false;
			}
			if (!target.CanPickup())
				return false;
			return true;
		}

		int GetCurrentUnits()
		{
			int unitsTaken = 0;
			foreach(InventoryItem item in itemList)
			{
				unitsTaken += item.GetUnits();
			}
			return unitsTaken;
		}

		void Pickup(InventoryItem target)
		{
			InvokeOnItemPickup(target);
			target.gameObject.SetActive(false);
			target.transform.parent = inventoryStorer; 
			target.transform.localPosition = Vector3.zero;
			target.SetInventory(this);
			itemList.Add(target);
		}

		void Drop(InventoryItem target)
		{
			if(itemList.Contains(target))
			{
				InvokeOnItemDrop(target);
				itemList.Remove(target);
				target.transform.parent = null;
				target.transform.position = inventoryDropPosition.position;
				target.gameObject.SetActive(true);
			}
		}

		//Event Callers
		IIE[] list;
		void Refresh() { list = this.GetInterfaces<IIE>(); }
		void InvokeOnItemEnterPickupArea(InventoryItem target) { Refresh(); if (list != null) foreach (IIE t in list) { t.OnItemEnterPickupArea(target); } }
		void InvokeOnItemPickup(InventoryItem target) { Refresh(); if (list != null) foreach (IIE t in list) { t.OnItemPickup(target); } }
		void InvokeOnItemDrop(InventoryItem target) { Refresh(); if (list != null) foreach (IIE t in list) { t.OnItemDrop(target); } }
	}
}
