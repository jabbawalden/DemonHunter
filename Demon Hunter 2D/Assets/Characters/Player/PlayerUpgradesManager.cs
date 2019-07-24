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
    [SerializeField] private float speedUpgradeAmountSet;
    public float speedUpgradeAmount { get { return speedUpgradeAmountSet; } }
    [SerializeField] private float healthUpgradeAmountSet;
    public float healthUpgradeAmount { get { return healthUpgradeAmountSet; } }
    [SerializeField] private float energyUpgradeAmountSet;
    public float energyUpgradeAmount { get { return energyUpgradeAmountSet; } }
    [Space(4)]

    [Header("Upgrade Costs")]
    [SerializeField] private int speedUpgradeCostSet;
    public int speedUpgradeCost { get { return speedUpgradeCostSet; } }
    [SerializeField] private int healthUpgradeCostSet;
    public int healthUpgradeCost { get { return healthUpgradeCostSet; } }
    [SerializeField] private int energyUpgradeCostSet;
    public int energyUpgradeCost { get { return energyUpgradeCostSet; } }



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
            PlayerMovementUpgrade(speedUpgradeAmount, speedUpgradeCost);
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

    void PlayerMovementUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.defaultMovementSpeed += amount;
            _playerController.currentMovementSpeed = _playerController.defaultMovementSpeed;
        }

    }

    void PlayerHealthUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerController.playerHealthComp.maxHealth += amount;
            _playerController.playerHealthComp.currentHealth = _playerController.playerHealthComp.maxHealth;
        }

    }

    void PlayerEnergyUpgrade(float amount, int cost)
    {
        if (_playerEnergyPoints.energyPoints >= cost)
        {
            _playerEnergyPoints.AddRemovePoints(-cost);
            _playerEnergy.playerMaxEnergy += amount;
            _playerEnergy.currentEnergy = _playerEnergy.playerMaxEnergy;
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
