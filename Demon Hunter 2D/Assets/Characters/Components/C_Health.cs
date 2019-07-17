﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Health : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _maxHealth;
    public float _currentHealth;
    public bool isPlayerComponent;
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
        _currentHealth = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (damage >= _currentHealth)
            _currentHealth = 0;
        else
        {
            _currentHealth -= damage;
        }

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
        if (_currentHealth < _maxHealth)
            _currentHealth += heal;
        
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        _uiManager.UpdateHealthSlider();
        //print("heal player");
    }

    public float GetHealthPercent()
    {
        float percent = _currentHealth / _maxHealth;
        return percent;
    }

    public bool IsAlive()
    {
        if (_currentHealth <= 0)
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
