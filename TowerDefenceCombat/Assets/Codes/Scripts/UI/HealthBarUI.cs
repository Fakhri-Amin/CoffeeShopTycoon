using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    // [SerializeField] private MMProgressBar healthBar;

    [Header("Child References")]
    [SerializeField] private Transform mask;
    [SerializeField] private Image barFront;

    private Color startingBarFront;

    private void Start()
    {
        // startingBarFront = barFront.color;
    }

    private void OnEnable()
    {
        healthSystem.OnHealthReset += HealthSystem_OnHealthReset;
        // healthSystem.OnDead += HealthSystem_OnHealthReset;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void OnDisable()
    {
        healthSystem.OnHealthReset -= HealthSystem_OnHealthReset;
        // healthSystem.OnDead -= HealthSystem_OnHealthReset;
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    public void HealthSystem_OnHealthReset()
    {
        // healthBar.transform.localScale = Vector3.one;
        mask.localScale = Vector3.one;
        barFront.color = startingBarFront;
    }

    private void UpdateHealthBar()
    {
        // healthBar.UpdateBar01(healthSystem.GetHealthNormalized());
        // healthBar.gameObject.SetActive(healthSystem.GetHealthNormalized() < 1);
    }
}
