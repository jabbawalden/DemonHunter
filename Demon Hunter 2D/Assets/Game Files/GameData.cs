using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //player position
    public Vector3 playerStartLocation;
    public Vector3 camStartLocation;

    //player stats (health, energy, points collected, upgrades etc.)
    public int energyPoints;
    public float playerHealth;
    public float playerMaxHealth;
    public float playerMaxEnergy; 
    public float defaultMovementSpeed;
    public bool meleeEnabled, shootEnabled, dashEnabled;
    public bool gameIntroMove, gameIntroMelee, gameIntroShoot, gameIntroDash;

    public float speedUpgradeAmount, healthUpgradeAmount, energyUpgradeAmount;
    public int speedUpgradesLeft, healthUpgradesLeft, energyUpgradesLeft, damageUpgradesLeft;
    public int speedUpgradeCost, healthUpgradeCost, energyUpgradeCost, damageMultiplierUpgradeCost;

    public bool canHealthRegen;
    public float damageMultiplier;

    //game state
    public bool tutorialComplete;

    //NPCs
    public bool oldWomanConv, shopConv, upgradeConv;

}
