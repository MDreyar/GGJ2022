using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerIdleState : PlayerGroundedState {
    public PlayerIdleState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter() {
        base.Enter();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (player.Input.Default.SidewaysMovement.inProgress) {
            stateMachine.ChangeState(player.runState);
        }
        if (player.LastPressedWaterDrawTime > 0) {
            var deathEffectObjects = GameObject.FindObjectsOfType<DeathEffect>();
            if (!deathEffectObjects.Any(de => Vector2.Distance(player.transform.position, de.transform.position) < 6))
                stateMachine.ChangeState(player.waterDrawState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        player.Run();
    }
}
