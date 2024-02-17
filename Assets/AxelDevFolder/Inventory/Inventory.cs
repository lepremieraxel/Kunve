using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> content = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        content.Add(item);
    }
}
