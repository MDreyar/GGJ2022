using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyData data;
    [SerializeField] Transform fireLaunchPoint;

    [SerializeField] float currentHealth;

    new SpriteRenderer renderer;

    float TimeToLaunchFire;
    PlayerController player;
    private float fireGravityMultiplier;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        TimeToLaunchFire = Random.Range(data.fireLaunchFrequencyMin, data.fireLaunchFrequencyMax);
        player = FindObjectOfType<PlayerController>();
        currentHealth = data.maxHealth;
        fireGravityMultiplier = data.FirePrefab.GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0) {
            Destroy(gameObject);
        }

        if (renderer.isVisible) {
            TimeToLaunchFire -= Time.deltaTime;

            if (TimeToLaunchFire <= 0)
                LaunchFire();
        }
    }

    private void LaunchFire() {
        TimeToLaunchFire = Random.Range(data.fireLaunchFrequencyMin, data.fireLaunchFrequencyMax);
        var fire = Instantiate(data.FirePrefab, fireLaunchPoint.position, fireLaunchPoint.rotation).GetComponent<Fire>();
        fire.rb.velocity = GetLaunchforce();
    }

    private Vector2 GetLaunchforce() {
        var playerOffset = player.transform.position - this.transform.position;

        var g = (Physics2D.gravity.y * fireGravityMultiplier) * -1;
        var b = data.fireLaunchForce * data.fireLaunchForce - playerOffset.y * g;
        var discriminant = b * b - g * g * (playerOffset.x * playerOffset.x + playerOffset.y * playerOffset.y);

        if (discriminant < 0) {
            Debug.Log("No firing solution");
            return playerOffset.normalized * data.fireLaunchForce;
        }

        var discRoot = Mathf.Sqrt(discriminant);

        Debug.Log("Fireing solution got!");
        var T = Mathf.Sqrt((b - discRoot) * 2 / (g * g));
        return new Vector2(playerOffset.x / T, playerOffset.y / T + T * g / 2);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Attack")) {
            currentHealth -= data.damageFromWater;
        }
    }
}
