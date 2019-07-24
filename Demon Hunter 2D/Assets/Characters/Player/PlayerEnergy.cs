using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private float _currentEnergy;
    public float playerMaxEnergy;
    public float currentEnergy
    {
        get
        {
            return _currentEnergy;
        }
        set
        {
            if (value < 0)
                _currentEnergy = 0;
            else if (value > playerMaxEnergy)
                _currentEnergy = playerMaxEnergy;
            else
                _currentEnergy = value;
        }
    }

    [SerializeField] private float energyRegenPercentSet;
    public float energyRegenPercent { get { return energyRegenPercentSet; } }
    [SerializeField] private float energyAmount;

    //[SerializeField] private float _regenRate;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = playerMaxEnergy;
        energyAmount = playerMaxEnergy * energyRegenPercent;
    }

    private void FixedUpdate()
    { 
        EnergyRegenerate();
    }

    private void EnergyRegenerate()
    {
        if (currentEnergy < playerMaxEnergy)
        {
            currentEnergy += energyAmount;
            uiManager.UpdateEnergySlider();
        }
    }

    public void AddEnergy(int energyToAdd)
    {
        currentEnergy += energyToAdd;
    }

    public void RemoveEnergy(float energyAmount)
    {
        currentEnergy -= energyAmount;
    }

    public float GetEnergyPercent()
    {
        float percent = currentEnergy / playerMaxEnergy;
        return percent;
    }
}
