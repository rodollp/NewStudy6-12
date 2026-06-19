using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Potion,
    Weapon
}
[System.Serializable]
public class ItemData
{
    public string ItemName;
    public ItemType Type;
    public int Value;
    

}
