using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            inventory.UseItem(ItemType.Potion);
        }
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            inventory.UseItem(ItemType.Weapon);
        }
    }
}
