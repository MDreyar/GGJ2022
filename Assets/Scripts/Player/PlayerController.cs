using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerData data;

    public InputActions Input { get; private set; }
    public Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer renderer;

    #region State machine
    public string CurrentState;
    StateMachine stateMachine;
    public PlayerIdleState idleState;
    public PlayerRunState runState;
    public PlayerJumpState jumpState;
    public PlayerInAirState airState;
    public PlayerWaterDrawState waterDrawState;
    #endregion

    public float LastOnGroundTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedWaterDrawTime { get; private set; }

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundLayer;

    private void Awake() {
        Input = new InputActions();

        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        #region State machine
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, data);
        runState = new PlayerRunState(this, stateMachine, data);
        jumpState = new PlayerJumpState(this, stateMachine, data);
        airState = new PlayerInAirState(this, stateMachine, data);
        waterDrawState = new PlayerWaterDrawState(this, stateMachine, data);
        #endregion
    }

    #region Basic Unity updates
    private void OnEnable() {
        Input.Default.Jump.performed += OnJump;
        Input.Default.Kill.performed += OnKill;
        Input.Default.Enable();
    }
    private void OnDisable() {
        Input.Default.Jump.performed -= OnJump;
        Input.Default.Kill.performed -= OnKill;
        Input.Default.Disable();
    }

    private void Start() {
        stateMachine.Initialize(this, idleState);
        SetGravityScale(data.gravityScale);
    }

    private void Update() {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedWaterDrawTime -= Time.deltaTime;

        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            LastOnGroundTime = data.coyoteTime;

        if(Mathf.Abs(rb.velocity.x) >= 0.01f) {
            if (rb.velocity.x < 0)
                renderer.flipX = true;
            else
                renderer.flipX = false;
        }

        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        stateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Input Events
    private void OnJump(InputAction.CallbackContext context) {
        LastPressedJumpTime = data.jumpBufferTime;
    }

    private void OnKill(InputAction.CallbackContext context) {
        LastPressedWaterDrawTime = data.waterDrawBufferTime;
    }
    #endregion

    #region Movement
    public void SetGravityScale(float scale) {
        rb.gravityScale = scale;
    }

    public void Run() {
        float targetSpeed = Input.Default.SidewaysMovement.ReadValue<float>() * data.runMaxSpeed;
        float speedDif = targetSpeed - rb.velocity.x;

        float accelRate;
        if (LastOnGroundTime > 0) {
            accelRate = Mathf.Abs(targetSpeed) > 0.01f ? data.runAcceleration : data.runDeceleration;
        } else {
            accelRate = Mathf.Abs(targetSpeed) > 0.01f ? data.runAcceleration * data.runAccelerationInAir : data.runDeceleration * data.runDecelerationInAir;
        }

        float velocityPower = Mathf.Abs(targetSpeed) < 0.01f ? data.runStopPower : data.runAcceleratePower;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);
        rb.AddForce(Vector2.right * movement);
    }

    public void Jump() {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float jumpForce = data.jumpForce;
        if (rb.velocity.y < 0)
            jumpForce -= rb.velocity.y;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void JumpCut() {
        rb.AddForce(Vector2.down * rb.velocity.y * data.jumpCutMultiplier, ForceMode2D.Impulse);
    }
    #endregion

    private void OnDrawGizmos() {
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
    }
}
