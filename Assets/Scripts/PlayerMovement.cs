using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float moveSpeed = 0.2f;
    [Tooltip("How much space should be kept between the collider and the environment")]
    [SerializeField] [Range(0, 0.08f)] float edgeWidth = 0.04f;
    [SerializeField] PlayerCamera playerCamera = null;
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
        //moveDirection.y = -1;
    }

    void FixedUpdate()
    {
        // Get direction vectors from camera
        Vector3 forwardVector = playerCamera.ForwardVector;
        Vector3 rightVector = playerCamera.RightVector;


        // Calculate movementVector
        Vector3 moveVector = Vector3.ProjectOnPlane
        (forwardVector * moveDirection.y * moveSpeed + rightVector * moveDirection.x * moveSpeed, Vector3.up);

        // Detect if the rigidbody will collide with anything
        Vector3 CapsuleP1 = new Vector3(rb.position.x, rb.position.y + 0.5f, rb.position.z);
        Vector3 CapsuleP2 = new Vector3(rb.position.x, rb.position.y - 0.3f, rb.position.z);
        Vector3 sweepDistance = new Vector3();

        RaycastHit sweepHitX;
        if (Physics.CapsuleCast(CapsuleP1, CapsuleP2, 0.5f, new Vector3(moveVector.x, 0, 0), out sweepHitX, Mathf.Abs(moveVector.x)))
        {
            sweepDistance.x = sweepHitX.point.x - transform.position.x >= 0 ? sweepHitX.distance : -sweepHitX.distance;

            // Re-calculate movementVector due to detected collision in the sweep test
            moveVector.x = Mathf.Clamp(sweepDistance.x - edgeWidth, 0, float.MaxValue);
        }

        RaycastHit sweepHitZ;
        if (Physics.CapsuleCast(CapsuleP1, CapsuleP2, 0.5f, new Vector3(0, 0, moveVector.z), out sweepHitZ, Mathf.Abs(moveVector.z)))
        {
            sweepDistance.z = sweepHitZ.point.z - transform.position.z >= 0 ? sweepHitZ.distance : -sweepHitZ.distance;

            // Re-calculate movementVector due to detected collision in the sweep test
            moveVector.z = Mathf.Clamp(sweepDistance.z - edgeWidth, 0, float.MaxValue);
        }

        //RaycastHit sweepHitY;
        //Vector3 sphere = new Vector3(rb.position.x, rb.position.y - 0.5f, rb.position.z);
        //if (Physics.SphereCast(sphere, 0.5f, Vector3.down, ))
        //{

        //}

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
