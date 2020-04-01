using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] bool smooth;
    [SerializeField] float smoothTime = 5f;
    [SerializeField] [Range(0, 1)] float XSensitivity;
    [SerializeField] [Range(0, 1)] float YSensitivity;
    [SerializeField] GameObject cameraGimble;
    [SerializeField] Camera camera;
    InputMaster controls;

    public float MinimumX = -90F;
    public float MaximumX = 90F;

    // Camera Gimble Rotation
    private Vector3 gimbleRot;
    private Vector3 gimbleTargRot;

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

    void Awake()
    {
        controls = new InputMaster();
        if (cameraGimble == null) { Debug.LogError("cameraGimble is null", this); }
        gimbleRot = cameraGimble.transform.localRotation.eulerAngles;
        gimbleTargRot = gimbleRot;
    }

    void Update()
    {
        Vector2 mouseDelta = (controls.Player.Look.ReadValue<Vector2>());

        gimbleTargRot += new Vector3(-mouseDelta.y * YSensitivity, mouseDelta.x * XSensitivity, 0);
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
}