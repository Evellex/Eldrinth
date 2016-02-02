namespace Augmenta
{
	public interface IInventoryEvent
	{
		void OnItemEnterPickupArea(InventoryItem target);
		void OnItemPickup(InventoryItem target);
		void OnItemDrop(InventoryItem target);
	}
}
