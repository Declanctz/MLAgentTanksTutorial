using UnityEngine;
using System.Collections;

public class TankHealthBrain : Complete.TankHealth
{
    public bool Dead
    {
        get
        {
            return base.m_Dead;
        }
    }

    public float CurrentHealth
    {
        get
        {
            return base.m_CurrentHealth;
        }
    }

    public new void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done.
        base.m_CurrentHealth -= amount;


        // Change the UI elements appropriately.
        base.SetHealthUI();

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            // Set the flag so that this function is only called once.
            base.m_Dead = true;
        }
    }
}
