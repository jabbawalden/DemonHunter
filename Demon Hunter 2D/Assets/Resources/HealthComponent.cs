using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Variables")]
    public float maxHealth;
    [SerializeField] private float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            if (value > maxHealth)
                _currentHealth = maxHealth;
            else if (value < 0)
                _currentHealth = 0;
            else
                _currentHealth = value;
        }
    }
    public bool isPlayerComponent;
    public bool canHealthRegen;

    [SerializeField] private float healthRegenPercentSet;
    public float HealthRegenPercent { get { return healthRegenPercentSet; } }
    [SerializeField] private float _healthRegenAmount;
    [SerializeField] private float _healRate, _newTime;
    [Space(4)]


    [Header("Scripts")]
    private UIManager _uiManager;
    [SerializeField] private PlayerCamera playerCam;


    private void Awake()
    {
        if (isPlayerComponent)
            _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        if (canHealthRegen)
            HealthRegenCalc();
    }

    private void FixedUpdate()
    {
        if (IsAlive() && canHealthRegen && CurrentHealth < maxHealth && _newTime <= Time.time)
        {
            _newTime = Time.time + _healRate;
            CurrentHealth += _healthRegenAmount;
            if (_uiManager)
                _uiManager.UpdateHealthSlider();
        }
    }

    public void HealthRegenCalc()
    {
        _healthRegenAmount = maxHealth * HealthRegenPercent;
        if (_uiManager)
            _uiManager.UpdateHealthSlider();
        //print("health regen calc activated");
    }

    public void Damage(float damage)
    {
        CurrentHealth -= damage;

        if (IsAlive())
        {
            if (_uiManager)
            {
                _uiManager.UpdateHealthSlider();
                _uiManager.DamageHealthBar();
            }

            if (isPlayerComponent)
                playerCam.CameraShake(0.11f, 0.14f);
        }
        else if (isPlayerComponent && !IsAlive())
        {
            GameEvents.ReportPlayerDeath();
        }
    }

    public void Heal(float heal)
    {
        if (CurrentHealth < maxHealth)
            CurrentHealth += heal;
        
        //if (currentHealth > _maxHealth)
        //    currentHealth = _maxHealth;

        _uiManager.UpdateHealthSlider();
        //print("heal player");
    }

    public float GetHealthPercent()
    {
        float percent = CurrentHealth / maxHealth;
        return percent;
    }

    public bool IsAlive()
    {
        if (CurrentHealth <= 0)
        {
            //to ensure healthbar reaches 0
            if (_uiManager)
                _uiManager.UpdateHealthSlider();
            return false;
        }
        else
            return true;
    }
   
}
