using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnHealthReset;
    public Action OnHealthChanged;
    public Action OnDead;

    private float healthAmount;
    private float maxHealth;

    public void ResetHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        healthAmount = maxHealth;
        OnHealthReset?.Invoke();
        OnHealthChanged?.Invoke();
    }

    public void Damage(float damageAmount)
    {
        healthAmount -= damageAmount;

        if (healthAmount <= 0)
        {
            healthAmount = 0;
            OnDead?.Invoke();
        }

        OnHealthChanged?.Invoke();
    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / maxHealth;
    }
}
