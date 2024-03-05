using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCData npcData;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public CurrentState state;
    public enum CurrentState
    {
        NoInteraction,
        FirstInteraction,
        NoTraded,
        Traded,
        Kunve
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        meshFilter.mesh = npcData.mesh;
        meshRenderer.material = npcData.material;
    }

    public CurrentState GetState()
    {
        return state;
    }
    public void SetState(CurrentState newState)
    {
        state = newState;
    }
}
