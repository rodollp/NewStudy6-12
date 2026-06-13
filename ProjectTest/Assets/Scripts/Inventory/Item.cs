using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]ItemData itemData;

    public ItemData ItemData => itemData;
}
