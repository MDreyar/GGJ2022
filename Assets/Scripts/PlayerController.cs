using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [Header("Speed settings")]
    public float MaxSpeed = 9;
    public float accelerationRate = 13;
    public float decelertionRate = 16;
    public float velPow = 0.96f;
    public float frictionAmount = 0.2f;

    [Header("Jump settings")]
    public Transform groundCheckBoxLocation;
    public Vector2 groundCheckBoxSize;
    public LayerMask GroundLayers;
    public float jumpForce = 5;
    public float coyoteTime = 0.15f; 

    Rigidbody2D rb;
    InputActions input;
    float lastGroundedTime;

    bool isGrounded => Physics2D.OverlapBox(groundCheckBoxLocation.position, groundCheckBoxSize, 0, GroundLayers);

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        input = new InputActions();
    }

    void OnEnable() {
        input.Default.Jump.performed += Jump;
        input.Default.Enable();
    }

    void OnDisable() {
        input.Default.Jump.performed -= Jump;
        input.Default.Disable();
    }

    private void Jump(InputAction.CallbackContext obj) {
        if (lastGroundedTime > 0) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Update() {
        lastGroundedTime -= Time.deltaTime;

        if (isGrounded)
            lastGroundedTime = coyoteTime;
    }

    void FixedUpdate() {
        var moveInput = input.Default.SidewaysMovement.ReadValue<float>();

        float targetSpeed = moveInput * MaxSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationRate : decelertionRate;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, velPow) * Mathf.Sign(speedDiff);
        rb.AddForce(movement * Vector2.right);

        if(isGrounded && !input.Default.SidewaysMovement.inProgress) {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawCube(groundCheckBoxLocation.position, groundCheckBoxSize);
    }
}
