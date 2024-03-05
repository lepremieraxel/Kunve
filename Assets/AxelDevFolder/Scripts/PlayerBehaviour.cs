using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// movement and camera inspired by Dave / GameDevelopment on YouTube

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    [SerializeField] private float dotProductToPlaceObject = 0.995f;
    public Transform orientation;

    private Vector2 moveInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private InputMaster controls;

    public PlayerInput playerInput;

    public GameObject inventoryPanel;

    public bool haveNightcamp;
    [SerializeField] private GameObject nightcampPrefab;

    [SerializeField] private Inventory inventory;
    [SerializeField] private InteractionBehaviour interactionBehaviour;
    [SerializeField] private ButtonDisplayBehaviour buttonDisplayBehaviour;
    [SerializeField] private DayNightCycle dayNightCycle;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    public GameMode gameMode;
    public enum GameMode
    {
        play,
        ui
    }
    public string currentControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveSpeed = walkSpeed;

        controls = new InputMaster();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Sprint.performed += ctx => moveSpeed = sprintSpeed;
        controls.Player.Sprint.canceled += ctx => moveSpeed = walkSpeed;
    }

    private void Start()
    {
        rb.freezeRotation = true;
        readyToJump = true;
        haveNightcamp = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        currentControls = playerInput.currentControlScheme;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        SpeedControl();

        if(grounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0;
        }

        switch(gameMode)
        {
            case GameMode.play:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameMode.ui:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            default: break;
        }
    }

    private void FixedUpdate()
    {
        if(gameMode == GameMode.play)
        {
            MovePlayer();
        }
    }

    void OnJump()
    {
        Jump();
    }
    void OnInventory()
    {
        inventoryPanel.GetComponent<Animator>().SetBool("isOpen", !inventoryPanel.GetComponent<Animator>().GetBool("isOpen"));
    }
    void OnInteract()
    {
        if(interactionBehaviour.hitObject != null)
        {
            if (interactionBehaviour.hitObject.transform.CompareTag("NPC"))
            {
                NPC hitNPC = interactionBehaviour.hitObject.GetComponent<NPC>();
                if(hitNPC.GetState() != NPC.CurrentState.Traded &&hitNPC.GetState() != NPC.CurrentState.Kunve)
                {
                    bool tradeState = CanTrade(hitNPC.npcData); // retourne true si le trade peut être effectué
                    if (tradeState)
                    {
                        Trade(hitNPC.npcData); // effectue le trade
                        hitNPC.SetState(NPC.CurrentState.Traded);
                        buttonDisplayBehaviour.UndisplayButton("Interact");
                        interactionBehaviour.dialogueText.text = hitNPC.npcData.tradedDialogue.ToString();
                    }
                    else
                    {
                        Debug.Log("Not Traded");
                    }
                }
            }
            if (interactionBehaviour.hitObject.transform.CompareTag("NightCamp"))
            {
                if(!haveNightcamp)
                {
                    if (dayNightCycle.isDay)
                    {
                        PickupNightcamp(interactionBehaviour.hitObject.transform.parent.gameObject);
                        buttonDisplayBehaviour.UndisplayButton("Interact");
                    }
                    else
                    {
                        Sleep();
                        buttonDisplayBehaviour.UndisplayButton("Interact");
                        buttonDisplayBehaviour.DisplayButton("Interact", "Ramasser");
                    }
                }
            }
            if (interactionBehaviour.hitObject.transform.CompareTag("Map"))
            {
                if(!dayNightCycle.isDay && haveNightcamp && CanPlaceNightcamp(interactionBehaviour.hit))
                {
                    PlaceNightcamp(interactionBehaviour.hit);
                    buttonDisplayBehaviour.UndisplayButton("Interact");
                }
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            exitingSlope = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    void PickupNightcamp(GameObject nightCamp)
    {
        if (nightCamp.transform.CompareTag("NightCamp"))
        {
            Destroy(nightCamp);
            haveNightcamp = true;
        }
    }

    void Sleep()
    {
        dayNightCycle.timeOfDay = dayNightCycle.dayHour;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public string GetInputButton(string buttonName)
    {
        string inputButton = playerInput.actions[buttonName].ToString();
        string[] interactButtonArray = inputButton.Split("/");
        inputButton = interactButtonArray.Last();
        interactButtonArray = inputButton.Split("]");
        return interactButtonArray.First();
    }

    public Sprite GetInputButtonImage(string buttonName)
    {
        string inputButton = playerInput.actions[buttonName].ToString();
        string[] interactButtonArray = inputButton.Split("/");
        inputButton = interactButtonArray.Last();
        interactButtonArray = inputButton.Split("]");
        inputButton = interactButtonArray.First();
        Sprite image = Resources.Load<Sprite>(inputButton);
        return image;
    }

    public bool CanTrade(NPCData npc)
    {
        // compte le nombre d'occurence de chaque item dans la liste demand
        List<int> demandCountById = new List<int>();
        for(int i = 0; i < inventory.authorizedItems.Count; i++)
        {
            int currentCount = 0;
            foreach(ItemData item in npc.demand)
            {
                if(item.id == i)
                {
                    currentCount++;
                }
            }
            demandCountById.Add(currentCount);
        }

        // compte le nombre d'occurence de chaque item dans la liste inventory
        List<int> inventoryCountById = new List<int>();
        for (int i = 0; i < inventory.authorizedItems.Count; i++)
        {
            int currentCount = 0;
            foreach (ItemData item in inventory.content)
            {
                if (item.id == i)
                {
                    currentCount++;
                }
            }
            inventoryCountById.Add(currentCount);
        }

        List<bool> checkCount = new List<bool>();
        for(int i = 0; i < inventory.authorizedItems.Count; i++)
        {
            if (inventoryCountById[i] >= demandCountById[i])
            {
                checkCount.Add(true);
            }
            else
            {
                checkCount.Add(false);
            }
        }

        return !checkCount.Contains(false);
    }

    void Trade(NPCData npc)
    {
        foreach (ItemData item in npc.demand)
        {
            inventory.content.Remove(item);
        }
        foreach (ItemData item in npc.offer)
        {
            inventory.content.Add(item);
        }
    }

    public bool CanPlaceNightcamp(RaycastHit hit)
    {
        if(Vector3.Dot(hit.normal, Vector3.up) > dotProductToPlaceObject)
        {
            return true;
        }
        return false;
    }
    void PlaceNightcamp(RaycastHit hit)
    {
        Instantiate(nightcampPrefab, hit.point, orientation.rotation);
        haveNightcamp = false;
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
