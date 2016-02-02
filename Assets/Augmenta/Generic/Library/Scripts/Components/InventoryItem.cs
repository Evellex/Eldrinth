using UnityEngine;

namespace Augmenta
{
    [AddComponentMenu("Augmenta/Inventory Item")]
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField]
        int units = 1;  
      
        Inventory currentInventory = null;

        #if UNITY_EDITOR
        void OnValidate()
        {
            ClampUnits();
        }
        #endif

        void ClampUnits()
        {
            units = Mathf.Max(units, 0);
        } 

        public int GetUnits()
        {
            ClampUnits();
            return units;
        }

        public bool CanPickup()
        {
            return (currentInventory == null);
        }

        public void SetInventory(Inventory newInventory)
        {
            currentInventory = newInventory;
        }
    }
}    