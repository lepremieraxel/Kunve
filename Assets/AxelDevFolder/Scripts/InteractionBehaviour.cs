using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

// inspired by TutoUnityFR on Youtube

public class InteractionBehaviour : MonoBehaviour
{
    [SerializeField] private float interactionRange = 5f;
    private InputMaster controls;
    private bool isClicked = false;
    public PickupBehaviour playerPickupBehaviour;
    public PlayerBehaviour player;
    [SerializeField] private DayNightCycle dayNightCycle;
    private bool isInteractable = true;
    public GameObject hitObject;
    public RaycastHit hit;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [SerializeField] private ButtonDisplayBehaviour buttonDisplayBehaviour;


    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.Interact.performed += ctx => isClicked = true;
        controls.Player.Interact.canceled += ctx => isClicked = false;
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);


        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            hitObject = hit.transform.gameObject;

            if (hit.transform.CompareTag("Item") && hit.distance < 3)
            {
                // Afficher dans l'UI
                Debug.Log(player.GetInputButton("Interact").ToUpper() + " Ramasser");
                if (isClicked)
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
            if (hit.transform.CompareTag("NightCamp") && hit.distance < 3)
            {
                if(!player.haveNightcamp)
                {
                    if(isInteractable)
                    {
                        if(dayNightCycle.timeOfDay >= dayNightCycle.dayHour && dayNightCycle.timeOfDay <= dayNightCycle.nightHour)
                        {
                            buttonDisplayBehaviour.DisplayButton("Interact", "Ramasser");
                        }
                        else
                        {
                            buttonDisplayBehaviour.DisplayButton("Interact", "Dormir");
                        }
                        isInteractable = false;
                    }
                }
            }
            if (hit.transform.CompareTag("Map"))
            {
                if (!dayNightCycle.isDay && player.haveNightcamp && player.CanPlaceNightcamp(hit) && isInteractable)
                {
                    buttonDisplayBehaviour.DisplayButton("Interact", "Feu de camp");
                    isInteractable = false;
                }
            }
            if (hit.transform.CompareTag("NPC") && hit.distance < 3)
            {
                NPC hitNPC = hitObject.GetComponent<NPC>();
                
                switch (hitNPC.state)
                {
                    case NPC.CurrentState.NoInteraction:
                        if (isInteractable)
                        {
                            buttonDisplayBehaviour.DisplayButton("Interact", "Echanger");
                            isInteractable = false;
                            dialoguePanel.SetActive(true);
                            dialogueText.text = hitNPC.npcData.firstInteractionDialogue.ToString();
                            hitNPC.SetState(NPC.CurrentState.FirstInteraction);
                        }
                        break;
                    case NPC.CurrentState.FirstInteraction:
                        if (isInteractable)
                        {
                            buttonDisplayBehaviour.DisplayButton("Interact", "Echanger");
                            isInteractable = false;
                            dialoguePanel.SetActive(true);
                            dialogueText.text = hitNPC.npcData.notYetTradedDialogue.ToString();
                            hitNPC.SetState(NPC.CurrentState.NoTraded);
                        }
                        break;
                    case NPC.CurrentState.NoTraded:
                        if (isInteractable)
                        {
                            buttonDisplayBehaviour.DisplayButton("Interact", "Echanger");
                            isInteractable = false;
                            dialoguePanel.SetActive(true);
                            dialogueText.text = hitNPC.npcData.notYetTradedDialogue.ToString();
                            hitNPC.SetState(NPC.CurrentState.NoTraded);
                        }
                        break;
                    case NPC.CurrentState.Traded:
                        if (isInteractable)
                        {
                            isInteractable = false;
                            hitNPC.SetState(NPC.CurrentState.Kunve);
                            // Go to Kunve
                        }
                        break;
                }
            }
        } else
        {
            isInteractable = true;
            dialogueText.text = "";
            dialoguePanel.SetActive(false);
            buttonDisplayBehaviour.UndisplayButton("Interact");
            hitObject = null;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Item"))
    //    {
    //        // Afficher dans l'UI
    //        Debug.Log(player.GetInputButton("Interact").ToUpper() + " Ramasser");
    //        if (isClicked)
    //        {
    //            playerPickupBehaviour.DoPickup(other.gameObject.GetComponent<Item>());
    //        }
    //    }
    //    if (other.CompareTag("NPC"))
    //    {
    //        // Afficher dans l'UI
    //        Debug.Log(player.GetInputButton("Interact").ToUpper() + " Echanger");
    //        if (isClicked)
    //        {
    //        }
    //    }
    //}

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
