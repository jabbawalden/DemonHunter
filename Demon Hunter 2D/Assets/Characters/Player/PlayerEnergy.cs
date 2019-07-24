﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    private float _maxEnergy;
    private float _currentEnergy;
    public float maxEnergy;

    public float currentEnergy
    {
        get
        {
            return _currentEnergy;
        }
        set
        {
            if (_currentEnergy < 0)
                _currentEnergy = 0;
            else
                _currentEnergy = value;
        }
    }

    [SerializeField] private float _regenRate;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    private void FixedUpdate()
    { 
        EnergyRegenerate();
    }

    private void EnergyRegenerate()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += _regenRate;
            uiManager.UpdateEnergySlider();
        }
    }

    public void AddEnergy(int energyToAdd)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyToAdd;
            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;
        }
    }

    public void RemoveEnergy(float energyAmount)
    {
        currentEnergy -= energyAmount;
        //if (currentEnergy >= energyAmount)
        //    currentEnergy -= energyAmount;
        //else
        //    currentEnergy = 0;
    }

    public float GetEnergyPercent()
    {
        float percent = currentEnergy / maxEnergy;
        return percent;
    }
}
