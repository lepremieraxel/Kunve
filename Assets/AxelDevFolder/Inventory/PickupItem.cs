using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by TutoUnityFR on Youtube

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float pickupRange = 2.6f;
    private InputMaster controls;
    private bool isClicked = false;
    public PickupBehaviour playerPickupBehaviour;

    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.PickUp.performed += ctx => isClicked = true;
        controls.Player.PickUp.canceled += ctx => isClicked = false;
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if(hit.transform.CompareTag("Item"))
            {
                if(isClicked)
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
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
