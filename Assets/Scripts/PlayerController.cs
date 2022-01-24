using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float speed = 1;
    public float jumpSpeed = 5;

    Rigidbody2D rb;
    InputActions input;

    bool shouldJump = false;


    bool isGrounded {
        get {
            bool hit = Physics2D.BoxCast(new Vector2(rb.position.x, rb.position.y - 0.5f), Vector2.one, 0, Vector2.down, 0.3f, LayerMask.GetMask("Foreground"));
            Debug.DrawRay(new Vector2(rb.position.x, rb.position.y - 0.5f), Vector2.down, hit ? Color.green : Color.red);
            return hit;
        }
    }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        input = new InputActions();
    }

    void OnEnable() {
        input.Default.Enable();
    }

    void Update() {
        if (!shouldJump && isGrounded) {
            shouldJump = input.Default.Jump.WasPerformedThisFrame();
        }
    }

    void FixedUpdate() {
        var movement = input.Default.SidewaysMovement.ReadValue<float>();

        rb.velocity = new Vector2(movement * speed, shouldJump ? jumpSpeed : rb.velocity.y);

        shouldJump = false;
    }
}
