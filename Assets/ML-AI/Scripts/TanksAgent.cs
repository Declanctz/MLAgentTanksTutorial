using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class TanksAgent : Agent
{
    Rigidbody agentRb;

    // agent operation
    private TankMovementBrain agentMove;
    private TankShootingBrain agentShoot;

    private RayPerception rayPer;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentRb = GetComponent<Rigidbody>();
        Monitor.verticalOffset = 1f;
        rayPer = GetComponent<RayPerception>();
        agentMove = GetComponent<TankMovementBrain>();
        agentShoot = GetComponent<TankShootingBrain>();
    }

    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 0f, 45f, 70f, 90f, 110f, 135f, 180f, 240f, 300f };
        string[] detectableObjects = { "obstacle", "agent" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, 0.1f));
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 10f));
        AddVectorObs(transform.InverseTransformDirection(agentRb.velocity));
        AddVectorObs(transform.InverseTransformDirection(agentRb.angularVelocity));
        AddVectorObs(agentShoot.CurrentLaunchForce);
        AddVectorObs(agentShoot.Fired);
    }

    public void MoveAgent(float[] act)
    {
        // encourage attack
        AddReward(1f * agentRb.velocity.magnitude / 3000f);

        int forward = (int)act[0];
        int rotate = (int)act[1];
        int shoot = (int)act[2];

        switch (forward)
        {
            case 1 :agentMove.moveInput = -1;break;
            case 2 :agentMove.moveInput = 1;break;
            default:agentMove.moveInput = 0;break;
        }

        switch (rotate)
        {
            case 1 :agentMove.turnInput = -1;break;
            case 2 :agentMove.turnInput = 1;break;
            default:agentMove.turnInput = 0;break;
        }

        switch (shoot)
        {
            case 1 :agentShoot.fireInput = true;break;
            default:agentShoot.fireInput = false;break;
        }
    }

    public void TakeDamage(float amount)
    {
        AddReward(-0.01f * amount);
    }

    public void MakeDamage(float amount)
    {
        AddReward(0.02f * amount);
    }

    public void EmptyShot()
    {
        AddReward(-0.001f);
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        MoveAgent(vectorAction);
    }

    public override void AgentReset()
    {
        //SendMessage("TakeDamage", 100f);
    }


    public override void AgentOnDone()
    {

    }
}
