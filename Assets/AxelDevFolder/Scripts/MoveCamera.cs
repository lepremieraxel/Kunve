using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by Dave / GameDevelopment on YouTube

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
