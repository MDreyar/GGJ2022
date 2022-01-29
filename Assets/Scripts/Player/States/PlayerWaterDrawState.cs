using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterDrawState : PlayerState {
    VoidEventChannelSO newDeathMaskChannel;

    public PlayerWaterDrawState(PlayerController player, StateMachine stateMachine, PlayerData data, VoidEventChannelSO newDeathMaskChannel) : base(player, stateMachine, data) {
        this.newDeathMaskChannel = newDeathMaskChannel;
    }

    float enterTime;

    public override void Enter() {
        base.Enter();
        enterTime = Time.time;

        GameObject.Instantiate(data.deathMaskprefab, player.rb.position, Quaternion.identity);
        newDeathMaskChannel.RaiseEvent();
    }

    public override void Exit() {
        base.Exit();

        player.WaterPower = Mathf.Min(player.WaterPower + data.waterDrawWaterGain, data.waterCapacity);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (Time.time >= enterTime + data.waterDrawDuration) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
