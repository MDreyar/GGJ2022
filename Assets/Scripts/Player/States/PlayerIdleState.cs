using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState {
    public PlayerIdleState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (player.Input.Default.SidewaysMovement.inProgress) {
            stateMachine.ChangeState(player.runState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        player.Run();
    }
}
