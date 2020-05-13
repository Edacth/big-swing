using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundingBoxUI : MonoBehaviour
{
    [SerializeField] Sprite canSwingIndicatorSprite = null;
    [SerializeField] Sprite emptySprite = null;

    RectTransform boundBox;
    RectTransform weaponDot;
    RectTransform targetWeaponDot;
    RectTransform cursorDot;
    Image canSwingIndicator;

    Vector2 dotPos;

    private void Awake()
    {
        boundBox = transform.Find("BoundingBox").GetComponent<RectTransform>();
        weaponDot = boundBox.Find("WeaponDot").GetComponent<RectTransform>();
        targetWeaponDot = boundBox.Find("TargetWeaponDot").GetComponent<RectTransform>();
        cursorDot = boundBox.Find("CursorDot").GetComponent<RectTransform>();
        canSwingIndicator = boundBox.Find("CanSwingIndicator").GetComponent<Image>();

        cursorDot.gameObject.SetActive(false);
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
        if (!cursorDot.gameObject.activeSelf) { cursorDot.gameObject.SetActive(true); }

        Vector2 newPos = new Vector2();
        newPos.x = CMath.Map(_position.x, -1, 1, -70, 70);
        newPos.y = CMath.Map(_position.y, -1, 1, -70, 70);
        cursorDot.anchoredPosition = newPos;
    }

    public void SetCanSwingIndicator(bool _canSwing)
    {
        if (_canSwing)
        {
            canSwingIndicator.sprite = canSwingIndicatorSprite;
        }
        else
        {
            canSwingIndicator.sprite = emptySprite;
        }
    }
}
