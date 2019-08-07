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
    private UIManager _uiManager;

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
    [SerializeField] private int _damageMultiplierUpgradeCostSet;
    public int SpeedUpgradeCost { get { return _speedUpgradeCostSet; } private set { _speedUpgradeCostSet = value; } }
    public int HealthUpgradeCost { get { return _healthUpgradeCostSet; } private set { _healthUpgradeCostSet = value; } }
    public int EnergyUpgradeCost { get { return _energyUpgradeCostSet; } private set { _energyUpgradeCostSet = value; } }
    public int DamageMultiplierUpgradeCost { get { return _damageMultiplierUpgradeCostSet; } private set { _damageMultiplierUpgradeCostSet = value; } }
    [SerializeField] private int _healthRegenCost;

    [SerializeField] private float _upgradeValueMultiplier, _upgradeCostMultiplier;
    [SerializeField] private int _currentSpeedUpgrade, _currentHealthUpgrade, _currentEnergyUpgrade, _currentDamageUpgrade;
    [SerializeField] private int _maxSpeedUpgrade, _maxHealthUpgrade, _maxEnergyUpgrade, _maxDamageUpgrade;

    [SerializeField] private int _speedUpgradesLeft, _healthUpgradesLeft, _energyUpgradesLeft, _damageUpgradesLeft;
    public int SpeedUpgradesLeft { get { return _speedUpgradesLeft; } private set { _speedUpgradesLeft = value; } }
    public int HealthUpgradesLeft { get { return _healthUpgradesLeft; } private set { _healthUpgradesLeft = value; } }
    public int EnergyUpgradesLeft { get { return _energyUpgradesLeft; } private set { _energyUpgradesLeft = value; } }
    public int DamageUpgradesLeft { get { return _damageUpgradesLeft; } private set { _damageUpgradesLeft = value; } }

    private void Awake() 
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerShoot = FindObjectOfType<PlayerShoot>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _playerEnergyPoints = FindObjectOfType<PlayerEnergyPoints>();
        _uiManager = FindObjectOfType<UIManager>();
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
            PlayerDamageUpgrade(DamageMultiplierUpgradeCost);
        }
    }

    public void StartGameInitiate()
    {
        _uiManager.UpdateUpgradesCount(_speedUpgradesLeft, _uiManager.speedUpgradesLeft);
        _uiManager.UpdateUpgradesCount(_healthUpgradesLeft, _uiManager.healthUpgradesLeft);
        _uiManager.UpdateUpgradesCount(_energyUpgradesLeft, _uiManager.energyUpgradeLeft);
        _uiManager.UpdateUpgradesCount(_damageUpgradesLeft, _uiManager.damageUpgradesLeft);
    }

    public void LoadData()
    {
        SpeedUpgradeAmount = JsonDataManager.gameData.speedUpgradeAmount;
        HealthUpgradeAmount = JsonDataManager.gameData.healthUpgradeAmount;
        EnergyUpgradeAmount = JsonDataManager.gameData.energyUpgradeAmount;
        //damage upgrade not required as it stays the same amount

        SpeedUpgradeCost = JsonDataManager.gameData.speedUpgradeCost;
        HealthUpgradeCost = JsonDataManager.gameData.healthUpgradeCost;
        EnergyUpgradeCost = JsonDataManager.gameData.energyUpgradeCost;
        DamageMultiplierUpgradeCost = JsonDataManager.gameData.damageMultiplierUpgradeCost;

        //upgrades left numbers
        SpeedUpgradesLeft = JsonDataManager.gameData.speedUpgradesLeft;
        HealthUpgradesLeft = JsonDataManager.gameData.healthUpgradesLeft;
        EnergyUpgradesLeft = JsonDataManager.gameData.energyUpgradesLeft;
        DamageUpgradesLeft = JsonDataManager.gameData.damageUpgradesLeft;

        //ui updates
        _uiManager.UpdateUpgradesCount(SpeedUpgradesLeft, _uiManager.speedUpgradesLeft);
        _uiManager.UpdateUpgradesCount(HealthUpgradesLeft, _uiManager.healthUpgradesLeft);
        _uiManager.UpdateUpgradesCount(EnergyUpgradesLeft, _uiManager.energyUpgradeLeft);
        _uiManager.UpdateUpgradesCount(DamageUpgradesLeft, _uiManager.damageUpgradesLeft);
        //player controller deals with the health regen status

    }

    void PlayerSpeedUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && _currentSpeedUpgrade < _maxSpeedUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.defaultMovementSpeed += amount;
            _playerController.currentMovementSpeed = _playerController.defaultMovementSpeed;

            SpeedUpgradeAmount += SpeedUpgradeAmount * _upgradeValueMultiplier;
            int newSpeedUpgradeCost = Mathf.RoundToInt(SpeedUpgradeCost * _upgradeCostMultiplier);
            SpeedUpgradeCost += newSpeedUpgradeCost;

            _currentSpeedUpgrade++;
            print("Upgrade complete");

            SpeedUpgradesLeft = _maxSpeedUpgrade - _currentSpeedUpgrade;
            _uiManager.UpdateUpgradesCount(SpeedUpgradesLeft, _uiManager.speedUpgradesLeft);
        }
        else
        {
            print("max upgrades reached");
        }

    }

    void PlayerHealthUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && _currentHealthUpgrade < _maxHealthUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.playerHealthComp.maxHealth += amount;
            _playerController.playerHealthComp.CurrentHealth = _playerController.playerHealthComp.maxHealth;

            HealthUpgradeAmount += HealthUpgradeAmount * _upgradeValueMultiplier;
            int newHealthUpgradeCost = Mathf.RoundToInt(HealthUpgradeCost * _upgradeCostMultiplier);
            HealthUpgradeCost += newHealthUpgradeCost;

            _currentHealthUpgrade++;
            print("Upgrade complete");

            HealthUpgradesLeft = _maxHealthUpgrade - _currentHealthUpgrade;
            _uiManager.UpdateUpgradesCount(HealthUpgradesLeft, _uiManager.healthUpgradesLeft);
        }
        else
        {
            print("max upgrades reached");
        }
    }

    void PlayerEnergyUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && _currentEnergyUpgrade < _maxEnergyUpgrade)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerEnergy.playerMaxEnergy += amount;
            _playerEnergy.currentEnergy = _playerEnergy.playerMaxEnergy;
            _playerEnergy.EnergyAmountCalc();

            EnergyUpgradeAmount += EnergyUpgradeAmount * _upgradeValueMultiplier;
            int newEnergyUpgradeCost = Mathf.RoundToInt(EnergyUpgradeCost * _upgradeCostMultiplier);
            EnergyUpgradeCost += newEnergyUpgradeCost;

            _currentEnergyUpgrade++;
            print("Upgrade complete");

            EnergyUpgradesLeft = _maxEnergyUpgrade - _currentEnergyUpgrade;
            _uiManager.UpdateUpgradesCount(EnergyUpgradesLeft, _uiManager.energyUpgradeLeft);
        }
        else
        {
            print("max upgrades reached");
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
            _uiManager.healthRegenStatus.text = _uiManager.regenStatusOn;
        }
        else
        {
            print("max upgrades reached");
        }
    }

    void PlayerDamageUpgrade(int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost && _currentDamageUpgrade < _maxDamageUpgrade)
        {
            _playerController.IncreaseDamageMultiplier(DamageMultiplierAmount);
            _currentDamageUpgrade++;
            int newDamageCost = Mathf.RoundToInt(DamageMultiplierUpgradeCost * _upgradeCostMultiplier);
            DamageMultiplierUpgradeCost += newDamageCost;

            DamageUpgradesLeft = _maxDamageUpgrade - _currentDamageUpgrade;
            _uiManager.UpdateUpgradesCount(DamageUpgradesLeft, _uiManager.damageUpgradesLeft);
        }
        else
        {
            print("max upgrades reached");
        }
    }

    public void HealingGraceUpgrade()
    {



    }

    public void PlantedBombUpgrade()
    {

    }

    public void EnergyMaximizerUpgrade()
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
