﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyPoints : MonoBehaviour
{
    public int energyPoints;
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
        _uiManager.UpdateEnergyPoints();
    }

}

