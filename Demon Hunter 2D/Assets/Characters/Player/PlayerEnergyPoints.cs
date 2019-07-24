using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyPoints : MonoBehaviour
{
    public int energyPoints;
    public float energyLossPercent;

    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void LoadData()
    {
        energyPoints = JsonDataManager.gameData.energyPoints;
        _uiManager.UpdateEnergyPoints();
    }

    public void AddRemovePoints(int amount)
    {
        energyPoints += amount;

        if (energyPoints <= 0)
        {
            energyPoints = 0;
        }

        _uiManager.UpdateEnergyPoints();
    }

    public void PointsLossDeath()
    {
        float lossResult = energyPoints * energyLossPercent;
        int lossAmount = Mathf.RoundToInt(lossResult);
        AddRemovePoints(-lossAmount);
    }
}

