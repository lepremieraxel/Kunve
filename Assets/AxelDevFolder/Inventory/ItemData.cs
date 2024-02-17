using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

[CreateAssetMenu(fileName = "ItemData", menuName = "Kunve/Items/New item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite visual;
    public GameObject prefab;
}
