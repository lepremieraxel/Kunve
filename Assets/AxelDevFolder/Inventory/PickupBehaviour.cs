using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    public void DoPickup(Item item)
    {
        inventory.AddItem(item.itemData);
        Destroy(item.gameObject);
    }
}
