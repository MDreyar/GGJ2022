using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fire : MonoBehaviour
{
    public Rigidbody2D rb;
    private new Light2D light;
    private float target;
    private bool killed;

    private void Awake() {
        light = GetComponent<Light2D>();
        rb = GetComponent<Rigidbody2D>();
        target = 5;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = Mathf.MoveTowards(light.intensity, target, Time.deltaTime);
    }

    void FixedUpdate() {
        if(!killed)
            target = Random.Range(1.1f, 1.7f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Attack")) {
            killed = true;
            target = 0;
            GetComponent<ParticleSystem>().Stop();
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(gameObject, 2);
            Destroy(collision.gameObject);
        }
    }
}
