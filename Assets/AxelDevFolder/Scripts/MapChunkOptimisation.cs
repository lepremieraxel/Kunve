using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunkOptimisation : MonoBehaviour
{
    [SerializeField] private Transform chunksParent;
    [SerializeField] private Transform anchorsParent;
    [SerializeField] private GameObject anchorPrefab;
    [SerializeField] private Transform player;
    public float loadRange;

    private List<GameObject> allChunks = new List<GameObject>();

    private void Start()
    {
        int nbChunks = chunksParent.childCount;
        for (int i = 0; i < nbChunks; i++)
        {
            GameObject currentChunk = chunksParent.GetChild(i).gameObject;
            allChunks.Add(currentChunk);
            GameObject currentAnchor = Instantiate(anchorPrefab, anchorsParent);
            currentAnchor.name = currentChunk.name;
            currentAnchor.transform.position = currentChunk.transform.position;
            currentChunk.SetActive(false);
            DistanceCheck(currentChunk);
        }
    }

    private void Update()
    {
        for(int i = 0; i < allChunks.Count; i++)
        {
            DistanceCheck(allChunks[i]);
        }
    }

    private void DistanceCheck(GameObject chunk)
    {
        if(Vector3.Distance(player.position, chunk.transform.position) < loadRange)
        {
            chunk.SetActive(true);
        } else
        {
            chunk.SetActive(false);
        }
    }
}
