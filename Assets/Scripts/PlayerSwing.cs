using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwing : MonoBehaviour
{
    [SerializeField] Transform weaponBase = null;
    [SerializeField] Transform weaponObject = null;
    [SerializeField] Vector2 boundingBox = new Vector2(2f, 2f);
    [Tooltip("How far in front of the character the weapon box should be")]
    [SerializeField] [Range(0,2)] float boxForwardDist = 0.7f;
    [SerializeField] [Range(0, 0.02f)] float mouseSensitivity = 0.01f;
    [SerializeField] BoundingBoxUI boundingBoxUI = null;
    InputMaster controls;

    Vector2 weaponPos;
    Vector3[] globalBoxCorners;
    bool cameraLocked;
    Vector2 cachedMouseDelta;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.LockCamera.performed += ctx => LockCamera(ctx);
        controls.Player.UnlockCamera.performed += ctx => UnlockCamera(ctx);
    }

    void Start()
    {
        weaponPos = new Vector2(0, 0);
        globalBoxCorners = new Vector3[4];
    }

    void Update()
    {
        if (cameraLocked)
        {
            Vector2 mouseDelta = (controls.Player.Look.ReadValue<Vector2>());
            cachedMouseDelta += mouseDelta;
        }
    }

    private void FixedUpdate()
    {
        #region Bounding Box Corners
        globalBoxCorners[0] = weaponBase.position + weaponBase.up * (boundingBox.y * 0.5f) + -weaponBase.right * (boundingBox.x * 0.5f); // Top left corner
        globalBoxCorners[1] = weaponBase.position + weaponBase.up * (boundingBox.y * 0.5f) + weaponBase.right * (boundingBox.x * 0.5f); // Top right corner
        globalBoxCorners[2] = weaponBase.position + -weaponBase.up * (boundingBox.y * 0.5f) + weaponBase.right * (boundingBox.x * 0.5f); // Bottom right corner
        globalBoxCorners[3] = weaponBase.position + -weaponBase.up * (boundingBox.y * 0.5f) + -weaponBase.right * (boundingBox.x * 0.5f); // Bottom left corner
        Debug.DrawLine(globalBoxCorners[0], globalBoxCorners[1], Color.red);
        Debug.DrawLine(globalBoxCorners[1], globalBoxCorners[2], Color.red);
        Debug.DrawLine(globalBoxCorners[2], globalBoxCorners[3], Color.red);
        Debug.DrawLine(globalBoxCorners[3], globalBoxCorners[0], Color.red);
        #endregion

        weaponPos += cachedMouseDelta * mouseSensitivity;
        weaponPos.x = Mathf.Clamp(weaponPos.x, -boundingBox.x * 0.5f, boundingBox.x * 0.5f);
        weaponPos.y = Mathf.Clamp(weaponPos.y, -boundingBox.y * 0.5f, boundingBox.y * 0.5f);
        cachedMouseDelta = Vector2.zero;

        // Update the UI
        boundingBoxUI.SetPosition(new Vector2(CMath.Map(weaponPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(weaponPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));

        // Move the object
        weaponObject.localPosition = new Vector3(weaponPos.x, weaponPos.y, 0);
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
