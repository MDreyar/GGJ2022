using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState {
    public int DoubleJumpCharges { get; private set; }

    public PlayerInAirState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Exit() {
        base.Exit();

        player.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (player.LastOnGroundTime > 0) {
            if (player.LastPressedJumpTime > 0) {
                stateMachine.ChangeState(player.jumpState);
            } else {
                stateMachine.ChangeState(player.idleState);
            }
        }else if(player.LastPressedJumpTime > 0 && DoubleJumpCharges > 0) {
            stateMachine.ChangeState(player.jumpState);
            DoubleJumpCharges--;
        } 
        else if (player.rb.velocity.y < 0) {
            player.SetGravityScale(data.gravityScale * data.fallGravityMult);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        player.Run();
    }

    public void ResetJumpCharges() {
        DoubleJumpCharges = data.doubleJumpCharges;
    }
}
