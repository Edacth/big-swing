using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxUI : MonoBehaviour
{
    RectTransform boundBox;
    RectTransform weaponDot;
    RectTransform targetWeaponDot;
    RectTransform CursorDot;

    Vector2 dotPos;

    private void Awake()
    {
        boundBox = transform.Find("BoundingBox").GetComponent<RectTransform>();
        weaponDot = boundBox.Find("WeaponDot").GetComponent<RectTransform>();
        targetWeaponDot = boundBox.Find("TargetWeaponDot").GetComponent<RectTransform>();
        CursorDot = boundBox.Find("CursorDot").GetComponent<RectTransform>();

        CursorDot.gameObject.SetActive(false);
    }

    public void SetWeaponPosition(Vector2 _position)
    {
        Vector2 newPos = new Vector2();
        newPos.x = CMath.Map(_position.x, -1, 1, -70, 70);
        newPos.y = CMath.Map(_position.y, -1, 1, -70, 70);
        weaponDot.anchoredPosition = newPos;
    }

    public void SetTargetWeaponPosition(Vector2 _position)
    {
        Vector2 newPos = new Vector2();
        newPos.x = CMath.Map(_position.x, -1, 1, -70, 70);
        newPos.y = CMath.Map(_position.y, -1, 1, -70, 70);
        targetWeaponDot.anchoredPosition = newPos;
    }

    public void SetCursorPosition(Vector2 _position)
    {
        if (!CursorDot.gameObject.activeSelf) { CursorDot.gameObject.SetActive(true); }

        Vector2 newPos = new Vector2();
        newPos.x = CMath.Map(_position.x, -1, 1, -70, 70);
        newPos.y = CMath.Map(_position.y, -1, 1, -70, 70);
        CursorDot.anchoredPosition = newPos;
    }
}
