using System.Collections;
using System.Collections.Generic;

public static class CMath
{
    // Remaps a range. Range 1 is the original, range 2 is the range to be mapped to.
    // https://stackoverflow.com/questions/3451553/value-remapping
    public static float Map(float _value, float _low1, float _high1, float _low2, float _high2)
    {
        return _low2 + (_value - _low1) * (_high2 - _low2) / (_high1 - _low1);
    }
}
