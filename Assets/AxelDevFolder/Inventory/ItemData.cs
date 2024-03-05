using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

[CreateAssetMenu(fileName = "ItemData", menuName = "Kunve/New item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite visual;
}
