using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if(player.LastPressedJumpTime > 0) {
            stateMachine.ChangeState(player.jumpState);
        }else if(player.LastOnGroundTime <= 0) {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
