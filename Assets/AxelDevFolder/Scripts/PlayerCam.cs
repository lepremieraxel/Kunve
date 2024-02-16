using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by Dave / GameDevelopment on YouTube

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    private Vector2 mouseDelta;

    private float xRotation;
    private float yRotation;

    private InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.Orientation.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        // controls.Player.Orientation.performed += ctx => Debug.Log(mouseDelta);
        controls.Player.Orientation.canceled += ctx => mouseDelta = Vector2.zero;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        float mouseY = mouseDelta.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
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
