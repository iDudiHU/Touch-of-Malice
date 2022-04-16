using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class intended to work with grid layout groups to create an image based health bar
/// </summary>
public class HealthBarDisplay : UIelement
{
    [Header("Settings")]
    [Tooltip("The health component to display health values for")]
    public Health targetHealth = null;
    [SerializeField]
    private float _targetHealth;
    [SerializeField]
    private float _previousHealth;

    [Tooltip("The maximum number of images to display before switching to just a number")]
    private int _maxHealth;
    [Tooltip("The slider for health")]
    [SerializeField]
    private Slider HealthSlider;
    [SerializeField]
    private TMP_Text _healthNumberText;

    private void Start()
    {
        if (targetHealth == null && (GameManager.instance != null && GameManager.instance.player != null))
        {
            targetHealth = GameManager.instance.player.GetComponentInChildren<Health>();
            _maxHealth = targetHealth.maximumHealth;
            HealthSlider.maxValue = _maxHealth;
            HealthSlider.value = targetHealth.currentHealth;
            _previousHealth = targetHealth.currentHealth;
            _targetHealth = targetHealth.currentHealth;
}
        UpdateUI();
    }

    /// <summary>
    /// Description:
    /// Upadates this UI element
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        //if (GameManager.instance != null && GameManager.instance.player != null)
        //{
        //    Health playerHealth = GameManager.instance.player.GetComponent<Health>();
        //    if (playerHealth != null)
        //    {
        //        SetChildImageNumber(playerHealth.currentHealth);
        //    }
        //}
        if (targetHealth != null && targetHealth.currentHealth != _previousHealth)
        {
            _targetHealth = targetHealth.currentHealth;

            if (_targetHealth != _previousHealth)
			{
				if (_targetHealth < _previousHealth)
				{
                    SetHealthNumber(true);
                }
				else
				{
                    SetHealthNumber(false);
                }
			}
                
        }
    }

    /// <summary>
    /// Description:
    /// Deletes and spawns images until this gameobject has as many children as the player has health
    /// Input: 
    /// int
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="number">The number of images that this object should have as children</param>
    private void SetHealthNumber(bool op)
    {
        if (HealthSlider != null)
        {
			if (op)
			{
                _previousHealth--;
                _healthNumberText.text = _previousHealth.ToString();
                HealthSlider.value = _previousHealth;
			}
			else
			{
                _previousHealth++;
                _healthNumberText.text = _previousHealth.ToString();
                HealthSlider.value = _previousHealth;
            }
            
        }
    }
}
