using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerData data;
    [SerializeField] Transform WaterBlobSpawnLoc;
    [SerializeField] IntEventChannelSO WaterPowerChanged;
    [SerializeField] PlayerStateChannelSO PlayerStateChannel;
    [SerializeField] VoidEventChannelSO NewDeathMaskChannel;

    public InputActions Input { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

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

    private int _waterPower;
    public int WaterPower {
        get { return _waterPower; }
        set {
            _waterPower = value;
            WaterPowerChanged.RaiseEvent(_waterPower);
        }
    }

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundLayer;

    private void Awake() {
        Input = new InputActions();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        #region State machine
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, data);
        runState = new PlayerRunState(this, stateMachine, data);
        jumpState = new PlayerJumpState(this, stateMachine, data);
        airState = new PlayerInAirState(this, stateMachine, data);
        waterDrawState = new PlayerWaterDrawState(this, stateMachine, data, NewDeathMaskChannel);
        #endregion
    }

    #region Basic Unity updates
    private void OnEnable() {
        Input.Default.Jump.performed += OnJump;
        Input.Default.Kill.performed += OnKill;
        Input.Default.WaterBlob.performed += OnWaterBlob;
        Input.Default.Enable();
    }
    private void OnDisable() {
        Input.Default.Jump.performed -= OnJump;
        Input.Default.Kill.performed -= OnKill;
        Input.Default.WaterBlob.performed -= OnWaterBlob;
        Input.Default.Disable();
    }

    private void Start() {
        stateMachine.Initialize(this, idleState, PlayerStateChannel);
        WaterPower = data.startingWater;
        SetGravityScale(data.gravityScale);
    }

    private void Update() {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedWaterDrawTime -= Time.deltaTime;

        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            LastOnGroundTime = data.coyoteTime;

        if (Mathf.Abs(rb.velocity.x) >= 0.01f) {
            if (rb.velocity.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
                WaterBlobSpawnLoc.rotation = Quaternion.Euler(0, 180, 0);
            } else {
                transform.localScale = Vector3.one;
                WaterBlobSpawnLoc.rotation = Quaternion.identity;
            }
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

    private void OnWaterBlob(InputAction.CallbackContext context) {
        if (WaterPower >= data.waterBallCost) {
            WaterPower -= data.waterBallCost;
            var waterBlob = Instantiate(data.waterBallPrefab, WaterBlobSpawnLoc.position, WaterBlobSpawnLoc.rotation).GetComponent<WaterBlob>();
            waterBlob.Launch(new Vector2(rb.velocity.x, 0), transform.localScale.x < 0);
        }
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
