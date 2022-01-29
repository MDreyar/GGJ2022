using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float Map(this float value, float inMin, float inMax, float outMin, float outMax) => (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    public static IEnumerator SmoothTowards(float start, float targetValue, float duration, Action<float> updateValue) {
        var startValue = start;
        var timeRunning = 0f;
        var currentValue = start;
        while (currentValue != targetValue) {
            currentValue = Mathf.Lerp(startValue, targetValue, timeRunning.Map(0, duration, 0, 1));
            updateValue(currentValue);
            yield return null;
            timeRunning += Time.deltaTime;
        }
    }
}
