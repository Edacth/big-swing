using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CMath
{
    // Remaps a range. Range 1 is the original, range 2 is the range to be mapped to.
    // https://stackoverflow.com/questions/3451553/value-remapping
    public static float Map(float _value, float _low1, float _high1, float _low2, float _high2)
    {
        return _low2 + (_value - _low1) * (_high2 - _low2) / (_high1 - _low1);
    }

    // Get a degrees rotation value from a vector2
    public static float Vec2ToRot(Vector2 _vec)
    {
        float value = (_vec.y / Mathf.Abs(_vec.y)) * Mathf.Acos(_vec.x / (Mathf.Sqrt(_vec.x * _vec.x + _vec.y * _vec.y)));
        return value * Mathf.Rad2Deg;
    }
}
