using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] FloatEventChannelSO HealthChange;

    private void OnEnable() {
        HealthChange.OnEventRaised += HealthChanged;
    }

    private void OnDisable() {
        HealthChange.OnEventRaised -= HealthChanged;
    }

    private void HealthChanged(float health) {
        if(health <= 0) {
            SceneManager.LoadScene(2);
        }
    }

    private void Update() {
        var allEnemies = FindObjectsOfType<EnemyController>();
        var allFire = FindObjectsOfType<Fire>();

        if (!allEnemies.Any() && !allFire.Any())
            SceneManager.LoadScene(3);
    }
}
