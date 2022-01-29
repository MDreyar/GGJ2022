using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlob : MonoBehaviour
{
    public float launchForce;
    public float gravityScaleMultiplier;
    [SerializeField] Rigidbody2D rb;

    public void Launch(Vector2 currentVelocity, bool lookingLeft)
    {
        rb.velocity = currentVelocity;
        rb.AddForce((lookingLeft ? Vector2.left : Vector2.right) * launchForce, ForceMode2D.Impulse);
    }

    void Update() {
        rb.gravityScale += Time.deltaTime * gravityScaleMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
