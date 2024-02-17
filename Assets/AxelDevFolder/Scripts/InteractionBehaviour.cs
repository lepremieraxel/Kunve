using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

public class InteractionBehaviour : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2.6f;
    private InputMaster controls;
    private bool isClicked = false;
    public PickupBehaviour playerPickupBehaviour;

    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.Interact.performed += ctx => isClicked = true;
        controls.Player.Interact.canceled += ctx => isClicked = false;
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if(hit.transform.CompareTag("Item"))
            {
                if(isClicked)
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
            if (hit.transform.CompareTag("NPC"))
            {
                if (isClicked)
                {
                    
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            if (isClicked)
            {
                playerPickupBehaviour.DoPickup(other.gameObject.GetComponent<Item>());
            }
        }
        if (other.CompareTag("NPC"))
        {
            if (isClicked)
            {
            }
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
