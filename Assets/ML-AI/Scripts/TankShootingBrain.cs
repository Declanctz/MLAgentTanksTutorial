using UnityEngine;

public class TankShootingBrain : Complete.TankShooting
{
    private bool m_LastFireInput = false;
    public bool fireInput = false;

    public float CurrentLaunchForce
    {
        get
        {
            return base.m_CurrentLaunchForce;
        }
    }

    public bool Fired
    {
        get
        {
            return base.m_Fired;
        }
    }

    private void Start()
    {
        // The fire axis is based on the player number.
        base.m_FireButton = "Fire" + base.m_PlayerNumber;

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        base.m_ChargeSpeed = (base.m_MaxLaunchForce - base.m_MinLaunchForce) / base.m_MaxChargeTime;
    }

    private void Update()
    {
        // Block Parent
    }

    private void FixedUpdate()
    {
        // The slider should have a default value of the minimum launch force.
        base.m_AimSlider.value = base.m_MinLaunchForce;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (base.m_CurrentLaunchForce >= base.m_MaxLaunchForce && !base.m_Fired)
        {
            // ... use the max force and launch the shell.
            base.m_CurrentLaunchForce = base.m_MaxLaunchForce;
            base.Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
        else if (fireInput && !m_LastFireInput)
        {
            // ... reset the fired flag and reset the launch force.
            base.m_Fired = false;
            base.m_CurrentLaunchForce = base.m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            base.m_ShootingAudio.clip = base.m_ChargingClip;
            base.m_ShootingAudio.Play();
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
        else if (fireInput && !base.m_Fired)
        {
            // Increment the launch force and update the slider.
            base.m_CurrentLaunchForce += base.m_ChargeSpeed * Time.deltaTime;
            base.m_AimSlider.value = base.m_CurrentLaunchForce;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
        else if (!fireInput && m_LastFireInput && !base.m_Fired)
        {
            // ... launch the shell.
            base.Fire();
        }

        m_LastFireInput = fireInput;

    }

}