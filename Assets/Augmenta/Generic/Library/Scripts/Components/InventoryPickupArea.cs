using UnityEngine;

namespace Augmenta
{
    [AddComponentMenu("")]
    public class InventoryPickupArea : MonoBehaviour
    {        
        Inventory connectedInventory = null;

        void OnTriggerEnter(Collider other)
        {
            InventoryItem itemComp = other.GetComponent<InventoryItem>();
            if (itemComp != null)
            {
                connectedInventory.ItemEnterPickupArea(itemComp);
            }
        }

        public void ConnectInventory(Inventory newConnection)
        {
            if (connectedInventory == null)
                connectedInventory = newConnection;
        }

        public void DisconnectInventory()
        {
            connectedInventory = null;
        }
    }
}
