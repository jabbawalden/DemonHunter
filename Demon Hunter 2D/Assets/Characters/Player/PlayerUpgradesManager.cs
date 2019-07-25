using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    private PlayerMeleeAttack _playerMelee;
    private PlayerController _playerController;
    private PlayerShoot _playerShoot;
    private PlayerEnergy _playerEnergy;
    private PlayerEnergyPoints _playerEnergyPoints;

    [Header("Upgrade Amounts")]
    [SerializeField] private float _speedUpgradeAmountSet;
    [SerializeField] private float _healthUpgradeAmountSet;
    [SerializeField] private float _energyUpgradeAmountSet;
    public float speedUpgradeAmount { get { return _speedUpgradeAmountSet; } private set { _speedUpgradeAmountSet = value; }}
    public float healthUpgradeAmount { get { return _healthUpgradeAmountSet; } private set { _healthUpgradeAmountSet = value; } }
    public float energyUpgradeAmount { get { return _energyUpgradeAmountSet; } private set { _energyUpgradeAmountSet = value; } }
    [Space(4)]

    [Header("Upgrade Costs")]
    [SerializeField] private int _speedUpgradeCostSet;
    [SerializeField] private int _healthUpgradeCostSet;
    [SerializeField] private int _energyUpgradeCostSet;
    public int speedUpgradeCost { get { return _speedUpgradeCostSet; } private set { _speedUpgradeCostSet = value; } }
    public int healthUpgradeCost { get { return _healthUpgradeCostSet; } private set { _healthUpgradeCostSet = value; } }
    public int energyUpgradeCost { get { return _energyUpgradeCostSet; } private set { _energyUpgradeCostSet = value; } }

    [SerializeField] private float upgradeValueMultiplier, upgradeCostMultiplier;
    [SerializeField] private int currentSpeedUpgrade, currentHealthUpgrade, currentEnergyUpgrade;
    [SerializeField] private int maxSpeedUpgrade, maxHealthUpgrade, maxEnergyUpgrade;

    private void Awake() 
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerSpeedUpgrade(speedUpgradeAmount, speedUpgradeCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerHealthUpgrade(healthUpgradeAmount, healthUpgradeCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerEnergyUpgrade(energyUpgradeAmount, energyUpgradeCost);
        }
    }

    public void LoadData()
    {
        speedUpgradeAmount = JsonDataManager.gameData.speedUpgradeAmount;
        healthUpgradeAmount = JsonDataManager.gameData.healthUpgradeAmount;
        energyUpgradeAmount = JsonDataManager.gameData.energyUpgradeAmount;

        speedUpgradeCost = JsonDataManager.gameData.speedUpgradeCost;
        healthUpgradeCost = JsonDataManager.gameData.healthUpgradeCost;
        energyUpgradeCost = JsonDataManager.gameData.energyUpgradeCost;
    }

    void PlayerSpeedUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.defaultMovementSpeed += amount;
            _playerController.currentMovementSpeed = _playerController.defaultMovementSpeed;

            speedUpgradeAmount += speedUpgradeAmount * upgradeValueMultiplier;
            int newSpeedUpgradeCost = Mathf.RoundToInt(speedUpgradeCost * upgradeCostMultiplier);
            speedUpgradeCost += newSpeedUpgradeCost;
        }

    }

    void PlayerHealthUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.playerHealthComp.maxHealth += amount;
            _playerController.playerHealthComp.currentHealth = _playerController.playerHealthComp.maxHealth;

            healthUpgradeAmount += healthUpgradeAmount * upgradeValueMultiplier;
            int newHealthUpgradeCost = Mathf.RoundToInt(healthUpgradeCost * upgradeCostMultiplier);
            healthUpgradeCost += newHealthUpgradeCost;
        }

    }

    void PlayerEnergyUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerEnergy.playerMaxEnergy += amount;
            _playerEnergy.currentEnergy = _playerEnergy.playerMaxEnergy;

            energyUpgradeAmount += energyUpgradeAmount * upgradeValueMultiplier;
            int newEnergyUpgradeCost = Mathf.RoundToInt(energyUpgradeCost * upgradeCostMultiplier);
            energyUpgradeCost += newEnergyUpgradeCost;
        }

    }

    void PlayerHealthRegen()
    {
        //do from health comp
    }

    void PlayerEnergyRegenUpgrade()
    {
        //one time increase
    }

    //upgrades required
    /*
     NORMALS
     Health amount increased - save max health
     Energy amount increased - save max energy
     Walk speed increased - 
     Allow for slow health regen
     Faster energy regen

     SPECIALS
     Healing Grace -> heal health total by 50% --- 15 second cooldown
     Planted bomb -> press Q to plant, then Q again to explode --- 10 second cooldown
     Energy Maximiser -> Limitless Energy for 3 seconds --- 40 second cooldown
     */


}
