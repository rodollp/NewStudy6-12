using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGetItem : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    Item[] items;

    [SerializeField] float pickUpRange = 3f;
    [SerializeField] private PlayerInputHandler input;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }
    private void Update()
    {
        

        if (input.InteractPressed)
        {
            PickUp();
        }
    }

    void PickUp()
    {
        items = FindObjectsByType<Item>(FindObjectsSortMode.None);

        Item closest = null;

        float closestDist = float.MaxValue;

        foreach (Item item in items)
        {
            float dist = (item.transform.position - transform.position).sqrMagnitude;

            if (dist < pickUpRange * pickUpRange && dist < closestDist)
            {
                closestDist = dist;
                closest = item;
            }
        }

        if (closest == null)
        {
            return;
        }

        inventory.AddItem(closest.ItemData);

        Destroy(closest.gameObject);
    }
}