using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

[CreateAssetMenu(fileName = "NPCData", menuName = "Kunve/New NPC")]
public class NPCData : ScriptableObject
{
    public enum NPCType
    {
        Worker,
        Explorer
    }
    public enum NPCNationality
    {
        Sensolo,
        Kunsolo
    }

    [Header("Renderer")]
    public Mesh mesh;
    public Material material;

    [Header("Data")]
    new public string name;
    public string personality;
    public NPCType type;
    public NPCNationality nationality;

    [Header("Dialogue")]
    [TextArea] public string firstInteractionDialogue;
    [TextArea] public string notYetTradedDialogue;
    [TextArea] public string tradedDialogue;

    [Header("Trade")]
    public List<ItemData> demand;
    public List<ItemData> offer;

    [Header("Production")]
    public List<ItemData> productionItems;
}
