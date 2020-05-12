using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum COMBATTYPE
{
    FollowMouse,
    SetTarget
}

public class PlayerSwing : MonoBehaviour
{
    [SerializeField] COMBATTYPE combatType = default;
    [SerializeField] Transform weaponBase = null;
    [SerializeField] Transform weaponObject = null;
    [SerializeField] Vector2 boundingBox = new Vector2(2f, 2f);
    [Tooltip("How far in front of the character the weapon box should be")]
    [SerializeField] [Range(0,2)] float boxForwardDist = 0.7f;
    [SerializeField] [Range(0, 0.02f)] float mouseSensitivity = 0.01f;
    [Tooltip("How quickly the weapon position moves towards the target position")]
    [SerializeField] [Range(0, 1)] float weaponFollowSpeed = 1f;
    [SerializeField] BoundingBoxUI boundingBoxUI = null;
    InputMaster controls;

    Vector2 cursorPos;
    Vector2 targetWeaponPos;
    Vector2 weaponPos;
    Vector3[] globalBoxCorners;
    bool cameraLocked;
    Vector2 cachedMouseDelta;
    bool weaponExtended;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.LockCamera.performed += ctx => LockCamera(ctx);
        controls.Player.UnlockCamera.performed += ctx => UnlockCamera(ctx);
        controls.Player.SetTargetWeaponPosition.performed += ctx => SetTargetWeaponPosition(ctx);
        controls.Player.ToggleWeaponExtension.performed += ctx => ToggleWeaponExtension(ctx);
    }

    void Start()
    {
        targetWeaponPos = new Vector2(0, 0);
        weaponPos = targetWeaponPos;
        globalBoxCorners = new Vector3[4];
        weaponExtended = true;
        if (weaponExtended) { ExtendWeapon(); }
        else { RetractWeapon(); }
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
        if (combatType == COMBATTYPE.FollowMouse)
        {
            targetWeaponPos += cachedMouseDelta * mouseSensitivity;
            targetWeaponPos.x = Mathf.Clamp(targetWeaponPos.x, -boundingBox.x * 0.5f, boundingBox.x * 0.5f);
            targetWeaponPos.y = Mathf.Clamp(targetWeaponPos.y, -boundingBox.y * 0.5f, boundingBox.y * 0.5f);
            cachedMouseDelta = Vector2.zero;

            // Update the UI
            boundingBoxUI.SetTargetWeaponPosition(new Vector2(CMath.Map(targetWeaponPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(targetWeaponPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));
            boundingBoxUI.SetWeaponPosition(new Vector2(CMath.Map(weaponPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(weaponPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));

            // Move the object
            weaponPos = Vector2.MoveTowards(weaponPos, targetWeaponPos, weaponFollowSpeed);
            weaponObject.localPosition = new Vector3(weaponPos.x, weaponPos.y, 0);
        }
        else if (combatType == COMBATTYPE.SetTarget)
        {
            cursorPos += cachedMouseDelta * mouseSensitivity;
            cursorPos.x = Mathf.Clamp(cursorPos.x, -boundingBox.x * 0.5f, boundingBox.x * 0.5f);
            cursorPos.y = Mathf.Clamp(cursorPos.y, -boundingBox.y * 0.5f, boundingBox.y * 0.5f);
            cachedMouseDelta = Vector2.zero;

            // Update the UI
            boundingBoxUI.SetCursorPosition(new Vector2(CMath.Map(cursorPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(cursorPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));
            boundingBoxUI.SetTargetWeaponPosition(new Vector2(CMath.Map(targetWeaponPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(targetWeaponPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));
            boundingBoxUI.SetWeaponPosition(new Vector2(CMath.Map(weaponPos.x, -boundingBox.x / 2, boundingBox.x / 2, -1, 1), CMath.Map(weaponPos.y, -boundingBox.y / 2, boundingBox.y / 2, -1, 1)));

            // Move the object
            weaponPos = Vector2.MoveTowards(weaponPos, targetWeaponPos, weaponFollowSpeed);
            weaponObject.localPosition = new Vector3(weaponPos.x, weaponPos.y, 0);
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

    private void SetTargetWeaponPosition(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        targetWeaponPos = cursorPos;
    }

    private void ToggleWeaponExtension(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        weaponExtended = !weaponExtended;
        if (weaponExtended) { ExtendWeapon(); }
        else { RetractWeapon(); }
    }

    private void ExtendWeapon()
    {
        //Debug.Log("Extend");
        weaponObject.localEulerAngles = new Vector3(0, weaponObject.localEulerAngles.y, weaponObject.localEulerAngles.z);
    }

    private void RetractWeapon()
    {
        //Debug.Log("Retract");
        weaponObject.localEulerAngles = new Vector3(-90, weaponObject.localEulerAngles.y, weaponObject.localEulerAngles.z);
    }
}
