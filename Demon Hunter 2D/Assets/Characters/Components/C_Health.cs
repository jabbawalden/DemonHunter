using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Health : MonoBehaviour
{
    [Header("Variables")]
    public float maxHealth;
    [SerializeField] private float _currentHealth;
    public float currentHealth
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
    public float healthRegenPercent { get { return healthRegenPercentSet; } }
    [SerializeField] private float healthRegenAmount;

    [Space(4)]


    [Header("Scripts")]
    private UIManager _uiManager;
    [SerializeField] private PlayerCamera playerCam;


    private void Awake()
    {
        if (isPlayerComponent)
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        HealthRegenCalc();
    }

    private void FixedUpdate()
    {
        if (IsAlive() && canHealthRegen && currentHealth < maxHealth)
        {
            currentHealth += healthRegenAmount;
            _uiManager.UpdateHealthSlider();
        }
    }

    public void HealthRegenCalc()
    {
        healthRegenAmount = maxHealth * healthRegenPercent;
        print("health regen calc activated");
    }

    public void Damage(float damage)
    {
        //if (damage >= currentHealth)
        //    currentHealth = 0;
        //else
        //{
        //    currentHealth -= damage;
        //}

        currentHealth -= damage;

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
    }

    public void Heal(float heal)
    {
        if (currentHealth < maxHealth)
            currentHealth += heal;
        
        //if (currentHealth > _maxHealth)
        //    currentHealth = _maxHealth;

        _uiManager.UpdateHealthSlider();
        //print("heal player");
    }

    public float GetHealthPercent()
    {
        float percent = currentHealth / maxHealth;
        return percent;
    }

    public bool IsAlive()
    {
        if (currentHealth <= 0)
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
