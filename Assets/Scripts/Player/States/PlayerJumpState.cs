using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter() {
        base.Enter();

        player.Jump();
        player.Animator.Play("Jump Up");
    }

    public override void Exit() {
        base.Exit();

        player.ParticleSystem.Stop();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (player.rb.velocity.y <= 0) {
            stateMachine.ChangeState(player.airState);
        } else if(!player.Input.Default.Jump.inProgress && player.rb.velocity.y > 0) {
            player.JumpCut();
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        player.Run();
    }
}
