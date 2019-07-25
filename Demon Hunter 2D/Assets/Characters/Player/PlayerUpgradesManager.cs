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
    [SerializeField] private float _damageMultiplierAmountSet;
    public float SpeedUpgradeAmount { get { return _speedUpgradeAmountSet; } private set { _speedUpgradeAmountSet = value; }}
    public float HealthUpgradeAmount { get { return _healthUpgradeAmountSet; } private set { _healthUpgradeAmountSet = value; } }
    public float EnergyUpgradeAmount { get { return _energyUpgradeAmountSet; } private set { _energyUpgradeAmountSet = value; } }
    public float DamageMultiplierAmount { get { return _damageMultiplierAmountSet;  } private set { _damageMultiplierAmountSet = value;  } }
    [Space(4)]

    [Header("Upgrade Costs")]
    [SerializeField] private int _speedUpgradeCostSet;
    [SerializeField] private int _healthUpgradeCostSet;
    [SerializeField] private int _energyUpgradeCostSet;
    [SerializeField] private int _damageMultiplierCostSet;
    public int SpeedUpgradeCost { get { return _speedUpgradeCostSet; } private set { _speedUpgradeCostSet = value; } }
    public int HealthUpgradeCost { get { return _healthUpgradeCostSet; } private set { _healthUpgradeCostSet = value; } }
    public int EnergyUpgradeCost { get { return _energyUpgradeCostSet; } private set { _energyUpgradeCostSet = value; } }
    public int DamageMultiplierCost { get { return _damageMultiplierCostSet; } private set { _damageMultiplierCostSet = value; } }
    [SerializeField] private int _healthRegenCost;

    [SerializeField] private float upgradeValueMultiplier, upgradeCostMultiplier;
    [SerializeField] private int currentSpeedUpgrade, currentHealthUpgrade, currentEnergyUpgrade, currentDamageUpgrade;
    [SerializeField] private int maxSpeedUpgrade, maxHealthUpgrade, maxEnergyUpgrade, maxDamageUpgrade;

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
            PlayerSpeedUpgrade(SpeedUpgradeAmount, SpeedUpgradeCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerHealthUpgrade(HealthUpgradeAmount, HealthUpgradeCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerEnergyUpgrade(EnergyUpgradeAmount, EnergyUpgradeCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerHealthRegen(_healthRegenCost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerDamageUpgrade(DamageMultiplierCost);
        }
    }

    public void LoadData()
    {
        SpeedUpgradeAmount = JsonDataManager.gameData.speedUpgradeAmount;
        HealthUpgradeAmount = JsonDataManager.gameData.healthUpgradeAmount;
        EnergyUpgradeAmount = JsonDataManager.gameData.energyUpgradeAmount;

        SpeedUpgradeCost = JsonDataManager.gameData.speedUpgradeCost;
        HealthUpgradeCost = JsonDataManager.gameData.healthUpgradeCost;
        EnergyUpgradeCost = JsonDataManager.gameData.energyUpgradeCost;
    }

    void PlayerSpeedUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && currentSpeedUpgrade < maxSpeedUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.defaultMovementSpeed += amount;
            _playerController.currentMovementSpeed = _playerController.defaultMovementSpeed;

            SpeedUpgradeAmount += SpeedUpgradeAmount * upgradeValueMultiplier;
            int newSpeedUpgradeCost = Mathf.RoundToInt(SpeedUpgradeCost * upgradeCostMultiplier);
            SpeedUpgradeCost += newSpeedUpgradeCost;

            currentSpeedUpgrade++;
            print("Upgrade complete");
        }

    }

    void PlayerHealthUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && currentHealthUpgrade < maxHealthUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.playerHealthComp.maxHealth += amount;
            _playerController.playerHealthComp.CurrentHealth = _playerController.playerHealthComp.maxHealth;

            HealthUpgradeAmount += HealthUpgradeAmount * upgradeValueMultiplier;
            int newHealthUpgradeCost = Mathf.RoundToInt(HealthUpgradeCost * upgradeCostMultiplier);
            HealthUpgradeCost += newHealthUpgradeCost;

            currentHealthUpgrade++;
            print("Upgrade complete");
        }

    }

    void PlayerEnergyUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && currentEnergyUpgrade < maxEnergyUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerEnergy.playerMaxEnergy += amount;
            _playerEnergy.currentEnergy = _playerEnergy.playerMaxEnergy;
            _playerEnergy.EnergyAmountCalc();

            EnergyUpgradeAmount += EnergyUpgradeAmount * upgradeValueMultiplier;
            int newEnergyUpgradeCost = Mathf.RoundToInt(EnergyUpgradeCost * upgradeCostMultiplier);
            EnergyUpgradeCost += newEnergyUpgradeCost;

            currentEnergyUpgrade++;
            print("Upgrade complete");
        }

    }

    void PlayerHealthRegen(int cost)
    {
        //do from health comp
        if (_playerEnergyPoints.energyPoints >= cost && !_playerController.playerHealthComp.canHealthRegen)
        {
            _playerController.playerHealthComp.canHealthRegen = true;
            _playerController.playerHealthComp.HealthRegenCalc();
            _playerEnergyPoints.AddRemovePoints(-cost);
        }
    }

    void PlayerDamageUpgrade(int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && currentDamageUpgrade < maxDamageUpgrade)
        {
            _playerController.IncreaseDamageMultiplier(DamageMultiplierAmount);
            currentDamageUpgrade++;
        }
    }

    public void HealingGraceUpgrade()
    {

    }

    public void PlantedBombUpgrade()
    {

    }

    public void EnergyMaximiserUpgrade()
    {
         
    }

    //upgrades required
    /*
     SPECIALS
     Healing Grace -> heal health total by 50% --- 30 second cooldown --- costs 80 energy
     Planted bomb -> press Q to plant, then Q again to explode --- 10 second cooldown --- costs 50 energy
     Energy Maximiser -> Limitless Energy for 3 seconds --- 40 second cooldown --- costs 70 energy
     */


}
