using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxUI : MonoBehaviour
{
    RectTransform boundBox;
    RectTransform dot;

    Vector2 dotPos;

    private void Awake()
    {
        boundBox = transform.Find("BoundingBox").GetComponent<RectTransform>();
        dot = boundBox.Find("Dot").GetComponent<RectTransform>();
    }

    public void SetPosition(Vector2 _position)
    {
        Vector2 newPos = new Vector2();
        newPos.x = CMath.Map(_position.x, -1, 1, -70, 70);
        newPos.y = CMath.Map(_position.y, -1, 1, -70, 70);
        dot.anchoredPosition = newPos;
    }
}
