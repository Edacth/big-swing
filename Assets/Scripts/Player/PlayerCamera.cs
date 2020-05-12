using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] bool smooth = false;
    [SerializeField] float smoothTime = 5f;
    [SerializeField] [Range(0, 1)] float XSensitivity = 0.5f;
    [SerializeField] [Range(0, 1)] float YSensitivity = 0.5f;
    [SerializeField] GameObject cameraGimble = null;
    [SerializeField] new Camera camera = null;
    InputMaster controls;

    public float MinimumX = -90F;
    public float MaximumX = 90F;

    // Camera Gimble Rotation
    private Vector3 gimbleRot;
    private Vector3 gimbleTargRot;

    bool cameraLocked;
    Vector2 cachedMouseDelta;

    public Vector3 ForwardVector
    {
        get
        {
            return camera.transform.forward;
        }
    }
    public Vector3 RightVector
    {
        get
        {
            return camera.transform.right;
        }
    }
    public Vector3 GimbleRotation
    {
        get
        {
            return gimbleRot;
        }
    }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.LockCamera.performed += ctx => LockCamera(ctx);
        controls.Player.UnlockCamera.performed += ctx => UnlockCamera(ctx);


        if (cameraGimble == null) { Debug.LogError("cameraGimble is null", this); }
        gimbleRot = cameraGimble.transform.localRotation.eulerAngles;
        gimbleTargRot = gimbleRot;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!cameraLocked)
        {
            Vector2 mouseDelta = (controls.Player.Look.ReadValue<Vector2>());
            cachedMouseDelta += mouseDelta;
        }  
    }

    private void FixedUpdate()
    {
        gimbleTargRot += new Vector3(-cachedMouseDelta.y * YSensitivity, cachedMouseDelta.x * XSensitivity, 0);
        cachedMouseDelta = Vector2.zero;
        gimbleTargRot.x = Mathf.Clamp(gimbleTargRot.x, MinimumX, MaximumX);

        if (smooth)
        {
            gimbleRot.x = Mathf.LerpAngle(gimbleRot.x, gimbleTargRot.x, smoothTime * Time.deltaTime);
            gimbleRot.y = gimbleTargRot.y;
            cameraGimble.transform.localEulerAngles = gimbleRot;
        }
        else
        {
            gimbleRot = gimbleTargRot;
            cameraGimble.transform.eulerAngles = gimbleRot;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void LockCamera(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        cameraLocked = true;
    }

    private void UnlockCamera(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        cameraLocked = false;
    }
}