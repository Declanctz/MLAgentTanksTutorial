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
    private TankHealthBrain agentHealth;
    public bool _needReset = false;

    private RayPerception rayPer;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentRb = GetComponent<Rigidbody>();
        Monitor.verticalOffset = 1f;
        rayPer = GetComponent<RayPerception>();
        agentMove = GetComponent<TankMovementBrain>();
        agentShoot = GetComponent<TankShootingBrain>();
        agentHealth = GetComponent<TankHealthBrain>();
    }

    public override void CollectObservations()
    {
        // Top Down Ray Percption
        float rayDistance = 50f;
        float[] rayAnglesTD = { 0f, 30f, 50f, 70f, 90f,
                              110f, 130f, 150f, 180f,
                              220f, 270f, 320f };
        string[] detectableObjects = { "obstacle", "agent", "shell" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAnglesTD, detectableObjects, 1f, 0.1f));
        // Buttom Up Ray Perception
        float[] rayAnglesBU = { 75f, 80f, 85f,
                              95f, 100f, 105f};
        AddVectorObs(rayPer.Perceive(rayDistance, rayAnglesBU, detectableObjects, 0.1f, 1f));
        // Ego State
        AddVectorObs(agentHealth.CurrentHealth);
        AddVectorObs(transform.InverseTransformDirection(agentRb.velocity));
        AddVectorObs(agentShoot.CurrentLaunchForce);
        AddVectorObs(agentShoot.Fired);
    }

    public void MoveAgent(float[] act)
    {
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
        // Dont be hit
        AddReward(-1f);
    }

    public void MakeDamage(float amount)
    {
        AddReward(amount);
    }

    public void EmptyShot()
    {
        // stop no meaning shots
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        MoveAgent(vectorAction);

        if (agentHealth.Dead) Done();
    }

    public override void AgentReset()
    {
        _needReset = true;
    }
}
