using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Health")]
    public float maxHealth;
    public float damageFromWater;

    [Header("Attack")]
    public GameObject FirePrefab;
    public float fireLaunchForce;
    public float fireLaunchFrequencyMin;
    public float fireLaunchFrequencyMax;
}
