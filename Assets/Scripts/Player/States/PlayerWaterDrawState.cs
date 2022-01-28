using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterDrawState : PlayerState
{
    public PlayerWaterDrawState(PlayerController player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    float enterTime;

    public override void Enter() {
        base.Enter();
        enterTime = Time.time;

        GameObject.Instantiate(data.deathMask, player.rb.position, Quaternion.identity);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if(Time.time >= enterTime + data.waterDrawDuration) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
