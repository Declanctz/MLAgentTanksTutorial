using UnityEngine;

public class TankMovementBrain: Complete.TankMovement
{
    public int moveInput;
    public int turnInput;
    public float smoothTime = 0.1f;

    private float m_MoveValue = 0f;
    private float m_vMoveValue = 0f;
    private float m_TurnValue = 0f;
    private float m_vTurnValue = 0f;


    private void Update()
    {
        // Block Parent
    }

    protected void FixedUpdate()
    {
        m_MoveValue = Mathf.Clamp((float)moveInput, -1f, 1f);
        m_TurnValue = Mathf.Clamp((float)turnInput, -1f, 1f);
        base.m_MovementInputValue = Mathf.SmoothDamp(base.m_MovementInputValue, m_MoveValue, ref m_vMoveValue, smoothTime);
        base.m_TurnInputValue = Mathf.SmoothDamp(base.m_TurnInputValue, m_TurnValue, ref m_vTurnValue, smoothTime);

        // Adjust the rigidbodies position and orientation in FixedUpdate.
        base.Move();
        base.Turn();
        base.EngineAudio();
    }
}
