using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Transform healthGroup;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void OnDisable()
    {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
        healthGroup.gameObject.SetActive(healthSystem.GetHealthNormalized() < 1);
    }
}
