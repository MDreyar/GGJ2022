using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float Map(this float value, float inMin, float inMax, float outMin, float outMax) => (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
}
