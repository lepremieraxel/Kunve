using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

[CreateAssetMenu(fileName = "NPCData", menuName = "Kunve/NPCs/New NPC")]
public class NPCData : ScriptableObject
{
    public enum NPCType
    {
        Worker,
        Explorer
    }

    [Header("Renderer")]
    public Mesh mesh;
    public Material material;

    [Header("Data")]
    new public string name;
    public string personality;
    public NPCType type;

    [Header("Dialogue")]
    [TextArea] public string firstInteractionDialogue;
    [TextArea] public string notYetTradedDialogue;
    [TextArea] public string tradedDialogue;

    [Header("Trade")]
    public ItemData[] demand;
    public ItemData[] offer;

    [Header("Production")]
    public ItemData[] productionItems;
}
