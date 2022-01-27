using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public float gravityScale;
    public float fallGravityMult;

    [Header("Run")]
    public float runMaxSpeed;
    public float runAcceleration;
    public float runDeceleration;
    public float runAcceleratePower;
    public float runAccelerationInAir;
    public float runDecelerationInAir;
    public float runStopPower;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCutMultiplier;
    public float coyoteTime;
    public float jumpBufferTime;
}