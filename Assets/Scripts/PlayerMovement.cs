using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float moveSpeed;
    [SerializeField] PlayerCamera playerCamera;
    InputMaster controls;
    Rigidbody rb;
    Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputMaster();
        controls.Player.Fire.performed += ctx => TestInput(ctx);
    }

    void Update()
    {
        moveDirection = controls.Player.Move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 forwardVector = playerCamera.ForwardVector;
        Vector3 rightVector = playerCamera.RightVector;
        Debug.DrawRay(transform.position, forwardVector * 10, Color.red);

        Vector3 moveVector = Vector3.ProjectOnPlane
        (forwardVector * moveDirection.y * moveSpeed + rightVector * moveDirection.x * moveSpeed, Vector3.up);
        rb.MovePosition(transform.position + moveVector);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void TestInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        Debug.Log("CLick!");
    }
}
