using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] IntEventChannelSO WaterPowerChanged;

    [SerializeField] Slider waterSlider;
    [SerializeField] float waterSliderChangeDuration;

    Coroutine current;

    private void OnEnable() {
        WaterPowerChanged.OnEventRaised += UpdateWaterLevelUI;
    }

    private void OnDisable() {
        WaterPowerChanged.OnEventRaised -= UpdateWaterLevelUI;
    }

    private void UpdateWaterLevelUI(int waterLevel) {
        if (current != null)
            StopCoroutine(current);
        current = StartCoroutine(MathHelper.SmoothTowards(waterSlider.value, waterLevel, waterSliderChangeDuration, (float newValue) => waterSlider.value = newValue));
    }
}
