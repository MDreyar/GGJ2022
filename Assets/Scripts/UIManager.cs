using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] IntEventChannelSO WaterPowerChanged;
    [SerializeField] FloatEventChannelSO HealthChanged;

    [SerializeField] Slider waterSlider;
    [SerializeField] float waterSliderChangeDuration;
    [SerializeField] Slider healthSlider;
    [SerializeField] float healthSliderChangeDuration;

    Coroutine currentWater;
    Coroutine currentHealth;

    private void OnEnable() {
        WaterPowerChanged.OnEventRaised += UpdateWaterLevelUI;
        HealthChanged.OnEventRaised += UpdateHealthUI;
    }

    private void OnDisable() {
        WaterPowerChanged.OnEventRaised -= UpdateWaterLevelUI;
        HealthChanged.OnEventRaised -= UpdateHealthUI;
    }

    private void UpdateWaterLevelUI(int waterLevel) {
        if (currentWater != null)
            StopCoroutine(currentWater);
        currentWater = StartCoroutine(MathHelper.SmoothTowards(waterSlider.value, waterLevel, waterSliderChangeDuration, (float newValue) => waterSlider.value = newValue));
    }

    private void UpdateHealthUI(float health) {
        if (currentHealth != null)
            StopCoroutine(currentHealth);
        currentHealth = StartCoroutine(MathHelper.SmoothTowards(healthSlider.value, health, healthSliderChangeDuration, (float newValue) => healthSlider.value = newValue));
    }
}
