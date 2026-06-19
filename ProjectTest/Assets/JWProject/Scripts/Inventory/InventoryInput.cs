using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] Inventory inventory;


    private void Awake()
    {
        if(input==null)input = GetComponent<PlayerInputHandler>();
    }
    private void Update()
    {
        

        if (input.UsePotionPressed)
        {
            inventory.UseItem(ItemType.Potion);
        }
        if(input.UseWeaponPressed)
        {
            inventory.UseItem(ItemType.Weapon);
        }
    }
}
