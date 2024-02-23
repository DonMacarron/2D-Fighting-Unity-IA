using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class FirstAgent : Agent
{
    [SerializeField] private Transform enemyPos;
    public BotMoves BotMoves;

    public float hitRewardAmount = 0.5f;
    public float lifeLostRewardAmount = -2f;
    public float enemyKillRewardAmount = 2f;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(enemyPos.localPosition);
        sensor.AddObservation(BotMoves.jumpsLeft);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        BotMoves.setHorizontal(actions.DiscreteActions[0]);

        BotMoves.setVertical(actions.DiscreteActions[1]);

        int j = actions.DiscreteActions[2];
        if (j == 1) BotMoves.setJump(true);
        else BotMoves.setJump(false);

        int a = actions.DiscreteActions[3];
        if (a == 1) BotMoves.setAttack(true);
        else BotMoves.setAttack(false);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int) Input.GetAxisRaw("Horizontal");
        discreteActions[1] = (int) Input.GetAxisRaw("Vertical");
    }


    //rewards
    public void hitReward() {
        Debug.Log("Hittt");
        SetReward(hitRewardAmount); }
    public void lifeLostReward() {
        Debug.Log("lifelost");
        SetReward(lifeLostRewardAmount); }
    public void enemyKillReward() {
        Debug.Log("kill");
        SetReward(enemyKillRewardAmount); }



}
