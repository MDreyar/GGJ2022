using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState {
    public PlayerRunState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (!player.Input.Default.SidewaysMovement.inProgress) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        player.Run();
    }
}
